using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizVerse.Data;
using QuizVerse.Models;
using QuizVerse.ViewModels;

namespace QuizVerse.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Dashboard()
    {
        var model = new AdminDashboardViewModel
        {
            TotalQuizzes = await _context.Quizzes.CountAsync(),
            PublishedQuizzes = await _context.Quizzes.CountAsync(q => q.IsPublished),
            TotalUsers = await _userManager.Users.CountAsync(),
            RecentQuizzes = await _context.Quizzes
                .Include(q => q.Questions)
                .OrderByDescending(q => q.Id)
                .Take(5)
                .ToListAsync()
        };
        return View(model);
    }
}
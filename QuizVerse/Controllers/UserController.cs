using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizVerse.Data;
using QuizVerse.Models;
using QuizVerse.ViewModels;

namespace QuizVerse.Controllers;

[Authorize(Roles = "User")]
public class UserController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Dashboard()
    {
        var user = await _userManager.GetUserAsync(User);

        var model = new UserDashboardViewModel
        {
            FullName = user?.FullName ?? "User",
            AvailableQuizzes = await _context.Quizzes
                .Include(q => q.Questions)
                .Where(q => q.IsPublished)
                .ToListAsync()
        };
        return View(model);
    }
}
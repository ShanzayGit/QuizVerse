using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizVerse.Data;
using QuizVerse.Models;
using QuizVerse.Repositories;
using QuizVerse.ViewModels;

namespace QuizVerse.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IUserRepository _User;
    private readonly IQuizRepository _Quiz;
    private readonly IAttemptRepository _Attempt;

    public AdminController(IUserRepository user, IQuizRepository quiz, IAttemptRepository attempt)
    {
        _User = user;
        _Quiz = quiz;
        _Attempt=attempt;
    }

    public async Task<IActionResult> Dashboard()
    {
        var model = new AdminDashboardViewModel
        {
            TotalQuizzes = await _Quiz.CountAsync(),
            PublishedQuizzes = await _Quiz.CountAsync(true),
            TotalUsers = await _User.CountAsync(),
            RecentQuizzes = (await _Quiz.GetAllAsync()).Take(5).ToList()
        };
        return View(model);
    }
}
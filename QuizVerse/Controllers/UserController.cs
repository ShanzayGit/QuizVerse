using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizVerse.Data;
using QuizVerse.Models;
using QuizVerse.Repositories;
using QuizVerse.ViewModels;

namespace QuizVerse.Controllers;

[Authorize(Roles = "User")]
public class UserController : Controller
{
    private readonly IQuizRepository _Quiz;
    private readonly IAttemptRepository _Attempt;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(IQuizRepository quiz, UserManager<ApplicationUser> userManager, IAttemptRepository attempt)
    {
        _Quiz = quiz;
        _userManager = userManager;
        _Attempt= attempt;
    }

    public async Task<IActionResult> Dashboard()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) { return Challenge(); }

        var model = new UserDashboardViewModel
        {
            FullName = user.FullName,
            AvailableQuizzes = (await _Quiz.GetAllAsync(publishedOnly: true)).ToList(),
            Stats = (await _Attempt.GetStatsByUserAsync(user.Id)),
            RecentAttempts=(await _Attempt.GetByUserAsync(user.Id)).Take(5).ToList()
        };
        return View(model);
    }
}
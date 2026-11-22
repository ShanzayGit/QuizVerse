using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizVerse.Models;
using QuizVerse.Repositories;

namespace QuizVerse.Controllers;

[Authorize]
public class AttemptsController : Controller
{
    private readonly IAttemptRepository _attempts;
    private readonly UserManager<ApplicationUser> _userManager;

    public AttemptsController(IAttemptRepository attempts, UserManager<ApplicationUser> userManager)
    {
        _attempts = attempts;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var attempts = User.IsInRole("Admin")
            ? await _attempts.GetAllAsync()
            : await _attempts.GetByUserAsync(_userManager.GetUserId(User)!);

        return View(attempts);
    }
}
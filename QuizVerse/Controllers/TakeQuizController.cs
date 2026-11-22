using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizVerse.Models;
using QuizVerse.Repositories;
using QuizVerse.ViewModels;

namespace QuizVerse.Controllers;

[Authorize(Roles = "User")]
public class TakeQuizController : Controller
{
    private readonly IQuizRepository _quizzes;
    private readonly IQuestionRepository _questions;
    private readonly IAttemptRepository _attempts;
    private readonly UserManager<ApplicationUser> _userManager;

    public TakeQuizController(IQuizRepository quizzes, IQuestionRepository questions,
                              IAttemptRepository attempts, UserManager<ApplicationUser> userManager)
    {
        _quizzes = quizzes;
        _questions = questions;
        _attempts = attempts;
        _userManager = userManager;
    }

    public async Task<IActionResult> Start(int id)
    {
        var quiz = await _quizzes.GetByIdAsync(id);
        if (quiz == null || !quiz.IsPublished) return NotFound();

        return View(new TakeQuizViewModel
        {
            Quiz = quiz,
            Questions = (await _questions.GetByQuizAsync(id)).ToList()
        });
    }

    // Called by $.ajax with a JSON body
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit([FromBody] SubmitQuizRequest request)
    {
        var quiz = await _quizzes.GetByIdAsync(request.QuizId);
        if (quiz == null || !quiz.IsPublished) return NotFound();

        var questions = (await _questions.GetByQuizAsync(request.QuizId)).ToList();
        var total = questions.Sum(q => q.Marks);
        var score = 0;

        foreach (var q in questions)
        {
            var answer = request.Answers.FirstOrDefault(a => a.QuestionId == q.Id);
            if (answer != null && q.Option.Any(o => o.Id == answer.OptionId && o.IsCorrect))
                score += q.Marks;
        }

        var now = DateTime.Now;
        await _attempts.CreateAsync(new Attempt
        {
            QuizId = quiz.Id,
            UserId = _userManager.GetUserId(User)!,
            Score = score,
            TotalMarks = total,
            StartTime = request.StartedOn > now ? now : request.StartedOn,
            EndTime = now
        });

        return Json(new { score, total, percent = total == 0 ? 0 : score * 100 / total });
    }
}
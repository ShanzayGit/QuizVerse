using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizVerse.Models;
using QuizVerse.Repositories;
using QuizVerse.ViewModels;

namespace QuizVerse.Controllers;

[Authorize(Roles = "Admin")]
public class QuestionsController : Controller
{
    private readonly IQuizRepository _quizzes;
    private readonly IQuestionRepository _questions;

    public QuestionsController(IQuizRepository quizzes, IQuestionRepository questions)
    {
        _quizzes = quizzes;
        _questions = questions;
    }

    public async Task<IActionResult> Index(int quizId)
    {
        var quiz = await _quizzes.GetByIdAsync(quizId);
        if (quiz == null) return NotFound();

        return View(new QuestionsPageViewModel
        {
            Quiz = quiz,
            Questions = (await _questions.GetByQuizAsync(quizId)).ToList()
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(QuestionFormViewModel form)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Enter the question text and all four options.";
            return RedirectToAction(nameof(Index), new { quizId = form.QuizId });
        }

        var texts = new[] { form.Option1, form.Option2, form.Option3, form.Option4 };
        var question = new Question
        {
            QuizId = form.QuizId,
            Text = form.Text,
            Marks = form.Marks,
            Option = texts.Select((t, i) => new AnswerOption
            {
                text = t,
                IsCorrect = i + 1 == form.CorrectOption
            }).ToList()
        };

        await _questions.AddAsync(question);
        return RedirectToAction(nameof(Index), new { quizId = form.QuizId });
    }

    [HttpDelete, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _questions.DeleteAsync(id);
        return Json(new { success = ok });
    }
}
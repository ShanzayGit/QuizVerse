using QuizVerse.Models;
using QuizVerse.Reposotories;
using Microsoft.AspNetCore.Mvc;
using QuizVerse.Repositories;
using System.Threading.Tasks;

namespace QuizVerse.Controllers
{
    public class QuizzesController : Controller
    {
        private readonly IQuizRepository _quiz;
        public QuizzesController(IQuizRepository quiz)
        {
            _quiz = quiz;
        }
        public async Task<IActionResult> Index()
        {
            
            return View(await _quiz.GetAllAsync());
        }
        [HttpGet]
        public async Task<IActionResult> Search(string? term)
        {
           return  PartialView("_QuizTable", await _quiz.GetAllAsync(term));
        }
        public IActionResult Create()
        {
            return View("Form", new Quiz());
        }
        public async Task<IActionResult> Edit(int id)
        {
            var selectedQuiz=await _quiz.GetByIdAsync(id);
            if (selectedQuiz != null)
            {
                return View("Form",selectedQuiz);
            }
            return NotFound();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Quiz quiz)
        {
            if (!ModelState.IsValid) return View("Form", quiz);

            if (quiz.Id == 0) await _quiz.CreateAsync(quiz);
            else await _quiz.UpdateAsync(quiz);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> TogglePublish(int id, bool publish)
        {
            var ok = await _quiz.SetPublishedAsync(id, publish);
            return Json(new { success = ok, isPublished = publish });
        }
        [HttpDelete, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _quiz.DeleteAsync(id);
            return Json( new { success = ok});
        }

    }
}

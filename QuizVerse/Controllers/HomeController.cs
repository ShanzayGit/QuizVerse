using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizVerse.Models;
using System.Diagnostics;
using QuizVerse.Data;
using QuizVerse.Repositories;

namespace QuizVerse.Controllers
{
    public class HomeController : Controller
    {
        private readonly IQuizRepository _Quiz;

        public HomeController(IQuizRepository quiz)
        {
            _Quiz = quiz;
        }

        public async Task<IActionResult> Index()
        {

            var published =( await _Quiz.GetAllAsync(publishedOnly:true)).ToList();
            ViewBag.QuizCount = published.Count;

            return View(published);
        }
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

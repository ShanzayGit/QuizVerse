using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizVerse.Models;
using System.Diagnostics;
using QuizVerse.Data;

namespace QuizVerse.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.QuizCount = await _context.Quizzes.CountAsync(q => q.IsPublished);

            var featured = await _context.Quizzes
                .Include(q => q.Questions)
                .Where(q => q.IsPublished)
                .OrderByDescending(q => q.Id)
                .Take(3)
                .ToListAsync();

            return View(featured);
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

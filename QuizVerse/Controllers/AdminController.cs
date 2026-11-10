using Microsoft.AspNetCore.Mvc;

namespace QuizVerse.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}

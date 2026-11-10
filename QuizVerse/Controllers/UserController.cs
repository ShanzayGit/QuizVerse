using Microsoft.AspNetCore.Mvc;

namespace QuizVerse.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}

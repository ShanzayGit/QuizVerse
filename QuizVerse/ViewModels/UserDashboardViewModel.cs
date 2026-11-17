using QuizVerse.Models;

namespace QuizVerse.ViewModels
{
    public class UserDashboardViewModel
    {
        public string FullName { get; set; } = string.Empty;
        public List<Quiz> AvailableQuizzes { get; set; } = new();
    }
}

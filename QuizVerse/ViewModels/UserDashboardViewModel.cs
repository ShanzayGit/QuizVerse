using QuizVerse.Models;

namespace QuizVerse.ViewModels
{
    public class UserDashboardViewModel
    {
        public string FullName { get; set; } = string.Empty;
        public AttemptStats Stats { get; set; } = new();
        public List<Attempt> RecentAttempts { get; set; } = new();

        public List<Quiz> AvailableQuizzes { get; set; } = new();
    }
}


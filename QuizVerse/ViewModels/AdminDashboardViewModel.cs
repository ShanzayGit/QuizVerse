using QuizVerse.Models;

namespace QuizVerse.ViewModels;

public class AdminDashboardViewModel
{
    public int TotalQuizzes { get; set; }
    public int PublishedQuizzes { get; set; }
    public int TotalUsers { get; set; }
    public int TotalAttempts { get; set; }

    public List<Quiz> RecentQuizzes { get; set; } = new();
}



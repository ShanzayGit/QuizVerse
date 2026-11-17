using System.ComponentModel.DataAnnotations;

namespace QuizVerse.Models;

public class Quiz
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [StringLength(300)]
    public string? Description { get; set; }

    public int TimeLimitMinutes { get; set; }
    public bool IsPublished { get; set; }

    public List<Question> Questions { get; set; } = new();
}
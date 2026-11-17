using System.ComponentModel.DataAnnotations;

namespace QuizVerse.Models;

public class Question
{
    public int Id { get; set; }
    public int QuizId { get; set; }
    public Quiz? Quiz { get; set; }

    [Required]
    public string Text { get; set; } = string.Empty;

    public int Marks { get; set; } = 1;
}
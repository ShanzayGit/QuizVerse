using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizVerse.Models;

public class Quiz
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [StringLength(300)]
    public string? Description { get; set; }
    [Display(Name ="Time Limit(minutes)")]
    public int TimeLimitMinutes { get; set
            ; }
    [Display(Name ="Published")]
    public bool IsPublished { get; set; }

    public List<Question> Questions { get; set; } = new();
    [NotMapped]
    public  int QuestionCount {  get; set; }
}
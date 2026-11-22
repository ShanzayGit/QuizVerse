using System.ComponentModel.DataAnnotations;
using QuizVerse.Models;

namespace QuizVerse.ViewModels;

public class QuestionFormViewModel
{
    public int QuizId { get; set; }

    [Required]
    public string Text { get; set; } = string.Empty;

    [Range(1, 20)]
    public int Marks { get; set; } = 1;

    [Required] public string Option1 { get; set; } = string.Empty;
    [Required] public string Option2 { get; set; } = string.Empty;
    [Required] public string Option3 { get; set; } = string.Empty;
    [Required] public string Option4 { get; set; } = string.Empty;

    [Range(1, 4)]
    public int CorrectOption { get; set; } = 1;
}

public class QuestionsPageViewModel
{
    public Quiz Quiz { get; set; } = new();
    public List<Question> Questions { get; set; } = new();
}

public class TakeQuizViewModel
{
    public Quiz Quiz { get; set; } = new();
    public List<Question> Questions { get; set; } = new();
}

public class AnswerDto
{
    public int QuestionId { get; set; }
    public int OptionId { get; set; }
}

public class SubmitQuizRequest
{
    public int QuizId { get; set; }
    public DateTime StartedOn { get; set; }
    public List<AnswerDto> Answers { get; set; } = new();
}
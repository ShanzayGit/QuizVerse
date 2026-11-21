using System.ComponentModel.DataAnnotations;

namespace QuizVerse.Models
{
    public class AnswerOption
    {
        public int Id { get; set; }
        public int QuestionId {  get; set; }
        public Question? Question {  get; set; }
        [Required]
        public string text {  get; set; }=string.Empty;
        public bool IsCorrect {  get; set; }
    }
}

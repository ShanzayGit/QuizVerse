using System.ComponentModel.DataAnnotations.Schema;

namespace QuizVerse.Models
{
    public class Attempt
    {
        public int Id { get; set; }

        public int QuizId { get; set; }
        public Quiz? Quiz { get; set; }

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

        public int Score { get; set; }
        public int TotalMarks { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        [NotMapped] public string UserName { get; set; } = string.Empty;
        [NotMapped] public string QuizTitle { get; set; } = string.Empty;
        [NotMapped] public decimal Percentage => TotalMarks == 0 ? 0 : Score * 100 / TotalMarks;
    }
}
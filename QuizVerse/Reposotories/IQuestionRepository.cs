using QuizVerse.Models;

namespace QuizVerse.Repositories;

public interface IQuestionRepository
{
    Task<IEnumerable<Question>> GetByQuizAsync(int quizId);
    Task<int> AddAsync(Question question);
    Task<bool> DeleteAsync(int id);
}
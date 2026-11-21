using QuizVerse.Models;

namespace QuizVerse.Repositories;

public interface IQuizRepository
{
    Task<IEnumerable<Quiz>> GetAllAsync(string? search = null, bool publishedOnly = false);
    Task<Quiz?> GetByIdAsync(int id);
    Task<int> CreateAsync(Quiz quiz);
    Task<bool> UpdateAsync(Quiz quiz);
    Task<bool> DeleteAsync(int id);
    Task<bool> SetPublishedAsync(int id, bool isPublished);
    Task<int> CountAsync(bool publishedOnly = false);
}
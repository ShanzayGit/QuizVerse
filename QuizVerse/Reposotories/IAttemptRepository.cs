using QuizVerse.Models;

namespace QuizVerse.Repositories;

public interface IAttemptRepository
{
    Task<int> CreateAsync(Attempt attempt);
    Task<IEnumerable<Attempt>> GetByUserAsync(string userId);
    Task<IEnumerable<Attempt>> GetAllAsync();
    Task<AttemptStats> GetStatsByUserAsync(string userId);
    Task<int> CountAsync();
}
using QuizVerse.Models;

namespace QuizVerse.Repositories;

public interface IUserRepository
{
    Task<int> CountAsync();
    Task<IEnumerable<UserListItem>> GetAllAsync();
}
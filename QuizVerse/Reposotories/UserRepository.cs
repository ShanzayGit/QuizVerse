using QuizVerse.Data;
using QuizVerse.Models;
using QuizVerse.Repositories;
using Dapper;
namespace QuizVerse.Reposotories
{
    public class UserRepository:IUserRepository
    {

        private readonly DapperContext _context;

        public UserRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> CountAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM AspNetUsers");
        }

        public async Task<IEnumerable<UserListItem>> GetAllAsync()
        {
            const string sql = @"
    SELECT u.Id, u.FullName, u.Email, u.RegisteredOn, ISNULL(r.Name, 'None') AS Role
    FROM AspNetUsers u
    LEFT JOIN AspNetUserRoles ur ON ur.UserId = u.Id
    LEFT JOIN AspNetRoles r ON r.Id = ur.RoleId
    ORDER BY u.FullName";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<UserListItem>(sql);
        }
    }
}

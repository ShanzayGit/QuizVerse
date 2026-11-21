using QuizVerse.Data;
using QuizVerse.Models;
using Dapper;
using QuizVerse.Repositories;
namespace QuizVerse.Reposotories
{
    public class AttemptRepository:IAttemptRepository
    {

        private readonly DapperContext _context;

        public AttemptRepository(DapperContext context)
        {
            _context = context;
        }

        private const string SelectAttempt = @"
        SELECT a.Id, a.QuizId, a.UserId, a.Score, a.TotalMarks, a.StartTime, a.EndTime,
               q.Title AS QuizTitle, u.FullName AS UserName
        FROM Attempts a
        INNER JOIN Quizzes q ON q.Id = a.QuizId
        INNER JOIN AspNetUsers u ON u.Id = a.UserId";

        public async Task<int> CreateAsync(Attempt attempt)
        {
            const string sql = @"
            INSERT INTO Attempts (QuizId, UserId, Score, TotalMarks, StartTime, EndTime)
            VALUES (@QuizId, @UserId, @Score, @TotalMarks, @StartTime, @EndTime);
            SELECT CAST(SCOPE_IDENTITY() AS int);";

            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, attempt);
        }

        public async Task<IEnumerable<Attempt>> GetByUserAsync(string userId)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Attempt>(
                SelectAttempt + " WHERE a.UserId = @UserId ORDER BY a.EndTime DESC",
                new { UserId = userId });
        }

        public async Task<IEnumerable<Attempt>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Attempt>(SelectAttempt + " ORDER BY a.EndTime DESC");
        }

        public async Task<AttemptStats> GetStatsByUserAsync(string userId)
        {
            const string sql = @"
            SELECT COUNT(*) AS TotalAttempts,
                   ISNULL(AVG(Score * 100.0 / NULLIF(TotalMarks, 0)), 0) AS AveragePercent,
                   ISNULL(MAX(Score * 100.0 / NULLIF(TotalMarks, 0)), 0) AS BestPercent
            FROM Attempts
            WHERE UserId = @UserId";

            using var connection = _context.CreateConnection();
            return await connection.QuerySingleAsync<AttemptStats>(sql, new { UserId = userId });
        }

        public async Task<int> CountAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Attempts");
        }
    }
}

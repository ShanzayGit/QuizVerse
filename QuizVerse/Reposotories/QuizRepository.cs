using Dapper;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using QuizVerse.Data;
using QuizVerse.Models;
using QuizVerse.Repositories;
using System.Data;
namespace QuizVerse.Reposotories
{
    public class QuizRepository: IQuizRepository
    {
        private readonly DapperContext _context;
        public QuizRepository(DapperContext context)
        {
            _context = context;
        }
        private const string SelectQuiz = @"
        SELECT q.Id, q.Title, q.Description, q.TimeLimitMinutes, q.IsPublished,
               (SELECT COUNT(*) FROM Questions WHERE QuizId = q.Id) AS QuestionCount
        FROM Quizzes q";
        public async Task<IEnumerable<Quiz>> GetAllAsync(string? search = null, bool publishedOnly = false)
        {
            var sql = SelectQuiz + " WHERE 1 = 1";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(search))
            {
                sql += " AND q.Title LIKE @Search";
                parameters.Add("Search", "%" + search.Trim() + "%");
            }

            if (publishedOnly)
            {
                sql += " AND q.IsPublished = 1";
            }

            sql += " ORDER BY q.Id DESC";

            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Quiz>(sql, parameters);
        }
        public async Task<Quiz?> GetByIdAsync(int id)
        {
            var query = SelectQuiz + @"WHERE q.Id=@id";
            using var connection=_context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Quiz>(query, new {Id=id});
        }
        public async Task<int> CreateAsync(Quiz quiz)
        {
            var query = @"INSERT INTO Quizzes (Title, Description,  TimeLimitMinutes,IsPublished)
                        VALUES(@Title,@Description,@TimeLimitMinutes,@IsPublished)
                        SELECT CAST(SCOPE_IDENTITY() AS int);";
            var connection= _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query,quiz);
        }
        public async Task<bool> UpdateAsync(Quiz quiz)
        {
            var query = @"UPDATE Quizzes
                       SET Title=@Title,Description=@Description,TimeLimitMinutes=@TimeLimitMinutes,IsPublished=@IsPublished
                       WHERE Id=@Id";
            using var connection=_context.CreateConnection();
            return await connection.ExecuteAsync(query, quiz)>0;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var query = @"DELETE from Quizzes WHERE Id=@Id";
            using var connection=_context.CreateConnection();
            return await connection.ExecuteAsync(query, new { Id = id }) > 0;
        }
        public async Task<bool> SetPublishedAsync(int id, bool isPublished)
        {
            var query = @"UPDATE Quizzes 
                      SET IsPublished=@IsPublished 
                      WHERE Id=@Id";
            using var connection=_context.CreateConnection();
            return await connection.ExecuteAsync(query, new {Id=id, IsPublished=isPublished})> 0;
        }
        public async Task<int> CountAsync(bool publishedOnly = false)
        {
            var sql = "SELECT COUNT(*) FROM Quizzes";
            if (publishedOnly) sql += " WHERE IsPublished = 1";

            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql);
        }
    }
}

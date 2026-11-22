using Dapper;
using QuizVerse.Models;
using QuizVerse.Data;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using System.Data;
using QuizVerse.Repositories;
namespace QuizVerse.Reposotories
{
    public class QuestionRepository:IQuestionRepository
    {
        private readonly DapperContext _context;
        public QuestionRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Question>> GetByQuizAsync(int quizId)
        {
            const string sql = @"
        SELECT Id, QuizId, [Text], Marks FROM Questions WHERE QuizId = @QuizId ORDER BY Id;

        SELECT o.Id, o.QuestionId, o.[Text], o.IsCorrect
        FROM AnswerOptions o
        INNER JOIN Questions q ON q.Id = o.QuestionId
        WHERE q.QuizId = @QuizId
        ORDER BY o.Id;";

            using var connection = _context.CreateConnection();
            using var multi = await connection.QueryMultipleAsync(sql, new { QuizId = quizId });

            var questions = (await multi.ReadAsync<Question>()).ToList();
            var options = (await multi.ReadAsync<AnswerOption>()).ToList();

            foreach (var question in questions)
                question.Option = options.Where(o => o.QuestionId == question.Id).ToList();

            return questions;
        }
        public async Task<int> AddAsync(Question question)
        {
            using var connection = _context.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                var questionId = await connection.ExecuteScalarAsync<int>(@"
                INSERT INTO Questions (QuizId, [Text], Marks) VALUES (@QuizId, @Text, @Marks);
                SELECT CAST(SCOPE_IDENTITY() AS int);", question, transaction);

                foreach (var option in question.Option)
                {
                    option.QuestionId = questionId;
                    await connection.ExecuteAsync(
                        "INSERT INTO AnswerOptions (QuestionId, [Text], IsCorrect) VALUES (@QuestionId, @Text, @IsCorrect)",
                        option, transaction);
                }

                transaction.Commit();
                return questionId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync("DELETE FROM Questions WHERE Id = @Id", new { Id = id }) > 0;
        }
    }
}

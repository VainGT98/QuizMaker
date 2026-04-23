using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using QuizMakerModel;
using System.Data;
using System.IO;
using System.Windows;

namespace QuizMakerDAL
{
    public class DALQuiz
    {
        public SqlConnection SqlConn { get; private set; }

        private IConfiguration _config;

        public DALQuiz()
        {
            try
            {
                _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

                string? connectionString = _config.GetConnectionString("QuizConnectionString");

                SqlConn = new SqlConnection(connectionString);
                SqlConn.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error connecting to database: " + e.Message);
            }
        }

        public int InsertQuiz(QuizModel quiz)
        {
            try
            {
                using var cmd = new SqlCommand(
                "INSERT INTO Quiz (Title, CreationDate, UserId) VALUES (@t, @cd, @u); SELECT SCOPE_IDENTITY();",
                SqlConn);
                cmd.Parameters.AddWithValue("@t", quiz.Title);
                cmd.Parameters.AddWithValue("@cd", DateTime.Now);
                cmd.Parameters.AddWithValue("@u", quiz.UserID);

                //using var cmd2 = new SqlCommand("SELECT SCOPE_IDENTITY();", SqlConn);
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }
                else
                {
                    throw new Exception("Failed to retrieve the inserted Quiz ID.");
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteQuiz(QuizModel quiz)
        {
            try
            {
                using var cmd = new SqlCommand(
                "DELETE FROM Quiz WHERE ID = @id",
                SqlConn);
                cmd.Parameters.AddWithValue("@id", quiz.QuizID);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }

        }

        public void UpdateQuiz(QuizModel quiz, SqlConnection connection, SqlTransaction transaction = null)
        {
            try
            {
                using var cmd = new SqlCommand(
                "UPDATE Quiz SET Title=@t, LastModifiedDate=@lmd WHERE ID=@id",
                connection, transaction);
                cmd.Parameters.AddWithValue("@t", quiz.Title);
                cmd.Parameters.AddWithValue("@lmd", DateTime.Now);
                cmd.Parameters.AddWithValue("@id", quiz.QuizID);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
        }

        public List<QuizModel> GetQuizzesByUserID(int userID)
        {
            try
            {
                List<QuizModel> quizzes = new List<QuizModel>();

                using var cmd = new SqlCommand(
                "SELECT * FROM Quiz WHERE UserId = @id",
                SqlConn);
                cmd.Parameters.AddWithValue("@id", userID);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    quizzes.Add(new QuizModel
                    {
                        QuizID = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        CreationDate = reader.GetDateTime(2),
                        LastModifiedDate = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                        UserID = reader.GetInt32(4)
                    });
                }
                return quizzes;
            }
            catch
            {
                throw;
            }
        }

        public QuizModel GetQuizByQuizID(int quizID)
        {
            try
            {
                QuizModel quiz = null;

                using var cmd = new SqlCommand(
                "SELECT * FROM Quiz WHERE ID = @id",
                SqlConn);
                cmd.Parameters.AddWithValue("@id", quizID);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    quiz = new QuizModel
                    {
                        QuizID = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        CreationDate = reader.GetDateTime(2),
                        LastModifiedDate = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                        UserID = reader.GetInt32(4)
                    };
                }
                quiz.QuestionsList = new DALQuestion().GetQuestionsByQuizID(quizID);
                return quiz;
            }
            catch
            {
                throw;
            }
        }

    }
}

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using QuizMakerModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuizMakerDAL
{
    public class DALQuestion
    {
        public SqlConnection SqlConn { get; private set; }

        private Microsoft.Extensions.Configuration.IConfiguration _config;

        public DALQuestion()
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

                //string config = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                //SqlConn = new SqlConnection(config);
                //SqlConn.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error connecting to database: " + e.Message);
            }
        }

        public int InsertQuestion(QuestionModel question, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            try
            {
                SqlCommand cmd;
                if (connection == null)
                {
                    cmd = new SqlCommand(
                        "INSERT INTO Question (QuizId, [Text], ImagePath, OrderNumber, Image) VALUES (@q, @t, @ip, @on, @img); SELECT SCOPE_IDENTITY();",
                        SqlConn,
                        transaction
                    );
                }
                else
                {
                    cmd = new SqlCommand(
                        "INSERT INTO Question (QuizId, [Text], ImagePath, OrderNumber, Image) VALUES (@q, @t, @ip, @on, @img); SELECT SCOPE_IDENTITY();",
                        connection,
                        transaction
                    );
                }
                using (cmd)
                {
                    cmd.Parameters.AddWithValue("@q", question.QuizID);
                    cmd.Parameters.AddWithValue("@t", question.Text);
                    cmd.Parameters.AddWithValue("@ip", string.IsNullOrWhiteSpace(question.ImagePath) ? (object)DBNull.Value : question.ImagePath);
                    cmd.Parameters.AddWithValue("@on", question.OrderNumber);
                    cmd.Parameters.Add("@img", SqlDbType.VarBinary, -1).Value = question.Image ?? (object)DBNull.Value;

                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        throw new Exception("Failed to retrieve the inserted Question ID.");
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteQuestion(QuestionModel question, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            try
            {
                SqlCommand cmd;
                if (connection == null)
                {
                    cmd = new SqlCommand(
                        "DELETE FROM Question WHERE ID = @id",
                        SqlConn,
                        transaction
                    );
                }
                else
                {
                    cmd = new SqlCommand(
                        "DELETE FROM Question WHERE ID = @id",
                        connection,
                        transaction
                    );
                }
                using (cmd)
                {
                    cmd.Parameters.AddWithValue("@id", question.QuestionID);
                    cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                throw;
            }

        }

        public void UpdateQuestion(QuestionModel question, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            try
            {
                SqlCommand cmd;
                if (connection == null)
                {
                    cmd = new SqlCommand(
                        "UPDATE Question SET [Text]=@t, ImagePath=@ip, OrderNumber=@on, Image=@img WHERE ID=@id",
                        SqlConn,
                        transaction
                    );
                }
                else
                {
                    cmd = new SqlCommand(
                        "UPDATE Question SET [Text]=@t, ImagePath=@ip, OrderNumber=@on, Image=@img WHERE ID=@id",
                        connection,
                        transaction
                    );
                }
                using (cmd)
                {
                    cmd.Parameters.AddWithValue("@t", question.Text);
                    cmd.Parameters.AddWithValue("@ip", string.IsNullOrWhiteSpace(question.ImagePath) ? (object)DBNull.Value : question.ImagePath);
                    cmd.Parameters.AddWithValue("@on", question.OrderNumber);
                    cmd.Parameters.AddWithValue("@id", question.QuestionID);
                    cmd.Parameters.Add("@img", SqlDbType.VarBinary, -1).Value = question.Image ?? (object)DBNull.Value;
                    cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<QuestionModel> GetQuestionsByQuizID(int quizID, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            try
            {
                List<QuestionModel> questions = new List<QuestionModel>();

                SqlCommand cmd;
                if (connection == null)
                {
                    cmd = new SqlCommand(
                        "SELECT * FROM Question WHERE QuizID = @q ORDER BY OrderNumber",
                        SqlConn, transaction);
                }
                else
                {
                    cmd = new SqlCommand(
                        "SELECT * FROM Question WHERE QuizID = @q ORDER BY OrderNumber",
                        connection, transaction);
                }
                using (cmd)
                {
                    cmd.Parameters.AddWithValue("@q", quizID);
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        QuestionModel question = new QuestionModel
                        {
                            QuestionID = reader.GetInt32(0),
                            OrderNumber = reader.GetInt32(1),
                            Text = reader.GetString(2),
                            ImagePath = reader.IsDBNull(3) ? null : reader.GetString(3),
                            QuizID = reader.GetInt32(4),
                            Image = reader.IsDBNull(5) ? null : (byte[])reader[5]
                        };
                        questions.Add(question);
                    }
                }
                foreach (var question in questions)
                {
                    question.AnswersList = new DALAnswer().GetAnswersByQuestionID(question.QuestionID.Value, connection, transaction);
                }
                return questions;
            }
            catch
            {
                throw;
            }
        }

        public int GetQuestionCountByQuizID(int quizID, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            try
            {
                SqlCommand cmd;
                if (connection == null)
                {
                    cmd = new SqlCommand(
                        "SELECT COUNT(*) FROM Question WHERE QuizId = @q",
                        SqlConn, transaction);
                }
                else
                {
                    cmd = new SqlCommand(
                        "SELECT COUNT(*) FROM Question WHERE QuizId = @q",
                        connection, transaction);
                }
                using (cmd)
                {
                    cmd.Parameters.AddWithValue("@q", quizID);
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        throw new Exception("Failed to retrieve the question count.");
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public void UpdateQuestionOrder(int questionID, int newOrderNumber, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            try
            {
                SqlCommand cmd;
                if (connection == null)
                {
                    cmd = new SqlCommand(
                        "UPDATE Question SET OrderNumber = @on WHERE ID = @id",
                        SqlConn,
                        transaction
                    );
                }
                else
                {
                    cmd = new SqlCommand(
                        "UPDATE Question SET OrderNumber = @on WHERE ID = @id",
                        connection,
                        transaction
                    );
                }
                using (cmd)
                {
                    cmd.Parameters.AddWithValue("@on", newOrderNumber);
                    cmd.Parameters.AddWithValue("@id", questionID);
                    cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}

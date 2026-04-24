using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using QuizMakerModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuizMakerDAL
{
    public class DALAnswer
    {
        public SqlConnection SqlConn { get; private set; }

        private Microsoft.Extensions.Configuration.IConfiguration _config;

        public DALAnswer()
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

                //string config = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                //SqlConn = new SqlConnection(config);

                //SqlConn.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error connecting to database: " + e.Message);
            }
        }

        public int InsertAnswer(AnswerModel answer, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            try
            {
                SqlCommand cmd;
                if (connection == null)
                {
                    cmd = new SqlCommand(
                    "INSERT INTO Answer (QuestionID, [Text], IsCorrect, OrderNumber) VALUES (@q, @t, @c, @on); SELECT SCOPE_IDENTITY();",
                    SqlConn, transaction);
                }
                else
                {
                    cmd = new SqlCommand(
                    "INSERT INTO Answer (QuestionID, [Text], IsCorrect, OrderNumber) VALUES (@q, @t, @c, @on); SELECT SCOPE_IDENTITY();",
                    connection, transaction);
                }
                using (cmd)
                {
                    cmd.Parameters.AddWithValue("@q", answer.QuestionID);
                    cmd.Parameters.AddWithValue("@t", answer.Text);
                    cmd.Parameters.AddWithValue("@c", answer.IsCorrect);
                    cmd.Parameters.AddWithValue("@on", answer.OrderNumber);

                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        throw new Exception("Failed to retrieve the inserted Answer ID.");
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public void UpdateAnswer(AnswerModel answer, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            try
            {
                SqlCommand cmd;
                if (connection == null)
                {
                    cmd = new SqlCommand(
                    "UPDATE Answer SET [Text]=@t, IsCorrect=@c, OrderNumber=@on WHERE ID=@a",
                    SqlConn, transaction);
                }
                else
                {
                    cmd = new SqlCommand(
                    "UPDATE Answer SET [Text]=@t, IsCorrect=@c, OrderNumber=@on WHERE ID=@a",
                    connection, transaction);
                }
                using (cmd)
                {
                    cmd.Parameters.AddWithValue("@t", answer.Text);
                    cmd.Parameters.AddWithValue("@c", answer.IsCorrect);
                    cmd.Parameters.AddWithValue("@on", answer.OrderNumber);
                    cmd.Parameters.AddWithValue("@a", answer.AnswerID);
                    cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteAnswer(AnswerModel answer, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            try
            {
                SqlCommand cmd;
                if (connection == null)
                {
                    cmd = new SqlCommand(
                    "DELETE FROM Answer WHERE ID=@a",
                    SqlConn, transaction);
                }
                else
                {
                    cmd = new SqlCommand(
                    "DELETE FROM Answer WHERE ID=@a",
                    connection, transaction);
                }
                using (cmd)
                {
                    cmd.Parameters.AddWithValue("@a", answer.AnswerID);
                    cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<AnswerModel> GetAnswersByQuestionID(int? questionID, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            List<AnswerModel> answers = new List<AnswerModel>();
            try
            {
                SqlCommand cmd;
                if (connection == null)
                {
                    cmd = new SqlCommand(
                    "SELECT * FROM Answer WHERE QuestionID=@q ORDER BY OrderNumber",
                    SqlConn, transaction);
                }
                else
                {
                    cmd = new SqlCommand(
                    "SELECT * FROM Answer WHERE QuestionID=@q ORDER BY OrderNumber",
                    connection, transaction);
                }
                using (cmd)
                {
                    cmd.Parameters.AddWithValue("@q", questionID);
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        answers.Add(new AnswerModel
                        {
                            AnswerID = reader.GetInt32(0),
                            OrderNumber = reader.GetInt32(1),
                            Text = reader.GetString(2),
                            IsCorrect = reader.GetBoolean(3),
                            QuestionID = reader.GetInt32(4)
                        });
                    }
                }
            }
            catch
            {
                throw;
            }
            return answers;
        }

        public int GetAnswerCountByQuestionID(int questionID, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            try
            {
                SqlCommand cmd;
                if (connection == null)
                {
                    cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM Answer WHERE QuestionId=@q",
                    SqlConn, transaction);
                }
                else
                {
                    cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM Answer WHERE QuestionId=@q",
                    connection, transaction);
                }
                using (cmd)
                {
                    cmd.Parameters.AddWithValue("@q", questionID);
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        throw new Exception("Failed to retrieve the count of Answers.");
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}

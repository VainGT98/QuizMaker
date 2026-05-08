using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using QuizMakerDAL;
using QuizMakerModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuizMakerBLL
{
    public class BLLQuiz
    {
        public SqlConnection SqlConn { get; private set; }

        private IConfiguration _config;

        public BLLQuiz()
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

                //string config = System.Configuration.ConfigurationManager.ConnectionStrings["QuizConnectionString"].ConnectionString;
                //SqlConn = new SqlConnection(config);
                //SqlConn.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error connecting to database: " + e.Message);
            }
        }

        public void InsertQuiz(QuizModel quiz)
        {
            try
            {
                DALQuiz dalQuiz = new DALQuiz();
                int quizId = dalQuiz.InsertQuiz(quiz);
                foreach (QuestionModel question in quiz.QuestionsList)
                {
                    question.QuizID = quizId;
                    BLLQuestion bLLQuestion = new BLLQuestion();
                    bLLQuestion.InsertQuestion(question);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteQuiz(QuizModel quiz, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            try
            {
                foreach (QuestionModel question in quiz.QuestionsList)
                {
                    BLLQuestion bLLQuestion = new BLLQuestion();
                    bLLQuestion.DeleteQuestion(question, connection, transaction);
                }
                DALQuiz dalQuiz = new DALQuiz();
                dalQuiz.DeleteQuiz(quiz, connection, transaction);
            }
            catch
            {
                throw;
            }
        }

        public void UpdateQuiz(QuizModel quiz)
        {
            SqlTransaction transaction = SqlConn.BeginTransaction();

            BLLQuestion bLLQuestion = new BLLQuestion();
            try
            {
                DALQuiz dalQuiz = new DALQuiz();
                dalQuiz.UpdateQuiz(quiz, SqlConn, transaction);
                foreach (QuestionModel question in quiz.QuestionsList)
                {
                    question.QuizID = quiz.QuizID.Value;
                    if (question.QuestionID == null)
                    {
                        bLLQuestion.InsertQuestion(question, SqlConn ,transaction);
                    }
                    else
                    {
                        bLLQuestion.UpdateQuestion(question, SqlConn, transaction);
                    }
                }
                List<QuestionModel> questionsDB = bLLQuestion.GetQuestionsByQuizID(quiz.QuizID.Value, SqlConn, transaction);

                var questionIdsUI = quiz.QuestionsList.Select(q => q.QuestionID).ToList();
                var questionIdsDB = questionsDB.Select(q => q.QuestionID).ToList();
                foreach (int questionId in questionIdsDB)
                {
                    if (!questionIdsUI.Contains(questionId))
                    {
                        QuestionModel question = questionsDB.Where(q => q.QuestionID == questionId).FirstOrDefault();
                        if (question != null)
                            bLLQuestion.DeleteQuestion(question, SqlConn, transaction);
                    }
                }
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public List<QuizModel> GetQuizzesByUserID(int userID, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            try
            {
                DALQuiz dalQuiz = new DALQuiz();
                return dalQuiz.GetQuizzesByUserID(userID, connection, transaction);
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
                DALQuiz dalQuiz = new DALQuiz();
                return dalQuiz.GetQuizByQuizID(quizID);
            }
            catch
            {
                throw;
            }
        }
    }
}

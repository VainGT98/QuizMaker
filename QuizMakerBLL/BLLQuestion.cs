using Microsoft.Data.SqlClient;
using QuizMakerDAL;
using QuizMakerModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMakerBLL
{
    public class BLLQuestion
    {
        public int InsertQuestion(QuestionModel question, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            try
            {
                DALQuestion dalQuestion = new DALQuestion();
                int questionId = dalQuestion.InsertQuestion(question, connection, transaction);
                question.QuestionID = questionId;
                foreach (AnswerModel answer in question.AnswersList)
                {
                    answer.QuestionID = questionId;
                    BLLAnswer bLLAnswer = new BLLAnswer();
                    bLLAnswer.InsertAnswer(answer, connection, transaction);
                }
                return questionId;
            }
            catch
            {
                throw;
            }
        }

        public void UpdateQuestion(QuestionModel question, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            BLLAnswer bLLAnswer = new BLLAnswer();
            try
            {
                DALQuestion dalQuestion = new DALQuestion();
                dalQuestion.UpdateQuestion(question, connection, transaction);

                foreach (AnswerModel answer in question.AnswersList)
                {
                    answer.QuestionID = question.QuestionID.Value;
                    if (answer.AnswerID == null)
                    {
                        bLLAnswer.InsertAnswer(answer, connection, transaction);
                    }
                    else
                    {
                        bLLAnswer.UpdateAnswer(answer, connection, transaction);
                    }
                }
                List<AnswerModel> answersDB = bLLAnswer.GetAnswersByQuestionID(question.QuestionID.Value, connection, transaction);

                var answerIdsUI = question.AnswersList.Select(a => a.AnswerID).ToList();
                var answerIdsDB = answersDB.Select(a => a.AnswerID).ToList();
                foreach (var answerId in answerIdsDB)
                {
                    if (!answerIdsUI.Contains(answerId))
                    {
                        AnswerModel answer = answersDB.Where(a => a.AnswerID == answerId).FirstOrDefault();
                        if (answer != null)
                            bLLAnswer.DeleteAnswer(answer, connection, transaction);
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
                foreach (AnswerModel answer in question.AnswersList)
                {
                    BLLAnswer bLLAnswer = new BLLAnswer();
                    bLLAnswer.DeleteAnswer(answer, connection, transaction);
                }

                DALQuestion dalQuestion = new DALQuestion();
                dalQuestion.DeleteQuestion(question, connection, transaction);
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
                DALQuestion dalQuestion = new DALQuestion();
                return dalQuestion.GetQuestionsByQuizID(quizID, connection, transaction);
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
                DALQuestion dalQuestion = new DALQuestion();
                return dalQuestion.GetQuestionCountByQuizID(quizID, connection, transaction);
            }
            catch
            {
                throw;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using QuizMakerDAL;
using QuizMakerModel;

namespace QuizMakerBLL
{
    public class BLLAnswer
    {
        public int InsertAnswer(AnswerModel answer, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            try
            {
                DALAnswer dalAnswer = new DALAnswer();
                int answerId = dalAnswer.InsertAnswer(answer, connection, transaction);
                answer.AnswerID = answerId;
                return answerId;
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
                DALAnswer dalAnswer = new DALAnswer();
                dalAnswer.UpdateAnswer(answer, connection, transaction);
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
                DALAnswer dalAnswer = new DALAnswer();
                dalAnswer.DeleteAnswer(answer, connection, transaction);
            }
            catch
            {
                throw;
            }
        }

        public List<AnswerModel> GetAnswersByQuestionID(int questionID, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            try
            {
                DALAnswer dalAnswer = new DALAnswer();
                return dalAnswer.GetAnswersByQuestionID(questionID, connection, transaction);
            }
            catch
            {
                throw;
            }
        }

        public int GetAnswerCountByQuestionID(int questionID, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            try
            {
                DALAnswer dalAnswer = new DALAnswer();
                return dalAnswer.GetAnswerCountByQuestionID(questionID, connection, transaction);
            }
            catch
            {
                throw;
            }
        }
    }
}

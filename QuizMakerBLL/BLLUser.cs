using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using QuizMakerDAL;
using QuizMakerModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace QuizMakerBLL
{
    public class BLLUser
    {
        public SqlConnection SqlConn { get; private set; }

        private IConfiguration _config;

        public BLLUser()
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


        public int FindUserID(string username)
        {
            try
            {
                DALUser dalUser = new DALUser();
                return dalUser.FindUserID(username);
            }
            catch
            {
                throw;
            }
        }

        public void DeleteUser(int userID)
        {
            SqlTransaction transaction = SqlConn.BeginTransaction();

            try
            {   
                List<QuizModel> quizzes = new BLLQuiz().GetQuizzesByUserID(userID, SqlConn, transaction);
                foreach (QuizModel quiz in quizzes)
                {
                    new BLLQuiz().DeleteQuiz(quiz, SqlConn, transaction);
                }
                DeleteUserRoles(userID, SqlConn, transaction);
                DALUser dalUser = new DALUser();
                dalUser.DeleteUser(userID, SqlConn, transaction);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public void AssignRoleToUser(int userID, int roleID)
        {
            try
            {
                DALUser dalUser = new DALUser();
                dalUser.AssignRoleToUser(userID, roleID);
            }
            catch
            {
                throw;
            }
        }

        public void DeleteUserRoles(int userID, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            try
            {
                DALUser dalUser = new DALUser();
                dalUser.DeleteUserRoles(userID);
            }
            catch
            {
                throw;
            }
        }
    }
}

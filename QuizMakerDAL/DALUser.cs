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
    public class DALUser
    {
        public SqlConnection SqlConn { get; private set; }

        private Microsoft.Extensions.Configuration.IConfiguration _config;

        public DALUser()
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

        public int FindUserID(string username)
        {
            try
            {
                int userID;
                using (SqlConn)
                {
                    using var cmd = new SqlCommand(
                    "SELECT ID FROM [User] WHERE Username=@u",
                    SqlConn);
                    cmd.Parameters.AddWithValue("@u", username);
                    userID = (int)cmd.ExecuteScalar();
                }
                return userID;
            }
            catch
            {
                throw;
            }
        }
        public void DeleteUser(int userID, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            try
            {
                SqlCommand cmd;

                if (connection == null)
                {
                    cmd = new SqlCommand(
                        "DELETE FROM [User] WHERE ID=@id",
                        SqlConn,
                        transaction
                    );
                }
                else
                {
                    cmd = new SqlCommand(
                        "DELETE FROM [User] WHERE ID=@id",
                        connection,
                        transaction
                    );
                }

                cmd.Parameters.AddWithValue("@id", userID);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
        }

        public void AssignRoleToUser(int userID, int roleID)
        {
            try
            {
                using (SqlConn)
                {
                    using var cmd = new SqlCommand(
                    "INSERT INTO UsersRoles (UserId, RoleId) VALUES (@userID, @roleID)",
                    SqlConn);
                    cmd.Parameters.AddWithValue("@userID", userID);
                    cmd.Parameters.AddWithValue("@roleID", roleID);
                    cmd.ExecuteNonQuery();
                }
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
                SqlCommand cmd;

                if (connection == null)
                {
                    cmd = new SqlCommand(
                        "DELETE FROM UsersRoles WHERE UserId=@userID",
                        SqlConn,
                        transaction
                    );
                }
                else
                {
                    cmd = new SqlCommand(
                        "DELETE FROM UsersRoles WHERE UserId=@userID",
                        connection,
                        transaction
                    );
                }

                cmd.Parameters.AddWithValue("@userID", userID);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
        }
    }
}

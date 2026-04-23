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
    }
}

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using QuizMaker.QuizMakerModel;
using QuizMakerModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuizMaker.QuizMakerDAL
{
    public class DALAuthenticate
    {
        
        public SqlConnection SqlConn { get; private set; }

        private IConfiguration _config;

        public DALAuthenticate()
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
                throw;
            }
        }
        public bool RegisterDAL(UserModel user, string hash, string salt)
        {
            try
            {
                using (SqlConn)
                {
                    using (var check = new SqlCommand("SELECT COUNT(*) FROM [User] WHERE Username=@u", SqlConn))
                    {
                        check.Parameters.AddWithValue("@u", user.Username);

                        int count = (int)check.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("Username already exists.");
                            return false;
                        }
                    }

                    // inserisce nuovo utente
                    using var cmd = new SqlCommand(
                    "INSERT INTO [User] (Username, Hash, Salt) VALUES (@u, @h, @s)",
                    SqlConn);

                    cmd.Parameters.AddWithValue("@u", user.Username);
                    cmd.Parameters.AddWithValue("@h", hash);
                    cmd.Parameters.AddWithValue("@s", salt);

                    cmd.ExecuteNonQuery();

                    return true;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public bool LoginDAL(UserModel user)
        {
            try
            {
                using (SqlConn)
                {
                    using var cmd = new SqlCommand(
                    "SELECT Hash, Salt FROM [User] WHERE Username=@u", SqlConn);

                    cmd.Parameters.AddWithValue("@u", user.Username);

                    using var reader = cmd.ExecuteReader();

                    if (!reader.Read())
                    {
                        MessageBox.Show("Username does not exist");
                        return false;
                    }

                    string hash = reader.GetString(0);
                    string salt = reader.GetString(1);

                    PasswordHasher passwordHasher = new PasswordHasher();

                    bool verifyPassword = passwordHasher.VerifyPassword(user.Password, hash, salt);

                    return verifyPassword;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}

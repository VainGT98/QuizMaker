using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using QuizMakerModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuizMakerDAL
{
    public class DALRole
    {
        public SqlConnection SqlConn { get; private set; }

        private Microsoft.Extensions.Configuration.IConfiguration _config;

        public DALRole()
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


        public List<RolesModel> GetRoles()
        {
            List<RolesModel> roles = new List<RolesModel>();
            try
            {
                using (SqlConn)
                {
                    using var cmd = new SqlCommand("SELECT Id, Name FROM Roles", SqlConn);
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        RolesModel role = new RolesModel
                        {
                            RoleID = reader.GetInt32(0),
                            RoleName = reader.GetString(1)
                        };
                        roles.Add(role);
                    }
                    return roles;
                }
            }
            catch
            {
                throw;
            }
        }

        public RolesModel GetRoleByID(int roleID)
        {
            try
            {
                RolesModel role = null;
                using (SqlConn)
                {
                    using var cmd = new SqlCommand("SELECT Id, Name FROM Roles WHERE Id=@roleID", SqlConn);
                    cmd.Parameters.AddWithValue("@roleID", roleID);
                    using var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        role = new RolesModel
                        {
                            RoleID = reader.GetInt32(0),
                            RoleName = reader.GetString(1)
                        };
                    }
                }
                return role;
            }
            catch
            {
                throw;
            }
        }
    }
}

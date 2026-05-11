using QuizMakerModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizMakerDAL;
using QuizMakerModel;

namespace QuizMakerBLL
{
    public class BLLRole
    {
        public List<RolesModel> GetRoles()
        {
            DALRole dalRole = new DALRole();
            return dalRole.GetRoles();
        }

        public RolesModel GetRoleByID(int roleID)
        {
            DALRole dalRole = new DALRole();
            return dalRole.GetRoleByID(roleID);
        }
    }

}

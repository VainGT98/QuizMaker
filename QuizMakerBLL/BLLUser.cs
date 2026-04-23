using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizMakerDAL;
using QuizMakerModel;

namespace QuizMakerBLL
{
    public class BLLUser
    {
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
    }
}

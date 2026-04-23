
using QuizMaker.QuizMakerDAL;
using QuizMaker.QuizMakerModel;
using QuizMakerModel;
using System.Windows;

namespace QuizMakerBLL
{
    public class BLLAuthenticate
    {
        public bool Register(UserModel user)
        {
            try
            {
                PasswordHasher passwordHasher = new PasswordHasher();

                var (hash, salt) = passwordHasher.HashPassword(user.Password);

                DALAuthenticate authServiceDAL = new DALAuthenticate();

                bool reg = authServiceDAL.RegisterDAL(user, hash, salt);

                return reg;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool Login(UserModel user)
        {
            try
            {
                DALAuthenticate authServiceDAL = new DALAuthenticate();

                bool login = authServiceDAL.LoginDAL(user);

                return login;
            }            
            catch (Exception ex)
            {
                throw;
            }
        }
    }

}

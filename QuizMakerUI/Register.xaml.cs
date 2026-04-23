using Microsoft.Win32;
using QuizMakerBLL;
using QuizMakerModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuizMaker
{
    /// <summary>
    /// Logica di interazione per Register.xaml
    /// </summary>
    public partial class Register : Page
    {
        private bool isVisible = false;

        public Register()
        {
            InitializeComponent();
        }

        private void Button_Register(object sender, RoutedEventArgs e)
        {
            try
            {
                UserModel userModel = new UserModel();

                userModel.Username = Username.Text;
                userModel.Password = Password.Password;

                BLLAuthenticate bLLAuthenticate = new BLLAuthenticate();

                bool reg = bLLAuthenticate.Register(userModel);

                if (reg)
                {
                    MessageBox.Show("Registered");

                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();

                    Window currentWindow = Window.GetWindow(this);
                    currentWindow?.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }   
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!isVisible)
            {
                PasswordText.Text = Password.Password;
            }
        }

        private void ButtonShowPassword_Click(object sender, RoutedEventArgs e)
        {
            isVisible = !isVisible;

            if (isVisible)
            {
                // Mostra password
                PasswordText.Text = Password.Password;
                PasswordText.Visibility = Visibility.Visible;
                Password.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Nascondi password
                Password.Password = PasswordText.Text;
                Password.Visibility = Visibility.Visible;
                PasswordText.Visibility = Visibility.Collapsed;
            }
        }
    }
}

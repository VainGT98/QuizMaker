using QuizMaker.QuizMakerModel;
using QuizMakerBLL;
using QuizMakerModel;
using QuizMakerUI;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace QuizMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isVisible = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Login(object sender, RoutedEventArgs e)
        {
            try
            {
                UserModel userModel = new UserModel();

                userModel.Username = Username.Text;
                userModel.Password = Password.Password;

                BLLAuthenticate bLLAuthenticate = new BLLAuthenticate();
                
                bool login = bLLAuthenticate.Login(userModel);

                if(login)
                {
                    //NavigationWindow nav = new NavigationWindow();
                    //nav.Navigate(new QuizList(userModel));
                    //nav.Show();
                    //this.Close();
                    var win = new Window1(userModel);
                    win.Show();
                    this.Close();
                }
                else
                {
                    //MessageBox.Show("Username or password incorrect");
                    new CustomMessageBox("Username or password incorrect", " - Login Error", CustomMessageBox.MessageBoxType.Ok).ShowDialog();
                }
            }            
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Register(object sender, RoutedEventArgs e)
        {
            NavigationWindow nav = new NavigationWindow();
            nav.Navigate(new Register());
            nav.Show();
            this.Close();
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

        private void ButtonDeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UserModel userModel = new UserModel();

                userModel.Username = Username.Text;
                userModel.Password = Password.Password;

                BLLAuthenticate bLLAuthenticate = new BLLAuthenticate();

                bool login = bLLAuthenticate.Login(userModel);

                if (login)
                {
                    BLLUser bLLUser = new BLLUser();
                    int userID = bLLUser.FindUserID(Username.Text);
                    bLLUser.DeleteUser(userID);
                    //MessageBox.Show("Account deleted successfully");
                    var box = new CustomMessageBox("Account deleted successfully", " - Account Deleted", CustomMessageBox.MessageBoxType.Ok);
                    box.Owner = Window.GetWindow(this); // imposta la finestra padre
                    box.WindowStartupLocation = WindowStartupLocation.CenterOwner; // centra rispetto al padre
                    box.ShowDialog();
                }
                else
                {
                    //MessageBox.Show("Username or password incorrect");
                    var box = new CustomMessageBox("Username or password incorrect", " - Login Error", CustomMessageBox.MessageBoxType.Ok);
                    box.Owner = Window.GetWindow(this); // imposta la finestra padre
                    box.WindowStartupLocation = WindowStartupLocation.CenterOwner; // centra rispetto al padre
                    box.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
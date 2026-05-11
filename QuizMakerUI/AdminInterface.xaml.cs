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

namespace QuizMakerUI
{
    /// <summary>
    /// Logica di interazione per AdminInterface.xaml
    /// </summary>
    public partial class AdminInterface : Page
    {
        public AdminInterface()
        {
            InitializeComponent();

            BLLUser bLLUser = new BLLUser();
            UsersGrid.ItemsSource = bLLUser.GetAllUsers();

            BLLRole bLLRole = new BLLRole();
            RolesGrid.ItemsSource = bLLRole.GetRoles();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void UsersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BLLUser bLLUser = new BLLUser();
            BLLRole bLLRole = new BLLRole();

            UserModel user = UsersGrid.SelectedItem as UserModel;
            
            int userID = bLLUser.FindUserID(user.Username);
            int roleID = bLLUser.GetRoleIDByUserID(userID);
            RolesModel role = bLLRole.GetRoleByID(roleID);

            RolesGrid.SelectedItem = role;
        }
    }
}

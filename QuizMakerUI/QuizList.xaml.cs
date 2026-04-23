using QuizMakerBLL;
using QuizMakerDAL;
using QuizMakerModel;
using System;
using System.CodeDom.Compiler;
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
    /// Logica di interazione per QuizList.xaml
    /// </summary>
    public partial class QuizList : Page
    {
        UserModel _user;
        public QuizList(UserModel user)
        {
            InitializeComponent();
            _user = user;
            BLLUser bLLUser = new BLLUser();
            int userID = bLLUser.FindUserID(user.Username);

            BLLQuiz bLLQuiz = new BLLQuiz();
            dataGrid.ItemsSource = bLLQuiz.GetQuizzesByUserID(userID);

            AddNewQuiz.Tag = user;
        }

        private void Button_Update(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var quiz = button.Tag as QuizModel;

            BLLQuiz bLLQuiz = new BLLQuiz();
            QuizModel quiz1 = bLLQuiz.GetQuizByQuizID(quiz.QuizID.Value);

            NavigationService.Navigate(new UpdateQuiz(quiz1, _user));
        }

        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var quiz = button.Tag as QuizModel;

            BLLQuiz bLLQuiz = new BLLQuiz();
            QuizModel quiz1 = bLLQuiz.GetQuizByQuizID(quiz.QuizID.Value);

            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete the quiz '{quiz.Title}'?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                bLLQuiz.DeleteQuiz(quiz1);
                // Refresh the quiz list after deletion
                dataGrid.ItemsSource = bLLQuiz.GetQuizzesByUserID(quiz.UserID);
            }
        }

        private void Button_AddNewQuiz(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            UserModel user = btn.Tag as UserModel;
            NavigationService.Navigate(new CreateQuiz(user));
        }

        private void Button_Details(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var quiz = button.Tag as QuizModel;

            BLLQuiz bLLQuiz = new BLLQuiz();
            QuizModel quiz1 = bLLQuiz.GetQuizByQuizID(quiz.QuizID.Value);
            NavigationService.Navigate(new QuizDetails(quiz1, _user));
        }
    }
}

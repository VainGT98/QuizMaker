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
    /// Logica di interazione per CreateQuiz.xaml
    /// </summary>
    public partial class CreateQuiz : Page
    {
        int UserId { get; set; }
        private UserModel _user;
        public CreateQuiz(UserModel user)
        {
            InitializeComponent();
            _user = user;
            BLLUser bLLUser = new BLLUser();
            UserId = bLLUser.FindUserID(user.Username);
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(QuizTitle.Text) || string.IsNullOrEmpty(QuestionNumber.Text) || AnswerNumber.SelectedItem == null)
            {
                //MessageBox.Show("Please fill in all fields.");
                new CustomMessageBox("Please fill in all fields.", " - Validation Error", CustomMessageBox.MessageBoxType.Ok).ShowDialog();
                return;
            }
            else if(!int.TryParse(QuestionNumber.Text, out int questionNum) || questionNum <= 0)
            {
                //MessageBox.Show("Please enter a valid positive integer for the number of questions.");
                new CustomMessageBox("Please enter a valid positive integer for the number of questions.", " - Validation Error", CustomMessageBox.MessageBoxType.Ok).ShowDialog();
                return;
               
            }
            else if (AnswerNumber.SelectedItem == null)
            {
                //MessageBox.Show("Please select the number of answers.");
                new CustomMessageBox("Please select the number of answers.", " - Validation Error", CustomMessageBox.MessageBoxType.Ok).ShowDialog();
                return;
            }
            else
            {
                QuizModel quiz = new QuizModel
                {
                    Title = QuizTitle.Text,
                    CreationDate = DateTime.Now,
                    UserID = UserId,
                    QuestionNumber = int.Parse(QuestionNumber.Text)
                };
                int questionNumber = 1;
                ComboBoxItem selectedItem = (ComboBoxItem)AnswerNumber.SelectedItem;
                int answerNumber = int.Parse(selectedItem.Content.ToString());
                NavigationService.Navigate(new CreateQuestion(quiz, questionNumber, answerNumber, _user));
            }  
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new QuizList(_user));
        }
    }
}

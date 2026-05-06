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
    /// Logica di interazione per UpdateQuiz.xaml
    /// </summary>
    public partial class UpdateQuiz : Page
    {
        int _questionNumber;
        int _questionCount = 0;
        UserModel _user;
        QuizModel _quiz;

        public UpdateQuiz(QuizModel quiz, UserModel user)
        {
            InitializeComponent();
            _quiz = quiz;
            BLLQuestion bLLQuestion = new BLLQuestion();
            _questionNumber = bLLQuestion.GetQuestionCountByQuizID(quiz.QuizID ?? 0);
            _user = user;
            
            QuizTitle.Text = quiz.Title;

            foreach (QuestionModel question in quiz.QuestionsList)
            {
                Button button = new Button()
                {
                    Content = $"Question {question.OrderNumber}",
                    Margin = new Thickness(5),
                    Tag = question,
                    Style = (Style)Application.Current.Resources["SecondaryButton"]
                };
                
                button.Click += (s, e) =>
                {
                    var btn = s as Button;
                    var q = btn.Tag as QuestionModel;
                    NavigationService.Navigate(new UpdateQuestion(quiz, question, _user));
                };

                QuestionsPanel.Children.Add(button);
            }

            ButtonSave.Tag = quiz;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new QuizList(_user));
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(QuizTitle.Text))
            {
                MessageBox.Show("Please enter a title for the quiz.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                var button = sender as Button;
                var quiz = button.Tag as QuizModel;
                quiz.Title = QuizTitle.Text;
                BLLQuiz bLLQuiz = new BLLQuiz();
                bLLQuiz.UpdateQuiz(quiz);
                MessageBox.Show("Quiz updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new QuizList(_user));
            }
        }

        private void ButtonAddQuestion_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddQuestion(_quiz, _user));
        }
    }
}

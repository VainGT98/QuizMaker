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
    /// Logica di interazione per QuizDetails.xaml
    /// </summary>
    public partial class QuizDetails : Page
    {
        UserModel _user;
        public QuizDetails(QuizModel quiz, UserModel user)
        {
            InitializeComponent();
            _user = user;
            QuizDetailsTextBlock.Text = quiz.Title;

            foreach (QuestionModel question in quiz.QuestionsList)
            {
                TextBlock questionTextBlock = new TextBlock
                {
                    Text = $"Question {question.OrderNumber}: {question.Text}",
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 10, 0, 5),
                    FontSize = 20
                };
                QuizDetailsStackPanel.Children.Add(questionTextBlock);
                if (question.Image != null)
                {
                    Image questionImage = new Image
                    {
                        Source = new BLLImage().ByteArrayToImage(question.Image),
                        Height = 100,
                        Margin = new Thickness(0, 0, 0, 10),
                        HorizontalAlignment = HorizontalAlignment.Left

                    };
                    QuizDetailsStackPanel.Children.Add(questionImage);
                }
                foreach (AnswerModel answer in question.AnswersList)
                {
                    TextBlock answerTextBlock = new TextBlock
                    {
                        Text = $"- {answer.Text}",
                        Margin = new Thickness(20, 2, 0, 2),
                        FontSize = 16
                    };
                    QuizDetailsStackPanel.Children.Add(answerTextBlock);
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new QuizList(_user));
        }
    }
}

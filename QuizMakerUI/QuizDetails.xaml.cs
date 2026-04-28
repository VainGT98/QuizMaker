using QuizMakerBLL;
using QuizMakerModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        QuizModel _quiz;
        public QuizDetails(QuizModel quiz, UserModel user)
        {
            InitializeComponent();
            _user = user;
            _quiz = quiz;
            QuizDetailsTextBlock.Text = _quiz.Title;

            foreach (QuestionModel question in _quiz.QuestionsList)
            {
                StackPanel container = new StackPanel
                {
                    Margin = new Thickness(0, 10, 0, 10)
                };

                TextBlock questionTextBlock = new TextBlock
                {
                    Text = $"Question {question.OrderNumber}: {question.Text}",
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 10, 0, 5),
                    FontSize = 20
                };
                container.Children.Add(questionTextBlock);

                StackPanel buttonsPanel = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    Margin = new Thickness(0, 5, 0, 5),
                    HorizontalAlignment = HorizontalAlignment.Right
                };

                Button upButton = new Button
                {
                    Content = "↑",
                    Tag = question,
                    Margin = new Thickness(2)
                };
                upButton.Click += MoveUp_Click;

                Button downButton = new Button
                {
                    Content = "↓",
                    Tag = question,
                    Margin = new Thickness(2)
                };
                downButton.Click += MoveDown_Click;

                buttonsPanel.Children.Add(upButton);
                buttonsPanel.Children.Add(downButton);

                container.Children.Add(buttonsPanel);

                if (question.Image != null)
                {
                    Image questionImage = new Image
                    {
                        Source = new BLLImage().ByteArrayToImage(question.Image),
                        Height = 100,
                        Margin = new Thickness(0, 0, 0, 10),
                        HorizontalAlignment = HorizontalAlignment.Left

                    };
                    container.Children.Add(questionImage);
                }
                foreach (AnswerModel answer in question.AnswersList)
                {
                    TextBlock answerTextBlock = new TextBlock
                    {
                        Text = $"- {answer.Text}",
                        Margin = new Thickness(20, 2, 0, 2),
                        FontSize = 16
                    };
                    container.Children.Add(answerTextBlock);
                }
                QuizDetailsStackPanel.Children.Add(container);
            }
        }

        private void MoveUp_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            QuestionModel question = (QuestionModel)btn.Tag;

            // Trova la domanda precedente (con OrderNumber minore)
            QuestionModel? previous = _quiz.QuestionsList
                .Where(q => q.OrderNumber < question.OrderNumber)
                .OrderByDescending(q => q.OrderNumber)
                .FirstOrDefault();

            if (previous == null) return; // È già la prima

            // Scambia gli OrderNumber
            int temp = previous.OrderNumber;
            previous.OrderNumber = question.OrderNumber;
            question.OrderNumber = temp;

            BLLQuiz bLLQuiz = new BLLQuiz();
            bLLQuiz.UpdateQuiz(_quiz); // Aggiorna il quiz nel database

            RefreshUI();
        }

        private void MoveDown_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            QuestionModel question = (QuestionModel)btn.Tag;

            // Trova la domanda successiva (con OrderNumber maggiore)
            QuestionModel? next = _quiz.QuestionsList
                .Where(q => q.OrderNumber > question.OrderNumber)
                .OrderBy(q => q.OrderNumber)
                .FirstOrDefault();

            if (next == null) return; // È già l'ultima

            // Scambia gli OrderNumber
            int temp = next.OrderNumber;
            next.OrderNumber = question.OrderNumber;
            question.OrderNumber = temp;

            BLLQuiz bLLQuiz = new BLLQuiz();
            bLLQuiz.UpdateQuiz(_quiz); // Aggiorna il quiz nel database

            RefreshUI();
        }

        private void RefreshUI()
        {
            QuizDetailsStackPanel.Children.Clear();

            foreach (QuestionModel question in _quiz.QuestionsList.OrderBy(q => q.OrderNumber))
            {
                // ... stessa logica del costruttore
                StackPanel container = new StackPanel
                {
                    Margin = new Thickness(0, 10, 0, 10)
                };

                TextBlock questionTextBlock = new TextBlock
                {
                    Text = $"Question {question.OrderNumber}: {question.Text}",
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 10, 0, 5),
                    FontSize = 20
                };
                container.Children.Add(questionTextBlock);

                StackPanel buttonsPanel = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    Margin = new Thickness(0, 5, 0, 5),
                    HorizontalAlignment = HorizontalAlignment.Right
                };

                Button upButton = new Button
                {
                    Content = "↑",
                    Tag = question,
                    Margin = new Thickness(2)
                };
                upButton.Click += MoveUp_Click;

                Button downButton = new Button
                {
                    Content = "↓",
                    Tag = question,
                    Margin = new Thickness(2)
                };
                downButton.Click += MoveDown_Click;

                buttonsPanel.Children.Add(upButton);
                buttonsPanel.Children.Add(downButton);

                container.Children.Add(buttonsPanel);

                if (question.Image != null)
                {
                    Image questionImage = new Image
                    {
                        Source = new BLLImage().ByteArrayToImage(question.Image),
                        Height = 100,
                        Margin = new Thickness(0, 0, 0, 10),
                        HorizontalAlignment = HorizontalAlignment.Left

                    };
                    container.Children.Add(questionImage);
                }
                foreach (AnswerModel answer in question.AnswersList)
                {
                    TextBlock answerTextBlock = new TextBlock
                    {
                        Text = $"- {answer.Text}",
                        Margin = new Thickness(20, 2, 0, 2),
                        FontSize = 16
                    };
                    container.Children.Add(answerTextBlock);
                }
                QuizDetailsStackPanel.Children.Add(container);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new QuizList(_user));
        }
    }
}

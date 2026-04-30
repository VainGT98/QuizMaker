using Microsoft.Win32;
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

            RefreshUI();
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
            //MessageBox.Show("RefreshUI chiamato"); // <-- temporaneo

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
                foreach (AnswerModel answer in question.AnswersList.OrderBy(a => a.OrderNumber))
                {
                    StackPanel answerRow = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Margin = new Thickness(20, 2, 0, 2)
                    };

                    StackPanel answerButtonsPanel = new StackPanel
                    {
                        Orientation = Orientation.Vertical,
                        Margin = new Thickness(0, 0, 5, 0)
                    };
                    Button answerUpButton = new Button
                    {
                        Content = "↑",
                        Tag = (question, answer),  // tupla: serve anche la question per accedere alla lista
                        Margin = new Thickness(2)
                    };
                    answerUpButton.Click += MoveAnswerUp_Click;
                    Button answerDownButton = new Button
                    {
                        Content = "↓",
                        Tag = (question, answer),
                        Margin = new Thickness(2)
                    };
                    answerDownButton.Click += MoveAnswerDown_Click;
                    answerButtonsPanel.Children.Add(answerUpButton);
                    answerButtonsPanel.Children.Add(answerDownButton);

                    TextBlock answerTextBlock = new TextBlock
                    {
                        Text = $"- {answer.Text}",
                        Margin = new Thickness(20, 2, 0, 2),
                        FontSize = 16
                    };

                    answerRow.Children.Add(answerButtonsPanel);
                    answerRow.Children.Add(answerTextBlock);
                    container.Children.Add(answerRow);
                }
                QuizDetailsStackPanel.Children.Add(container);
            }
        }

        private void MoveAnswerUp_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            var (question, answer) = ((QuestionModel, AnswerModel))btn.Tag;
            
            // Trova la risposta precedente
            AnswerModel? previous = question.AnswersList
                .Where(a => a.OrderNumber < answer.OrderNumber)
                .OrderByDescending(a => a.OrderNumber)
                .FirstOrDefault();
            if (previous == null) return; // È già la prima
            // Scambia gli OrderNumber
            int temp = previous.OrderNumber;
            previous.OrderNumber = answer.OrderNumber;
            answer.OrderNumber = temp;
            
            BLLQuiz bLLQuiz = new BLLQuiz();
            bLLQuiz.UpdateQuiz(_quiz); // Aggiorna il quiz nel database
            
            RefreshUI();
        }

        private void MoveAnswerDown_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            var (question, answer) = ((QuestionModel, AnswerModel))btn.Tag;
           
            // Trova la risposta successiva
            AnswerModel? next = question.AnswersList
                .Where(a => a.OrderNumber > answer.OrderNumber)
                .OrderBy(a => a.OrderNumber)
                .FirstOrDefault();
            if (next == null) return; // È già l'ultima
            // Scambia gli OrderNumber
            int temp = next.OrderNumber;
            next.OrderNumber = answer.OrderNumber;
            answer.OrderNumber = temp;
            
            BLLQuiz bLLQuiz = new BLLQuiz();
            bLLQuiz.UpdateQuiz(_quiz); // Aggiorna il quiz nel database
            
            RefreshUI();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new QuizList(_user));
        }

        private void ExportToPDFButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf",
                FileName = _quiz.Title
            };

            if (dialog.ShowDialog() == true)
            {
                BLLExportPDF bLLExportPDF = new BLLExportPDF();
                bLLExportPDF.ExportQuizToPdf(_quiz, dialog.FileName);
                MessageBox.Show("PDF esportato con successo!");
            }
        }
    }
}

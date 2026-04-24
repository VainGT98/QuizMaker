using Microsoft.Win32;
using QuizMakerBLL;
using QuizMakerModel;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Logica di interazione per UpdateQuestion.xaml
    /// </summary>
    public partial class UpdateQuestion : Page
    {
        int _questionNumber;
        int _questionCount;
        QuizModel _quiz;
        UserModel _user;
        int _questionID;
        int _answerID;
        public UpdateQuestion(QuizModel quiz, int questionNumber, int questionCount, UserModel user)
        {
            InitializeComponent();
            _quiz = quiz;

            if (questionNumber == 1)
            {
                ButtonNext.Visibility = Visibility.Collapsed;
            }
            questionNumber--;
            _questionNumber = questionNumber;
            _questionCount = questionCount;
            _user = user;
            _questionID = quiz.QuestionsList[_questionCount].QuestionID.Value;

            QuestionLabel.Text = "Question " + quiz.QuestionsList[_questionCount].OrderNumber;
            QuestionText.Text = quiz.QuestionsList[_questionCount].Text;
            if (!string.IsNullOrWhiteSpace(quiz.QuestionsList[_questionCount].ImagePath))
            {
                TxtPath.Text = quiz.QuestionsList[_questionCount].ImagePath;
                //Image.Source = new BitmapImage(new Uri(quiz.QuestionsList[_questionCount].ImagePath));
            }
            Image.Source = quiz.QuestionsList[_questionCount].Image != null ? new BLLImage().ByteArrayToImage(quiz.QuestionsList[_questionCount].Image) : null;

            foreach (AnswerModel answer in quiz.QuestionsList[_questionCount].AnswersList)
            {
                _answerID = answer.AnswerID.Value;
                TextBlock textBlock = new TextBlock
                {
                    Text = "Answer " + answer.OrderNumber,
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 10, 0, 5)
                };
                Panel.Children.Add(textBlock);
                TextBox textBox = new TextBox
                {
                    Name = "Answer" + answer.OrderNumber,
                    Text = answer.Text,
                    Height = 30,
                    Margin = new Thickness(0, 0, 0, 10)
                };
                Panel.Children.Add(textBox);
                RadioButton radioButton = new RadioButton
                {
                    Content = "Correct",
                    Margin = new Thickness(0, 0, 0, 20),
                    Tag = new { textBox, _answerID }, // 🔥 collega il TextBox e l'ID della risposta
                    IsChecked = answer.IsCorrect
                };
                Panel.Children.Add(radioButton);
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(QuestionText.Text))
            {
                MessageBox.Show("Please enter a question text.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (Panel.Children.OfType<TextBox>().Any(tb => string.IsNullOrWhiteSpace(tb.Text)))
            {
                MessageBox.Show("Please fill in all answer fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (!Panel.Children.OfType<RadioButton>().Any(rb => rb.IsChecked == true))
            {
                MessageBox.Show("Please select the correct answer.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (Panel.Children.OfType<RadioButton>().Count(rb => rb.IsChecked == true) > 1)
            {
                MessageBox.Show("Please select only one correct answer.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                List<AnswerModel> answers = new List<AnswerModel>();

                int order = 1;

                foreach (var child in Panel.Children)
                {
                    if (child is RadioButton rb)
                    {
                        answers.Add(new AnswerModel
                        {
                            AnswerID = ((dynamic)rb.Tag)._answerID, // 🔥 recupera l'ID della risposta
                            Text = ((dynamic)rb.Tag).textBox.Text, // 🔥 recupera il testo dal TextBox
                            IsCorrect = rb.IsChecked == true,
                            OrderNumber = order++
                        });
                    }
                }

                _quiz.QuestionsList.Add(new QuestionModel
                {
                    QuestionID = _questionID,
                    Text = QuestionText.Text,
                    ImagePath = string.IsNullOrWhiteSpace(TxtPath.Text) ? null : TxtPath.Text,
                    OrderNumber = _questionCount + 1,
                    Image = Image.Source != null ? new BLLImage().ImageToByteArray((BitmapSource)Image.Source) : null,
                    AnswersList = answers
                });

                BLLQuiz bLLQuiz = new BLLQuiz();
                bLLQuiz.UpdateQuiz(_quiz);

                MessageBox.Show("Quiz updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                NavigationService.Navigate(new QuizList(_user));
            }   
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(QuestionText.Text))
            {
                MessageBox.Show("Please enter a question text.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (Panel.Children.OfType<TextBox>().Any(tb => string.IsNullOrWhiteSpace(tb.Text)))
            {
                MessageBox.Show("Please fill in all answer fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (!Panel.Children.OfType<RadioButton>().Any(rb => rb.IsChecked == true))
            {
                MessageBox.Show("Please select the correct answer.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (Panel.Children.OfType<RadioButton>().Count(rb => rb.IsChecked == true) > 1)
            {
                MessageBox.Show("Please select only one correct answer.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                List<AnswerModel> answers = new List<AnswerModel>();

                int order = 1;

                foreach (var child in Panel.Children)
                {
                    if (child is RadioButton rb)
                    {
                        answers.Add(new AnswerModel
                        {
                            AnswerID = ((dynamic)rb.Tag)._answerID, // 🔥 recupera l'ID della risposta
                            Text = ((dynamic)rb.Tag).textBox.Text, // 🔥 recupera il testo dal TextBox
                            IsCorrect = rb.IsChecked == true,
                            OrderNumber = order++
                        });
                    }
                }

                _quiz.QuestionsList.Add(new QuestionModel
                {
                    QuestionID = _questionID,
                    Text = QuestionText.Text,
                    ImagePath = string.IsNullOrWhiteSpace(TxtPath.Text) ? null : TxtPath.Text,
                    OrderNumber = _questionCount + 1,
                    Image = Image.Source != null ? new BLLImage().ImageToByteArray((BitmapSource)Image.Source) : null,
                    AnswersList = answers
                });


                _questionCount++;
                NavigationService.Navigate(new UpdateQuestion(_quiz, _questionNumber, _questionCount, _user));
            } 
        }

        private void ButtonChooseImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            string path = Environment.CurrentDirectory;
            dlg.InitialDirectory = System.IO.Path.Combine(path, "Images");
            dlg.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";

            if (dlg.ShowDialog() == true)
            {
                if (!dlg.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase) && !dlg.FileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Please select a valid image file (PNG or JPG).", "Invalid File", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    TxtPath.Text = dlg.FileName;
                    Image.Source = new BitmapImage(new Uri(dlg.FileName));
                }
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new QuizList(_user));
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            //_questionCount--;
            //NavigationService.GoBack();
        }
    }
}

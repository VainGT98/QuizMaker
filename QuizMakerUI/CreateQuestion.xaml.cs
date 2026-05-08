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
using static System.Net.Mime.MediaTypeNames;

namespace QuizMakerUI
{
    /// <summary>
    /// Logica di interazione per CreateQuestion.xaml
    /// </summary>
    public partial class CreateQuestion : Page
    {
        private QuizModel _quiz;
        private int _questionNumber;
        private int _answerNumber;
        private UserModel _user;
        public CreateQuestion(QuizModel quiz, int questionNumber ,int answerNumber, UserModel user)
        {
            InitializeComponent();
            _quiz = quiz;
            _questionNumber = questionNumber;
            _answerNumber = answerNumber;
            _user = user;
            if (quiz.QuestionNumber == 1)
            {
                ButtonNext.Visibility = Visibility.Collapsed;
            }
            quiz.QuestionNumber--;

            QuestionLabel.Text = "Question " + _questionNumber;

            for (int i = 1; i <= answerNumber ; i++)
            {
                TextBlock textBlock = new TextBlock
                {
                    Text = "Answer " + i,
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 10, 0, 5)
                };
                Panel.Children.Add(textBlock);
                TextBox textBox = new TextBox
                {
                    Name = "Answer" + i,
                    Height = 30,
                    Margin = new Thickness(0, 0, 0, 10)
                };
                Panel.Children.Add(textBox);
                RadioButton radioButton = new RadioButton
                {
                    Content = "Correct",
                    GroupName = "AnswersGroup", // 🔥 fondamentale
                    Tag = textBox,
                    Margin = new Thickness(0, 0, 0, 15)
                };
                Panel.Children.Add(radioButton);
            }
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(QuestionText.Text))
            {
                //MessageBox.Show("Please enter a question text.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                new CustomMessageBox("Please enter a question text.", " - Validation Error", CustomMessageBox.MessageBoxType.Ok).ShowDialog();
                return;
            }
            else if (Panel.Children.OfType<TextBox>().Any(tb => string.IsNullOrWhiteSpace(tb.Text)))
            {
                //MessageBox.Show("Please fill in all answer fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                new CustomMessageBox("Please fill in all answer fields.", " - Validation Error", CustomMessageBox.MessageBoxType.Ok).ShowDialog();
                return;
            }
            else if (!Panel.Children.OfType<RadioButton>().Any(rb => rb.IsChecked == true))
            {
                //MessageBox.Show("Please select the correct answer.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                new CustomMessageBox("Please select the correct answer.", " - Validation Error", CustomMessageBox.MessageBoxType.Ok).ShowDialog();
                return;
            }
            else if (Panel.Children.OfType<RadioButton>().Count(rb => rb.IsChecked == true) > 1)
            {
                //MessageBox.Show("Please select only one correct answer.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                new CustomMessageBox("Please select only one correct answer.", " - Validation Error", CustomMessageBox.MessageBoxType.Ok).ShowDialog();
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
                        TextBox txt = rb.Tag as TextBox;

                        answers.Add(new AnswerModel
                        {
                            Text = txt.Text,
                            IsCorrect = rb.IsChecked == true,
                            OrderNumber = order++
                        });
                    }
                }

                _quiz.QuestionsList.Add(new QuestionModel
                {
                    Text = QuestionText.Text,
                    ImagePath = string.IsNullOrWhiteSpace(TxtPath.Text) ? null : TxtPath.Text,
                    OrderNumber = _questionNumber,
                    Image = Image.Source != null ? new BLLImage().ImageToByteArray((BitmapImage)Image.Source) : null,
                    AnswersList = answers

                });
                _questionNumber++;
                NavigationService.Navigate(new CreateQuestion(_quiz, _questionNumber, _answerNumber, _user));
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(QuestionText.Text))
            {
                //MessageBox.Show("Please enter a question text.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                new CustomMessageBox("Please enter a question text.", " - Validation Error", CustomMessageBox.MessageBoxType.Ok).ShowDialog();
                return;
            }
            else if (Panel.Children.OfType<TextBox>().Any(tb => string.IsNullOrWhiteSpace(tb.Text)))
            {
                //MessageBox.Show("Please fill in all answer fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                new CustomMessageBox("Please fill in all answer fields.", " - Validation Error", CustomMessageBox.MessageBoxType.Ok).ShowDialog();
                return;
            }
            else if (!Panel.Children.OfType<RadioButton>().Any(rb => rb.IsChecked == true))
            {
                //MessageBox.Show("Please select the correct answer.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                new CustomMessageBox("Please select the correct answer.", " - Validation Error", CustomMessageBox.MessageBoxType.Ok).ShowDialog();
                return;
            }
            else if (Panel.Children.OfType<RadioButton>().Count(rb => rb.IsChecked == true) > 1)
            {
                //MessageBox.Show("Please select only one correct answer.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                new CustomMessageBox("Please select only one correct answer.", " - Validation Error", CustomMessageBox.MessageBoxType.Ok).ShowDialog();
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
                        TextBox txt = rb.Tag as TextBox;

                        answers.Add(new AnswerModel
                        {
                            Text = txt.Text,
                            IsCorrect = rb.IsChecked == true,
                            OrderNumber = order++
                        });
                    }
                }

                _quiz.QuestionsList.Add(new QuestionModel
                {
                    Text = QuestionText.Text,
                    ImagePath = string.IsNullOrWhiteSpace(TxtPath.Text) ? null : TxtPath.Text,
                    OrderNumber = _questionNumber,
                    Image = Image.Source != null ? new BLLImage().ImageToByteArray((BitmapImage)Image.Source) : null,
                    AnswersList = answers
                });

                BLLQuiz bLLQuiz = new BLLQuiz();
                bLLQuiz.InsertQuiz(_quiz);

                NavigationService.Navigate(new QuizList(_user));
            } 
        }

        private void ButtonChooseImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            string path = Environment.CurrentDirectory;
            dlg.InitialDirectory = System.IO.Path.Combine(path, "Images");
            dlg.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg";

            if (dlg.ShowDialog() == true)
            {
                if (!dlg.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase) && !dlg.FileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
                {
                    //MessageBox.Show("Please select a valid image file (PNG or JPG).", "Invalid File", MessageBoxButton.OK, MessageBoxImage.Error);
                    new CustomMessageBox("Please select a valid image file (PNG or JPG).", " - Invalid File", CustomMessageBox.MessageBoxType.Ok).ShowDialog();
                    return;
                }
                else
                {
                    // Salva il path nella TextBox
                    TxtPath.Text = dlg.FileName;

                    // Mostra l'immagine
                    Image.Source = new BitmapImage(new Uri(dlg.FileName));
                }
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new QuizList(_user));
        }
    }
}

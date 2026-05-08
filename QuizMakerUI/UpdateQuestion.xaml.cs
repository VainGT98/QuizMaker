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
        QuizModel _quiz;
        UserModel _user;
        int _questionID;
        int _answerID;
        QuestionModel _question;
        public UpdateQuestion(QuizModel quiz, QuestionModel question, UserModel user)
        {
            InitializeComponent();
            _quiz = quiz;
            _question = question;
            _user = user;
            _questionID = question.QuestionID.Value;

            QuestionLabel.Text = "Question " + question.OrderNumber;
            QuestionText.Text = question.Text;
            if (!string.IsNullOrWhiteSpace(question.ImagePath))
            {
                TxtPath.Text = question.ImagePath;
            }
            Image.Source = question.Image != null ? new BLLImage().ByteArrayToImage(question.Image) : null;
            foreach (AnswerModel answer in question.AnswersList)
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
                //MessageBox.Show("Please enter a question text.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                new CustomMessageBox("Please enter a question text.", "Validation Error", CustomMessageBox.MessageBoxType.Ok).ShowDialog();
                return;
            }
            else if (Panel.Children.OfType<TextBox>().Any(tb => string.IsNullOrWhiteSpace(tb.Text)))
            {
                //MessageBox.Show("Please fill in all answer fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                new CustomMessageBox("Please fill in all answer fields.", "Validation Error", CustomMessageBox.MessageBoxType.Ok).ShowDialog();
                return;
            }
            else if (!Panel.Children.OfType<RadioButton>().Any(rb => rb.IsChecked == true))
            {
                //MessageBox.Show("Please select the correct answer.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                new CustomMessageBox("Please select the correct answer.", "Validation Error", CustomMessageBox.MessageBoxType.Ok).ShowDialog();
                return;
            }
            else if (Panel.Children.OfType<RadioButton>().Count(rb => rb.IsChecked == true) > 1)
            {
                //MessageBox.Show("Please select only one correct answer.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                new CustomMessageBox("Please select only one correct answer.", "Validation Error", CustomMessageBox.MessageBoxType.Ok).ShowDialog();
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
                    OrderNumber = _question.OrderNumber,
                    Image = Image.Source != null ? new BLLImage().ImageToByteArray((BitmapSource)Image.Source) : null,
                    AnswersList = answers
                });

                BLLQuiz bLLQuiz = new BLLQuiz();
                bLLQuiz.UpdateQuiz(_quiz);

                //MessageBox.Show("Question updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                new CustomMessageBox("Question updated successfully!", "Success", CustomMessageBox.MessageBoxType.Ok).ShowDialog();

                NavigationService.GoBack();
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
                    //MessageBox.Show("Please select a valid image file (PNG or JPG).", "Invalid File", MessageBoxButton.OK, MessageBoxImage.Error);
                    new CustomMessageBox("Please select a valid image file (PNG or JPG).", "Invalid File", CustomMessageBox.MessageBoxType.Ok).ShowDialog();
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
            NavigationService.GoBack();
        }
    }
}

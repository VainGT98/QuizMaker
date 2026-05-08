using Microsoft.Win32;
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
    /// Logica di interazione per AddQuestion.xaml
    /// </summary>
    public partial class AddQuestion : Page
    {
        private QuizModel _quiz;
        private UserModel _user;

        public AddQuestion(QuizModel quiz, UserModel user)
        {
            InitializeComponent();
            _quiz = quiz;
            _user = user;
            int answerCount = quiz.QuestionsList.Count > 0 ? quiz.QuestionsList.Max(q => q.AnswersList.Count) : 0;

            for (int i = 1; i <= answerCount; i++)
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
                    Margin = new Thickness(0, 0, 0, 10)
                };
                Panel.Children.Add(textBox);
                RadioButton radioButton = new RadioButton
                {
                    Content = "Correct",
                    Margin = new Thickness(0, 0, 0, 20),
                    Tag = textBox,
                };
                Panel.Children.Add(radioButton);
            }
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(QuestionText.Text))
            {
                //MessageBox.Show("Please enter a question text.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                var box = new CustomMessageBox("Please enter a question text.", " - Validation Error", CustomMessageBox.MessageBoxType.Ok);
                box.Owner = Window.GetWindow(this); // imposta la finestra padre
                box.WindowStartupLocation = WindowStartupLocation.CenterOwner; // centra rispetto al padre
                box.ShowDialog();
                return;
            }
            else if (Panel.Children.OfType<TextBox>().Any(tb => string.IsNullOrWhiteSpace(tb.Text)))
            {
                //MessageBox.Show("Please fill in all answer fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                var box = new CustomMessageBox("Please fill in all answer fields.", " - Validation Error", CustomMessageBox.MessageBoxType.Ok);
                box.Owner = Window.GetWindow(this); // imposta la finestra padre
                box.WindowStartupLocation = WindowStartupLocation.CenterOwner; // centra rispetto al padre
                box.ShowDialog();
                return;
            }
            else if (!Panel.Children.OfType<RadioButton>().Any(rb => rb.IsChecked == true))
            {
                //MessageBox.Show("Please select the correct answer.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                var box = new CustomMessageBox("Please select the correct answer.", " - Validation Error", CustomMessageBox.MessageBoxType.Ok);
                box.Owner = Window.GetWindow(this); // imposta la finestra padre
                box.WindowStartupLocation = WindowStartupLocation.CenterOwner; // centra rispetto al padre
                box.ShowDialog();
                return;
            }
            else if (Panel.Children.OfType<RadioButton>().Count(rb => rb.IsChecked == true) > 1)
            {
                //MessageBox.Show("Please select only one correct answer.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                var box = new CustomMessageBox("Please select only one correct answer.", " - Validation Error", CustomMessageBox.MessageBoxType.Ok);
                box.Owner = Window.GetWindow(this); // imposta la finestra padre
                box.WindowStartupLocation = WindowStartupLocation.CenterOwner; // centra rispetto al padre
                box.ShowDialog();
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
                            Text = ((TextBox)rb.Tag).Text,
                            IsCorrect = rb.IsChecked == true,
                            OrderNumber = order++
                        });
                    }
                }

                _quiz.QuestionsList.Add(new QuestionModel
                {
                    Text = QuestionText.Text,
                    ImagePath = string.IsNullOrWhiteSpace(TxtPath.Text) ? null : TxtPath.Text,
                    OrderNumber = _quiz.QuestionsList.Count + 1,
                    Image = Image.Source != null ? new BLLImage().ImageToByteArray((BitmapSource)Image.Source) : null,
                    AnswersList = answers
                });

                BLLQuiz bLLQuiz = new BLLQuiz();
                bLLQuiz.UpdateQuiz(_quiz);

                var box = new CustomMessageBox("Question added successfully!", " - Success", CustomMessageBox.MessageBoxType.Ok);
                box.Owner = Window.GetWindow(this); // imposta la finestra padre
                box.WindowStartupLocation = WindowStartupLocation.CenterOwner; // centra rispetto al padre
                box.ShowDialog();
                NavigationService.Navigate(new UpdateQuiz(_quiz, _user));
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
                    var box = new CustomMessageBox("Please select a valid image file (PNG or JPG).", " - Invalid File", CustomMessageBox.MessageBoxType.Ok);
                    box.Owner = Window.GetWindow(this); // imposta la finestra padre
                    box.WindowStartupLocation = WindowStartupLocation.CenterOwner; // centra rispetto al padre
                    box.ShowDialog();
                    return;
                }
                else
                {
                    TxtPath.Text = dlg.FileName;
                    Image.Source = new BitmapImage(new Uri(dlg.FileName));
                }
            }
        }
    }
}

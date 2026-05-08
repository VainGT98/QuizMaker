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
using System.Windows.Shapes;

namespace QuizMakerUI
{
    /// <summary>
    /// Logica di interazione per CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        public enum MessageBoxType { Ok, OkCancel, YesNo }
        public bool Result { get; private set; } = false;

        public CustomMessageBox(string message, string title = "Info", MessageBoxType type = MessageBoxType.Ok)
        {
            InitializeComponent();
            TitleText.Text = title;
            MessageText.Text = message;

            switch (type)
            {
                case MessageBoxType.Ok:
                    AddButton("OK", true, "PrimaryButton");
                    break;
                case MessageBoxType.OkCancel:
                    AddButton("OK", true, "PrimaryButton");
                    AddButton("Cancel", false, "SecondaryButton");
                    break;
                case MessageBoxType.YesNo:
                    AddButton("Yes", true, "PrimaryButton");
                    AddButton("No", false, "DangerButton");
                    break;
            }
        }

        private void AddButton(string content, bool result, string styleKey)
        {
            Button btn = new Button
            {
                Content = content,
                Style = (Style)Application.Current.Resources[styleKey],
                Margin = new Thickness(8, 0, 0, 0),
                MinWidth = 80
            };
            btn.Click += (s, e) => { Result = result; Close(); };
            ButtonsPanel.Children.Add(btn);
        }
    }
}

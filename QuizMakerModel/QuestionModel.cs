using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace QuizMakerModel
{
    public class QuestionModel
    {
        public int? QuestionID { get; set; }
        public int OrderNumber { get; set; }
        public string Text { get; set; }
        public string? ImagePath { get; set; }
        public Byte[]? Image { get; set; }
        public int QuizID { get; set; }
        public List<AnswerModel> AnswersList { get; set; } = new List<AnswerModel>();
        public int? AnswerNumber { get; set; }
    }
}

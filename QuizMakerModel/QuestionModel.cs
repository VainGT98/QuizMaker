using System.IO;
using System.Text.Json.Serialization;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace QuizMakerModel
{
    public class QuestionModel
    {
        [JsonIgnore]
        public int? QuestionID { get; set; }
        public int OrderNumber { get; set; }
        public string Text { get; set; }
        [JsonIgnore]
        public string? ImagePath { get; set; }
        [JsonIgnore]
        public Byte[]? Image { get; set; }
        [JsonIgnore]
        public int QuizID { get; set; }
        public List<AnswerModel> AnswersList { get; set; } = new List<AnswerModel>();
        [JsonIgnore]
        public int? AnswerNumber { get; set; }
    }
}

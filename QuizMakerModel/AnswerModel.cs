using System.Text.Json.Serialization;

namespace QuizMakerModel
{
    public class AnswerModel
    {
        [JsonIgnore]
        public int? AnswerID { get; set; }
        public int OrderNumber { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
        [JsonIgnore]
        public int QuestionID { get; set; }
    }
}

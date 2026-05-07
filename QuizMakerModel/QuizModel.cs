using System.Text.Json.Serialization;

namespace QuizMakerModel
{
    public class QuizModel
    {
        [JsonIgnore]
        public int? QuizID { get; set; }
        public string Title { get; set; }
        [JsonIgnore]
        public DateTime CreationDate { get; set; }
        [JsonIgnore]
        public DateTime? LastModifiedDate { get; set; }
        [JsonIgnore]
        public int UserID { get; set; }
        public List<QuestionModel> QuestionsList { get; set; } = new List<QuestionModel>();
        [JsonIgnore]
        public int? QuestionNumber { get; set; }
    }
}

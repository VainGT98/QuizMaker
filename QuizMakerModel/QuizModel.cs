namespace QuizMakerModel
{
    public class QuizModel
    {
        public int? QuizID { get; set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int UserID { get; set; }
        public List<QuestionModel> QuestionsList { get; set; } = new List<QuestionModel>();
        public int? QuestionNumber { get; set; }
    }
}

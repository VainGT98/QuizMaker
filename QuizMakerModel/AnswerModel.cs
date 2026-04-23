namespace QuizMakerModel
{
    public class AnswerModel
    {
        public int? AnswerID { get; set; }
        public int OrderNumber { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
        public int QuestionID { get; set; }
    }
}

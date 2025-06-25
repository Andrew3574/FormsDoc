namespace FormsAPP.Models.FormAnswers
{
    public class IntegerAnswer
    {
        public int? Id { get; set; }
        public int? AnswerId { get; set; }
        public int FormQuestionId { get; set; }
        public int? Answer { get; set; }
    }
}

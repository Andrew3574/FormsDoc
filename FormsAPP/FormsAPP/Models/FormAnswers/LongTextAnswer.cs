namespace FormsAPP.Models.FormAnswers
{
    public class LongTextAnswer 
    {
        public int? Id { get; set; }
        public int? AnswerId { get; set; }
        public int FormQuestionId { get; set; }
        public string Answer { get; set; } = null!;
    }
}

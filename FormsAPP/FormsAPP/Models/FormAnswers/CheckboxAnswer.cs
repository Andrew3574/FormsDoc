namespace FormsAPP.Models.FormAnswers
{
    public class CheckboxAnswer
    {
        public int? Id { get; set; }
        public int? AnswerId { get; set; }
        public int FormQuestionId { get; set; }
        public bool Answer { get; set; }
    }
}

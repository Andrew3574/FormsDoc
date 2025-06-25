namespace FormsAPI.Models.FormAnswers
{
    public class LongTextAnswerDTO 
    {
        public int? Id { get; set; }
        public int? AnswerId { get; set; }
        public int FormQuestionId { get; set; }
        public string? Answer { get; set; }
    }
}

namespace FormsAPP.Models.Forms
{
    public class FormQuestionModel
    {
        public int? Id { get; set; }

        public int QuestionTypeId { get; set; }

        public string Question { get; set; } = null!;

        public string? Description { get; set; }

        public bool? DisplayState { get; set; }

        public int Position { get; set; }

        public List<FormQuestionOptionModel> Options { get; set; } = new List<FormQuestionOptionModel>();
    }
}

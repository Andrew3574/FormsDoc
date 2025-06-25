namespace FormsAPI.ModelsDTO.Forms
{
    public class FormQuestionDTO
    {
        public int? QuestionTypeId { get; set; }

        public string QuestionText { get; set; } = null!;

        public string? Description { get; set; }

        public bool? DisplayState { get; set; }

        public int Position { get; set; }

        public List<FormQuestionOptionDTO> Options { get; set; } = new List<FormQuestionOptionDTO>();

    }
}

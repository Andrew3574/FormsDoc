using FormsAPI.Models.FormAnswers;
using FormsAPI.ModelsDTO.Forms;

namespace FormsAPI.ModelsDTO.FormAnswers.CRUD
{
    public class AnsweredFormTemplateDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int FormId { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public int Version { get; set; }

        public List<FormQuestionDTO> Questions { get; set; } = new List<FormQuestionDTO>();

        public List<ShortTextAnswerDTO> ShortTextAnswers { get; set; } = new List<ShortTextAnswerDTO>();
        public List<LongTextAnswerDTO> LongTextAnswers { get; set; } = new List<LongTextAnswerDTO>();
        public List<IntegerAnswerDTO> IntegerAnswers { get; set; } = new List<IntegerAnswerDTO>();
        public List<CheckboxAnswerDTO> CheckboxAnswers { get; set; } = new List<CheckboxAnswerDTO>();
    }
}

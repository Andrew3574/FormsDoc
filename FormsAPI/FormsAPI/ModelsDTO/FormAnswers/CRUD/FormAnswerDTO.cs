using FormsAPI.Models.FormAnswers;
using FormsAPI.ModelsDTO.Forms;

namespace FormsAPI.ModelsDTO.FormAnswers.CRUD
{
    public class FormAnswerDTO
    {
        public FilterUserDTO User { get; set; } = null!;
        public List<ShortTextAnswerDTO>? ShortTextAnswers { get; set; } = new List<ShortTextAnswerDTO>();
        public List<LongTextAnswerDTO>? LongTextAnswers { get; set; } = new List<LongTextAnswerDTO>();
        public List<IntegerAnswerDTO>? IntegerAnswers { get; set; } = new List<IntegerAnswerDTO>();
        public List<CheckboxAnswerDTO>? CheckboxAnswers { get; set; } = new List<CheckboxAnswerDTO>();
    }
}

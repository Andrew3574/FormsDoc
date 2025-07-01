using FormsAPI.Models.FormAnswers;

namespace FormsAPI.ModelsDTO.FormAnswers.CRUD
{
    public class CreateFormAnswerDTO
    {
        public int? Id { get; set; }

        public int UserId { get; set; }

        public int FormId { get; set; }

        public int Version { get; set; }

        public List<ShortTextAnswerDTO> ShortTextAnswers { get; set; } = new List<ShortTextAnswerDTO>();
        public List<LongTextAnswerDTO> LongTextAnswers { get; set; } = new List<LongTextAnswerDTO>();
        public List<IntegerAnswerDTO> IntegerAnswers { get; set; } = new List<IntegerAnswerDTO>();
        public List<CheckboxAnswerDTO> CheckboxAnswers { get; set; } = new List<CheckboxAnswerDTO>();

    }
}

using FormsAPI.ModelsDTO.FormAnswers.CRUD;
using FormsAPI.ModelsDTO.Forms;

namespace FormsAPI.ModelsDTO.FormAnswers
{
    public class FormStatisticsDTO
    {
        public List<FormQuestionDTO> QuestionList { get; set; } = new List<FormQuestionDTO>();
        public List<FormAnswerDTO>? Answers { get; set; } = new List<FormAnswerDTO>();
    }
}

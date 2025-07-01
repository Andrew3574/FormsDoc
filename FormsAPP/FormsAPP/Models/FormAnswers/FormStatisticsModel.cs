using FormsAPP.Models.FormAnswers.CRUD;
using FormsAPP.Models.Forms;

namespace FormsAPP.Models.FormAnswers
{
    public class FormStatisticsModel
    {
        public List<FormQuestionModel> QuestionList { get; set; } = new List<FormQuestionModel>();
        public List<FormAnswerModel>? Answers { get; set; } = new List<FormAnswerModel>();
    }
}

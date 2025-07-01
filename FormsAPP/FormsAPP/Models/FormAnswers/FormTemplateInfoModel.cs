using FormsAPP.Models.Forms;
using FormsAPP.Models.Users;

namespace FormsAPP.Models.FormAnswers
{
    public class FormTemplateInfoModel
    {
        public int? Id { get; set; }

        public int UserId { get; set; }

        public int FormId { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public int Version { get; set; }

        public List<AccessformUserModel>? AccessUsers { get; set; } = new List<AccessformUserModel>();
        
        public List<FormQuestionModel>? Questions { get; set; } = new List<FormQuestionModel>();

        public List<ShortTextAnswer> ShortTextAnswers { get; set; } = new List<ShortTextAnswer>();

        public List<LongTextAnswer> LongTextAnswers { get; set; } = new List<LongTextAnswer>();

        public List<IntegerAnswer> IntegerAnswers { get; set; } = new List<IntegerAnswer>();

        public List<CheckboxAnswer> CheckboxAnswers { get; set; } = new List<CheckboxAnswer>();

    }
}

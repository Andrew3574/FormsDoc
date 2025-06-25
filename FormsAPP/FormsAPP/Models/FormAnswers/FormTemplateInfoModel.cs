using FormsAPP.Models.Forms;
using FormsAPP.Models.Users;

namespace FormsAPP.Models.FormAnswers
{
    public class FormTemplateInfoModel
    {
        public int FormId { get; set; }

        public int UserId { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public List<AccessformUserModel> AccessUsers { get; set; } = new List<AccessformUserModel>();

        public List<FormQuestionModel> Questions { get; set; } = new List<FormQuestionModel>();
    }
}

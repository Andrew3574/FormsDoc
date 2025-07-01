using FormsAPP.Models.Forms.CRUD;
using FormsAPP.Models.Users;

namespace FormsAPP.Models.FormAnswers
{
    public class AnsweredFormModel
    {
        public int AnswerId { get; set; }

        public UserModel? User { get; set; }

        public FormModel? Form { get; set; }

        public string? AsnweredAt { get; set; }
    }
}

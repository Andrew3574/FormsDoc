using FormsAPI.Models.FormAnswers;
using FormsAPI.ModelsDTO.Forms;
using FormsAPI.ModelsDTO.Forms.CRUD_DTO;
using FormsAPI.ModelsDTO.Users;
using Models;

namespace FormsAPI.ModelsDTO.FormAnswers
{
    public class AnsweredFormDTO
    {
        public int AnswerId { get; set; }

        public UserDTO? User { get; set; }

        public FormDTO? Form { get; set; }

        public string AsnweredAt { get; set; } = null!;

    }
}

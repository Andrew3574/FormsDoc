using FormsAPI.ModelsDTO.Forms;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormsAPI.ModelsDTO.FormAnswers
{
    public class FormTemplateDTO
    {
        public int FormId { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public int Version { get; set; }

        public List<FormQuestionDTO> Questions { get; set; } = new List<FormQuestionDTO>();
    }
}

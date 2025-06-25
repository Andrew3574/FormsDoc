using Models.Enums;
using Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormsAPI.ModelsDTO.Forms
{
    public class CreateFormDTO
    {
        public int? Id { get; set; }

        public int UserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public int TopicId { get; set; }

        public int Accessibility { get; set; }

        public List<string> Tags { get; set; } = new List<string>();

        public List<AccessformUserDTO> AccessUsers { get; set; } = new List<AccessformUserDTO>();

        public List<FormQuestionDTO> Questions { get; set; } = new List<FormQuestionDTO>();

    }
}

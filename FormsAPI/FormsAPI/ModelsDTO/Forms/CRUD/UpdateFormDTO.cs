using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FormsAPI.ModelsDTO.Forms.CRUD_DTO
{
    public class UpdateFormDTO
    {
        public int? Id { get; set; }

        public int UserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        [JsonIgnore]
        public IFormFile? ImageFile { get; set; }

        public int TopicId { get; set; }

        public int Accessibility { get; set; }

        public int Version { get; set; }

        public List<FilterTagDTO> FormTags { get; set; } = new List<FilterTagDTO>();

        public List<FilterUserDTO> AccessFormUsers { get; set; } = new List<FilterUserDTO>();

        public List<FormQuestionDTO> Questions { get; set; } = new List<FormQuestionDTO>();

        public List<string> NewTags { get; set; } = new List<string>();

    }
}

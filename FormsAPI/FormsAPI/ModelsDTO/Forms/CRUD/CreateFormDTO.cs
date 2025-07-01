using Models.Enums;
using Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FormsAPI.ModelsDTO.Forms.CRUD_DTO
{
    public class CreateFormDTO
    {
        public int? Id { get; set; }

        public int UserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }
        public IFormFile? ImageFile { get; set; }

        public int TopicId { get; set; }

        public int Accessibility { get; set; }

        public int Version { get; set; } = 1;

        public List<string> Tags { get; set; } = new List<string>();

        public List<AccessformUserDTO> AccessUsers { get; set; } = new List<AccessformUserDTO>();

        public List<FormQuestionDTO> Questions { get; set; } = new List<FormQuestionDTO>();

    }
}

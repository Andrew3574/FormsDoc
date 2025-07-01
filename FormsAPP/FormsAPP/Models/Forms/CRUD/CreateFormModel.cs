using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FormsAPP.Models.Forms.CRUD
{
    public class CreateFormModel
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

        public int Version { get; set; } = 1;

        public List<string> Tags { get; set; } = new List<string>();

        public List<AccessformUserModel> AccessUsers { get; set; } = new List<AccessformUserModel>();

        public List<FormQuestionModel> Questions { get; set; } = new List<FormQuestionModel>();

    }
}

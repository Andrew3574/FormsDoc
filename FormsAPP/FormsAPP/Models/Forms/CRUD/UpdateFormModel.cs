using FormsAPP.Models.Users;
using System.Text.Json.Serialization;

namespace FormsAPP.Models.Forms.CRUD
{
    public class UpdateFormModel
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

        public List<FilterTagModel> FormTags { get; set; } = new List<FilterTagModel>();

        public List<FilterUserModel> AccessFormUsers { get; set; } = new List<FilterUserModel>();

        public List<FormQuestionModel> Questions { get; set; } = new List<FormQuestionModel>();

        public List<string> NewTags { get; set; } = new List<string>();


    }
}

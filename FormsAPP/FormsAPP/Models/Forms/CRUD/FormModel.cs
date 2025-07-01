using FormsAPP.Models.Users;

namespace FormsAPP.Models.Forms.CRUD
{
    public class FormModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public UserModel? User {  get; set; }

        public string CreatedAt { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public string Topic { get; set; } = null!;

        public string Accessibility { get; set; } = null!;

        public List<string> Tags { get; set; } = new List<string>();

        public int LikesCount { get; set; } = 0;

        public List<CommentModel> Comments { get; set; } = new List<CommentModel>();
    }
}

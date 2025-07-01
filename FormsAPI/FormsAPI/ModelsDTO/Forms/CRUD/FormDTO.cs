using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormsAPI.ModelsDTO.Forms.CRUD_DTO
{
    public class FormDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public UserFormDTO User { get; set; } = null!;

        public string CreatedAt { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public string Topic { get; set; } = null!;

        public string Accessibility { get; set; } = null!;

        public List<string> Tags { get; set; } = new List<string>();

        public int LikesCount { get; set; }

        public List<CommentDTO> Comments { get; set; } = new List<CommentDTO>();
    }
}

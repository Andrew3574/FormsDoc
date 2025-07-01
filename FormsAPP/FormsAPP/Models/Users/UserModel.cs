using System.Text.Json.Serialization;

namespace FormsAPP.Models.Users
{
    public class UserModel
    {
        public int Id { get; set; }

        public string Email { get; set; } = null!;

        public string Role { get; set; } = null!;

        public string State { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public string Lastlogin { get; set; } = null!;

        public IFormFile? ImageFile { get; set; }

        public string? ImageUrl { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace FormsAPP.Models.Account
{
    public class RegisterModel
    {
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Surname { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}

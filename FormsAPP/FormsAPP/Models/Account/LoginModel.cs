using System.ComponentModel.DataAnnotations;

namespace FormsAPP.Models.Account
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        [DataType("Password")]
        public string Password { get; set; } = null!;
        public bool RememberMe { get; set; }
    }
}

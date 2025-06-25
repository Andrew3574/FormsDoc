namespace FormsAPI.ModelsDTO.Account
{
    public class AuthDTO
    {
        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}

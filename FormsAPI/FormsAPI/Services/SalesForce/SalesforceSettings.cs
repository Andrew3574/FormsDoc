namespace FormsAPI.Services.SalesForce
{
    public class SalesforceSettings
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string ClientId { get; set; } = null!;
        public string ClientSecret { get; set; } = null!;
        public string LoginEndpoint { get; set; } = null!;
        public string ApiEndpoint { get; set; } = null!; 
        public string AccessToken {  get; set; } = null!;
        public string InstanceUrl { get; set; } = null!;
    }
}

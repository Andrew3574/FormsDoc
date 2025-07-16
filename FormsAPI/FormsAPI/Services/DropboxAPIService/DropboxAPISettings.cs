namespace FormsAPI.Services.DropboxAPIService
{
    public class DropboxAPISettings
    {
        public string ClientId { get; set; } = null!;
        public string ClientSecret { get; set; } = null!;
        public string LoginEndpoint { get; set; } = null!;
        public string CodeAuthEndpoint { get; set; } = null!;
        public string Token { get; set; }
    }
}

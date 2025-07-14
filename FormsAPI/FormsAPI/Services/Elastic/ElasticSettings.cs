namespace FormsAPI.Services.Elastic
{
    public class ElasticSettings
    {
        public string Url { get; set; } = null!;

        public string DefaultIndex { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}

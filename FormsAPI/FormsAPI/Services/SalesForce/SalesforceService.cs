
using FormsAPI.ModelsDTO.Account.Salesforce;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Principal;

namespace FormsAPI.Services.SalesForce
{
    public class SalesForceService : ISalesforceService
    {
        private readonly SalesforceSettings _settings;
        private readonly ILogger<SalesForceService> _logger;

        public SalesForceService(IOptions<SalesforceSettings> settings, ILogger<SalesForceService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }   

        public async Task<bool> CreateContact(SalesforceContact contact)
        {
            await IsValidToken();
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.AccessToken);
            var response = await client.PostAsJsonAsync($"{_settings.InstanceUrl}{_settings.ApiEndpoint}/sobjects/Contact/", contact);
            if (response.IsSuccessStatusCode) return true;
            return false;
        }

        public async Task<SFAuthResponse?> Auth()
        {
            using var htppclient = new HttpClient();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type","client_credentials"),
                new KeyValuePair<string, string>("client_id", _settings.ClientId),
                new KeyValuePair<string, string>("client_secret", _settings.ClientSecret),
            });
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_settings.LoginEndpoint),
                Content = content
            };
            var response = await htppclient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            var authResponse = System.Text.Json.JsonSerializer.Deserialize<SFAuthResponse>(responseContent);
            return response.IsSuccessStatusCode ? authResponse! : null;
        }


        private async Task IsValidToken()
        {
            if (_settings.TokenExpiration > DateTime.UtcNow.AddMinutes(5)) return;
            var authResponse = await Auth();
            _settings.AccessToken = authResponse.access_token;
            _settings.InstanceUrl = authResponse.instance_url;
            _settings.TokenExpiration = DateTime.UtcNow.AddHours(2);
        }
    }
}

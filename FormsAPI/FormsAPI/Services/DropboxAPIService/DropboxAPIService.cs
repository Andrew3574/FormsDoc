
using Amazon.Runtime.Internal.Transform;
using Azure.Identity;
using Dropbox.Api;
using Dropbox.Api.Files;
using Elastic.Clients.Elasticsearch.Nodes;
using FormsAPI.ModelsDTO.Account;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Core;
using Microsoft.Graph.Drives.Item.Items.Item.CreateUploadSession;
using Microsoft.Graph.Models;
using Microsoft.Identity.Client;
using Microsoft.Kiota.Abstractions.Authentication;
using System.Text;
using System.Text.Json;

namespace FormsAPI.Services.DropboxAPIService
{
    public class DropboxAPIService : IDropboxAPIService
    {
        private readonly DropboxAPISettings _settings;
        private DropboxClient _client;
        private readonly ILogger<DropboxAPIService> _logger;

        public DropboxAPIService(IOptions<DropboxAPISettings> options, ILogger<DropboxAPIService> logger)
        {
            _settings = options.Value;            
        }

        public async Task<bool> UploadToDropbox(BugReportDTO bugReport)
        {
            string fileName = $"Bug_{DateTime.UtcNow.ToShortDateString()}_{bugReport.ReportedBy}.json";
            string content = JsonSerializer.Serialize(bugReport);
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            var response = await _client.Files.UploadAsync($"/Reports/{fileName}", WriteMode.Add.Instance, autorename:true, body:stream);
            return response.IsFile ? true : false;
        }

        public async Task Auth(string code)
        {
            var client = new HttpClient();
            var authResponse = await AuthByCode(client,code);
            var config = new DropboxClientConfig("BugReportsUploader"){HttpClient = client};
            _client = new DropboxClient(authResponse!.access_token, authResponse.refresh_token, _settings.ClientId, _settings.ClientSecret, config);
        }

        private async Task<DropboxAuthResponse?> AuthByCode(HttpClient client,string code)
        {            
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("client_id", _settings.ClientId),
                new KeyValuePair<string, string>("client_secret", _settings.ClientSecret)
            });
            var response = await client.PostAsync($"{_settings.CodeAuthEndpoint}",content);
            response.EnsureSuccessStatusCode();            
            return JsonSerializer.Deserialize<DropboxAuthResponse>(await response.Content.ReadAsStringAsync());
        }
    }
}

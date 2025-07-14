
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
        private readonly DropboxClient _client;
        private readonly ILogger<DropboxAPIService> _logger;

        public DropboxAPIService(IOptions<DropboxAPISettings> options, ILogger<DropboxAPIService> logger)
        {
            _settings = options.Value;
            _client = new DropboxClient(_settings.Token);
        }

        public async Task<bool> UploadToDropbox(BugReportDTO bugReport)
        {
            string fileName = $"Bug_{DateTime.UtcNow.ToShortDateString()}_{bugReport.ReportedBy}.json";
            string content = JsonSerializer.Serialize(bugReport);
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            var response = await _client.Files.UploadAsync($"/Reports/{fileName}", WriteMode.Add.Instance, autorename:true, body:stream);
            return response.IsFile ? true : false;
        }       

    }
}

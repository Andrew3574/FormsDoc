using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Bulk;
using Elastic.Clients.Elasticsearch.Nodes;
using Models;
using OnixLabs.Core.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;
using FormsAPI.ModelsDTO.Forms;

namespace FormsAPI.Services
{
    public class ElasticsearchService
    {
        private readonly ElasticsearchClient _client;

        public ElasticsearchService(string uri = "http://localhost:9200")
        {
            var settings = new ElasticsearchClientSettings(new Uri(uri)).DefaultIndex("forms");
            _client = new ElasticsearchClient(settings);
        }
/*
        public async Task<IEnumerable<Form>> FullTextSearch(string query)
        {

        }*/

        public async Task IndexAllExistingForms(IEnumerable<FormDTO> forms)
        {
            var bulkRequest = new BulkRequest("forms") { Operations = new List<IBulkOperation>() };
            forms.ForEach(form => bulkRequest.Operations.Add(new BulkIndexOperation<FormDTO>(form)));
            var response = await _client.BulkAsync(bulkRequest);
            if (response.IsSuccess())
            {

            }
        }

        public async Task IndexForm(Form form)
        {
            await _client.IndexAsync(form, index=>index.Index("forms"));
        }

        public ElasticsearchClient GetClient()
        {
            return _client;
        }
    }
}

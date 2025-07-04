using Elastic.Clients.Elasticsearch;
using FormsAPI.ModelsDTO.Forms.CRUD_DTO;
using Microsoft.Extensions.Options;

namespace FormsAPI.Services.Elastic
{
    public class ElasticsearchService : IElasticService
    {
        private readonly ElasticsearchClient _client;
        private readonly ElasticSettings _elasticSettings;

        public ElasticsearchService(IOptions<ElasticSettings> options)
        {
            _elasticSettings = options.Value;
            var settings = new ElasticsearchClientSettings(new Uri(_elasticSettings.Url))
                //.Authentication()
                .DefaultIndex(_elasticSettings.DefaultIndex);
            _client = new ElasticsearchClient(settings);
        }

        public async Task CreateIndexIfNotExists(string indexName)
        {
            if(!_client.Indices.Exists(indexName).Exists)
                await _client.Indices.CreateAsync(indexName);
        }

        public async Task<bool> AddOrUpdate(FormDTO form)
        {
            var response = await _client.IndexAsync(form, idx =>
            {
                idx.Index(_elasticSettings.DefaultIndex)
                .OpType(OpType.Index);
            });
            return response.IsValidResponse;
        }

        public async Task<bool> AddOrUpdateBulk(IEnumerable<FormDTO> forms)
        {
            var response = await _client.BulkAsync(b =>
            {
                b.Index(_elasticSettings.DefaultIndex)
                    .UpdateMany(forms, (fd, f) => fd.Doc(f).DocAsUpsert(true));
            }); 
            return response.IsValidResponse;
        }

        public async Task<FormDTO?> Get(string key)
        {
            var response = await _client.GetAsync<FormDTO>(key, g =>
            {
                g.Index(_elasticSettings.DefaultIndex);
            });
            return response.Source;
        }

        public async Task<List<FormDTO>?> GetAll()
        {
            var response = await _client.SearchAsync<FormDTO>(g =>
            {
                g.Index(_elasticSettings.DefaultIndex);
            });
            return response.IsValidResponse ? response.Documents.ToList() : default;
        }

        public async Task<bool> Remove(string key)
        {
            var response = await _client.DeleteAsync<FormDTO>(key, g =>
            {
                g.Index(_elasticSettings.DefaultIndex);
            });
            return response.IsValidResponse;
        }

        public async Task<long?> RemoveAll()
        {
            var response = await _client.DeleteByQueryAsync<FormDTO>(g =>
            {
                g.Indices(_elasticSettings.DefaultIndex);
            });
            return response.IsValidResponse ? response.Deleted : default;
        }

        public async Task<IEnumerable<FormDTO>?> SearchFormsByQuery(string query)
        {
            var response = await _client.SearchAsync<FormDTO>(s => s
                .Index(_elasticSettings.DefaultIndex)
                .Query(q => q
                    .MultiMatch(m => m
                        .Query(query)
                        .Fields(Infer.Fields<FormDTO>(
                            f=>f.Title,
                            f=>f.Description,
                            f=>f.Topic,
                            f=>f.Tags,
                            f=>f.Comments.First().Text)
                        ).Fuzziness(new Fuzziness("AUTO"))
                    )
                )
            );
            return response.IsValidResponse ? response.Documents.ToList() : default;
        }


    }
}

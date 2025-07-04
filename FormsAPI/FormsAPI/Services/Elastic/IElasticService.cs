using FormsAPI.ModelsDTO.Forms.CRUD_DTO;

namespace FormsAPI.Services.Elastic
{
    public interface IElasticService
    {
        Task CreateIndexIfNotExists(string indexName);

        Task<bool> AddOrUpdate(FormDTO form);

        Task<bool> AddOrUpdateBulk(IEnumerable<FormDTO> forms);

        Task<FormDTO?> Get(string key);

        Task<List<FormDTO>?> GetAll();

        Task<bool> Remove(string key);

        Task<long?> RemoveAll();

        Task<IEnumerable<FormDTO>?> SearchFormsByQuery(string query);
    }
}
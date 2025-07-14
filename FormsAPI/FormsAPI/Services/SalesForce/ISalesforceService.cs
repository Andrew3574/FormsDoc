using FormsAPI.ModelsDTO.Account.Salesforce;

namespace FormsAPI.Services.SalesForce
{
    public interface ISalesforceService
    {
        public Task<SFAuthResponse?> Auth();
        public Task<bool> CreateContact(SalesforceContact contact);
    }
}
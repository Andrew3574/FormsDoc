using System.Text.Json.Serialization;

namespace FormsAPI.ModelsDTO.Account.Salesforce
{
    public class SalesforceAccount
    {
        public string Name { get; set; } = null!;
        public string Industry { get; set; } = null!;
        public string Phone { get; set; } = null!;

    }
}

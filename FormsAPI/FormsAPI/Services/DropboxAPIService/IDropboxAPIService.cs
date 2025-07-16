using FormsAPI.ModelsDTO.Account;
using Microsoft.Graph;

namespace FormsAPI.Services.DropboxAPIService
{
    public interface IDropboxAPIService
    {
        public Task<bool> UploadToDropbox(BugReportDTO bugReport);
        public Task Auth(string code);
    }
}

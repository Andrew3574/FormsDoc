using FormsAPP.Models.Forms;
using FormsAPP.Models.Forms.CRUD;
using Microsoft.AspNetCore.SignalR;

namespace FormsAPP.Hubs
{
    public class FormsHub : Hub
    {
        public async Task CommentAction(CommentModel comment)
        {
            await Clients.All.SendAsync("CommentAction", comment);
        }
        public async Task LikeAction(int formId, int likesCount)
        {
            await Clients.All.SendAsync("LikeAction", formId, likesCount);
        }
        public async Task FormCreatedAction(FormModel form)
        {
            await Clients.All.SendAsync("FormCreatedAction", form);
        }
    }
}

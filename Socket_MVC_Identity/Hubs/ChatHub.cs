using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Socket_MVC_Identity.Data;

namespace Socket_MVC_Identity.Hubs
{
    public class ChatHub : Hub
    {
        ApplicationDbContext context;
        public ChatHub()
        {
            context = new ApplicationDbContext(ApplicationDbContext.Opts());
        }
        //[Authorize(Policy = "chat")]
        public async Task SendMessage(string user, string message)
        {
            if (CheckRole(user))
            {
                await Clients.All.SendAsync("ReceiveMessage", user, message);
            }
            else
            {
                await Clients.Caller.SendAsync("ReceiveMessage", user, "nothing");
            }
        }

        public bool CheckRole(string name)
        {
            var token = context.Users.First(x => x.UserName == name).Id;
            if (context.UserRoles.Any(x => x.RoleId == "72b51e3c-6c5e-4bb9-8361-86cb682fe118" && x.UserId == token))
                return true;
            return false;
        }
    }
}

using System;
using System.Threading.Tasks;
using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    [Authorize]
    public class PresenceHub : Hub
    {
        private readonly PresenceTracker tracker;
        public PresenceHub(PresenceTracker tracker)
        {
            this.tracker = tracker;
        }

        public override async Task OnConnectedAsync()
        {
            var isOnline = await this.tracker.UserConnected(Context.User.GetUsername(), Context.ConnectionId);
            if (isOnline)
            {
                // These are the clients connected to the hub
                // Others is for everybody except the connection that triggered the current invocation
                // UserIsOnline is the name of the method that we're gonna use inside the client
                // Context.User.GetUsername() is what will pass back to the other users
                await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUsername());
            }
            

            var currentUsers = await this.tracker.GetOnlineUsers();
            await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers);
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            var isOffline = await this.tracker.UserDisconnected(Context.User.GetUsername(), Context.ConnectionId);
            if (isOffline)
                await Clients.Others.SendAsync("UserIsOffline", Context.User.GetUsername());
            
            await base.OnDisconnectedAsync(ex);
        }
    }
}
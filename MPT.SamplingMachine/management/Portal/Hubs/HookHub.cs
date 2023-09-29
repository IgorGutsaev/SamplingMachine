using Microsoft.AspNetCore.SignalR;
using MPT.Vending.API.Dto;
using System.Text.Json;

namespace Portal.Hubs
{
    public class HookHub : Hub
    {
        public HookHub() { }

        public async Task OnNewSession(Session? session)
            => await Clients.All.SendAsync("OnNewSession", JsonSerializer.Serialize(session));
    }
}

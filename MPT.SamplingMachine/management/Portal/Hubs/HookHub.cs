using Microsoft.AspNetCore.SignalR;
using MPT.Vending.API.Dto;
using System.Text.Json;

namespace Portal.Hubs
{
    public class HookHub : Hub
    {
        public HookHub() { }

        public async Task OnNewTransaction(Transaction? transaction)
            => await Clients.All.SendAsync("OnNewTransaction", JsonSerializer.Serialize(transaction));
    }
}
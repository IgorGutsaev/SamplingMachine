using Filuet.Infrastructure.Communication.Helpers;
using Microsoft.AspNetCore.Mvc;
using MPT.Vending.API.Dto;
using Portal.Hubs;
using System.Text.Json;

namespace Portal.Controllers
{
    public class HookController : Controller
    {
        public HookController(HookHub hub, IConfiguration configuration)
        {
            _hub = hub;
            _configuration = configuration;
        }

        [HttpPost("api/hook/transaction")]
        public async Task<IActionResult> Process([FromBody]TransactionHookRequest request)
        {
            string decrypted = DecryptMessage(request.Message);

            if (_hub.Clients != null)
                await _hub.OnNewTransaction(JsonSerializer.Deserialize<Transaction>(decrypted.Trim().Replace("\0", string.Empty)));

            return Ok();
        }

        private string DecryptMessage(string message)
        {
            string? secret = _configuration["HookSecret"];
            if (string.IsNullOrWhiteSpace(secret))
                throw new Exception("Hook secret is missing");

            return HookHelpers.Decrypt(secret, message);
        }

        private readonly HookHub _hub;
        private readonly IConfiguration _configuration;
    }
}

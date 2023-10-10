using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Filuet.Infrastructure.Abstractions.Converters;
using MPT.Vending.API.Dto;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace MessagingServices
{
    public class Portal2KioskMessagesSender
    {
        public Portal2KioskMessagesSender(string serviceBusConnectionString)
        {
            if (string.IsNullOrWhiteSpace(serviceBusConnectionString))
                throw new ArgumentException("Service bus connection string is mandatory");

            _busClient = new ServiceBusClient(serviceBusConnectionString);
            _administrationClient = new ServiceBusAdministrationClient(serviceBusConnectionString);

            _serializerOptions = new JsonSerializerOptions();
            _serializerOptions.Converters.Add(new CurrencyJsonConverter());
            _serializerOptions.Converters.Add(new CountryJsonConverter());
            _serializerOptions.Converters.Add(new LanguageJsonConverter());
            _serializerOptions.Converters.Add(new N2JsonConverter());

            _senders = new ConcurrentDictionary<string, ServiceBusSender>();
        }

        public async Task OnKioskHasChanged(object? sender, Kiosk revision)
            => await sendMessageToKiosk(revision.UID, "kiosk;" + JsonSerializer.Serialize(revision.OptimizeForCommunication(), _serializerOptions));

        public async Task OnProductHasChanged(object? sender, Product revision, IEnumerable<Kiosk> kiosks)
        {
            foreach (var kiosk in kiosks)
                await sendMessageToKiosk(kiosk.UID, "product;" + JsonSerializer.Serialize(revision, _serializerOptions));
        }

        private async Task sendMessageToKiosk(string kioskUid, string message)
        {
            string queueName = $"smp_{kioskUid}";
            if (!_senders.ContainsKey(queueName))
                _senders.AddOrUpdate(queueName, x => _busClient.CreateSender(queueName), (x, oldValue) => _senders[queueName]);

            if (!await _administrationClient.QueueExistsAsync(queueName))
                await _administrationClient.CreateQueueAsync(queueName);

            await _senders[queueName].SendMessageAsync(new ServiceBusMessage(message));
        }

        private readonly ServiceBusClient _busClient;
        private readonly ServiceBusAdministrationClient _administrationClient;
        private ConcurrentDictionary<string, ServiceBusSender> _senders;
        private readonly JsonSerializerOptions _serializerOptions;
    }
}
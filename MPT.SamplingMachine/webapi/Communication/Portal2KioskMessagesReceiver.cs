using Azure.Messaging.ServiceBus;

namespace webapi.Communication
{
    public class Portal2KioskMessagesReceiver
    {
        public Portal2KioskMessagesReceiver(string serviceBusConnectionString, string kioskUid)
        {
            string queueName = $"smp_{kioskUid}";
            _busClient = new ServiceBusClient(serviceBusConnectionString);

            _processor = _busClient.CreateProcessor(queueName);
            _processor.ProcessMessageAsync += async (ProcessMessageEventArgs arg) =>
            {
                var message = arg.Message;
                await arg.CompleteMessageAsync(message);
            };

            _processor.ProcessErrorAsync += async (ProcessErrorEventArgs arg) =>
            {
                var message = arg.Exception;
                // log
            };
        }

        public async Task RunAsync()
            =>  await _processor.StartProcessingAsync();

        private readonly ServiceBusClient _busClient;
        private readonly ServiceBusProcessor _processor;
    }
}
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.SignalR;

namespace webapi.Communication
{
    public class Portal2KioskMessagesReceiver
    {
        public Portal2KioskMessagesReceiver(string serviceBusConnectionString, string kioskUid, IHubContext<NotificationHub> hubContext)
        {
            string queueName = $"smp_{kioskUid}";
            _busClient = new ServiceBusClient(serviceBusConnectionString);
            _processor = _busClient.CreateProcessor(queueName);
            _processor.ProcessMessageAsync += async (ProcessMessageEventArgs arg) =>
            {
                var message = arg.Message;
                await hubContext.Clients.All.SendAsync("sync", "hello");
                await arg.CompleteMessageAsync(message);
            };

            _processor.ProcessErrorAsync += async (ProcessErrorEventArgs arg) =>
            {
                var message = arg.Exception;
                // log
            };

            //Task.Delay(10000).ContinueWith(x => { 
            //    hubContext.Clients.All.SendAsync("Send", "hello"); 
            //});
        }

        public async Task RunAsync()
            =>  await _processor.StartProcessingAsync();

        private readonly ServiceBusClient _busClient;
        private readonly ServiceBusProcessor _processor;
    }
}
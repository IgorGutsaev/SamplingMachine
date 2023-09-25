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

            _processor.ProcessMessageAsync += async (ProcessMessageEventArgs args) => {
                string body = args.Message.Body.ToString();
                await hubContext.Clients.All.SendAsync("syncKiosk", body);

                // complete the message. message is deleted from the queue
                await args.CompleteMessageAsync(args.Message);
            };

            _processor.ProcessErrorAsync += async (ProcessErrorEventArgs args) =>
            {
                //Console.WriteLine(args.Exception.ToString());
                await Task.CompletedTask;
            };
        }

        public async Task Run()
            => await _processor.StartProcessingAsync();


        private readonly ServiceBusClient _busClient;
        private readonly ServiceBusProcessor _processor;
    }
}
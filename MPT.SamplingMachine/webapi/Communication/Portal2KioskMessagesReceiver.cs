using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.SignalR;

namespace webapi.Communication
{
    public class Portal2KioskMessagesReceiver
    {
        public Portal2KioskMessagesReceiver(string serviceBusConnectionString, string kioskUid, IHubContext<NotificationHub> hubContext)
        {
            string queueName = $"smp_{kioskUid}";

            var clientOptions = new ServiceBusClientOptions {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            };

            _busClient = new ServiceBusClient(serviceBusConnectionString, clientOptions);
            _processor = _busClient.CreateProcessor(queueName, new ServiceBusProcessorOptions());
            
            _processor.ProcessMessageAsync += async (ProcessMessageEventArgs args) => {
                string body = args.Message.Body.ToString();

                string action = body.Substring(0, body.IndexOf(';', StringComparison.Ordinal));
                string message = body.Substring(body.IndexOf(';', StringComparison.Ordinal) + 1);

                switch (action)
                {
                    case "kiosk":
                        await hubContext.Clients.All.SendAsync("syncKiosk", message);
                        break;
                    case "product":
                        await hubContext.Clients.All.SendAsync("syncProduct", message);
                        break;
                    default:
                        break;
                }

                // complete the message. message is deleted from the queue
                await args.CompleteMessageAsync(args.Message);
            };

            _processor.ProcessErrorAsync += async (ProcessErrorEventArgs args) => {
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filuet.Infrastructure.Abstractions.Enums;
using FunctionApp.extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using MPT.Vending.API.Dto;

namespace FunctionApp
{
    public class NotificationFunctions
    {
        public NotificationFunctions(IConfiguration configuration) {
            _configuration = configuration;
        }

        // Run every 12 hours
        [Function("StocksAreRunningLow")]
        public async Task Run([TimerTrigger("0 */5 * * * *", RunOnStartup = true)] TimerInfo myTimer)//, ILogger log)
        //public async Task Run([TimerTrigger("0 0 */1 * * *", RunOnStartup = true)] TimerInfo myTimer)//, ILogger log)
        {
            // log.LogInformation($"Stocks check executed at: {DateTime.Now}");
            var _apiClient = SamplingMachineApiClientBuilder.GetClient();
            IEnumerable<KioskStock> stock = await _apiClient.GetStockAsync();

            foreach (var kioskStock in stock) {
                IEnumerable<ProductStock> runningLowProducts = kioskStock.Stock.Where(x => x.Quantity / (decimal)x.MaxQuantuty < 0.25m);
                if (runningLowProducts.Any()) {
                    KioskStock runningLowStock = new KioskStock { KioskUid = kioskStock.KioskUid, Stock = runningLowProducts };
                    string messageHash = runningLowStock.Md5Hash();

                    TimeSpan? lastOccured = NotificationService.Instance.LastOccurrence(messageHash);
                    
                    if (!lastOccured.HasValue) { // The notification has already been sent
                        StringBuilder messageBuilder = new StringBuilder();
                        int idx = 0;
                        foreach (ProductStock ps in runningLowProducts.OrderBy(x => x.Quantity))
                            messageBuilder.Append($";sku.name.{++idx}={ps.Name};sku.qty.{idx}={ps.Quantity}");

                        NotificationService.Instance.RememberOccurrence(messageHash);
                        NotificationService.Instance.GetNotificationManager(kioskStock.KioskUid, _configuration["AzureServiceBusConnectionString"]).AddMessageToSend(NotificationTypes.LowSkuQuantity, messageBuilder.ToString(), kioskStock.KioskUid);
                    }
                }
            }
        }

        private IConfiguration _configuration;
    }
}

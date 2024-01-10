using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using MPT.Vending.API.Dto;

namespace FunctionApp
{
    public class NotificationFunctions
    {
        // Run every 12 hours
        [FunctionName("StocksAreRunningLow")]
        public async Task Run([TimerTrigger("0 0 */12 * * *", RunOnStartup = true)]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"Stocks check executed at: {DateTime.Now}");

            var _apiClient = SamplingMachineApiClientBuilder.GetClient();
            IEnumerable<KioskStock> stock = await _apiClient.GetStockAsync();

            foreach (var ks in stock) {
                IEnumerable<ProductStock> runningLowProducts = ks.Stock.Where(x => x.Quantuty / (decimal)x.MaxQuantuty < 0.25m);
                if (runningLowProducts.Any()) {
                    //_notificationManager.AddMessageToSend(NotificationTypes.ZReportCloseSuccess, notification, ks.KioskUid);
                }
            }
        }
    }
}

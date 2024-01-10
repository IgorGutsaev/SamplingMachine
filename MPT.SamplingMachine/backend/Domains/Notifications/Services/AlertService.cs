using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Notifications.Abstractions;

namespace Services
{
    public class AlertService : IAlertService
    {
        public IEnumerable<Alert> QueuedAlerts(TimeSpan depth) {
            throw new NotImplementedException();
        }
    }
}
using MPT.Vending.API.Dto;

namespace MPT.Vending.Domains.Notifications.Abstractions
{
    public interface IAlertService {
        IEnumerable<Alert> QueuedAlerts(TimeSpan depth);
    }
}
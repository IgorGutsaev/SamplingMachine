using Filuet.Infrastructure.Abstractions.Communication;
using Filuet.Infrastructure.Communication.Notifications;
using Filuet.Infrastructure.DataProvider;
using System;
using System.Collections.Generic;

namespace FunctionApp
{
    public class NotificationService
    {
        public static NotificationService Instance;
        static NotificationService() {
            Instance = new NotificationService();
        }

        public NotificationService() {
            _memoryCachingService = new MemoryCachingService();
            _notificationManagers = new Dictionary<string, INotificationManager>();
        }

        public INotificationManager GetNotificationManager(string kioskUid, string serviceBusAddress) {
            if (string.IsNullOrWhiteSpace(kioskUid))
                return null;

            kioskUid = kioskUid.Trim().ToUpper();

            if (!_notificationManagers.ContainsKey(kioskUid))
                _notificationManagers.Add(kioskUid, new AzureServiceBusNotificationManager(serviceBusAddress, "notificationsqueue", kioskUid));

            return _notificationManagers[kioskUid];
        }

        /// <summary>
        /// Searches for the last occurence of the notification
        /// </summary>
        /// <param name="notificationHash"></param>
        /// <remarks>
        /// There's point in sending the same notification every az function loop
        /// It's better to leave duplicates out. In that regard the application stores notification hashes
        /// </remarks>
        /// <returns></returns>
        public TimeSpan? LastOccurrence(string notificationHash) {
            DateTime? datetime = _memoryCachingService.Get("notifications", 10).Get<DateTime?>(notificationHash);
            if (datetime == null)
                return null;

            return DateTime.UtcNow - datetime.Value;
        }

        /// <summary>
        /// remem
        /// </summary>
        /// <param name="notificationHash"></param>
        public void RememberOccurrence(string notificationHash)
            => _memoryCachingService.Get("notifications", 10).Set(notificationHash, DateTime.UtcNow, TimeSpan.FromDays(1).TotalMilliseconds);

        private MemoryCachingService _memoryCachingService;
        private Dictionary<string, INotificationManager> _notificationManagers;
    }
}

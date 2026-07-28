using Catalog.Domain.Interfaces;

namespace Catalog.Domain.Notifications
{
    public class NotificationCollector : INotificationCollector
    {
        private List<Notification> _notifications;

        public NotificationCollector()
        {
            _notifications = new List<Notification>();
        }

        public void AddNotification(Notification notification)
        {
            _notifications.Add(notification);
        }

        public List<Notification> GetAllNotifications()
        {
            return _notifications;
        }

        public int CountAllNotifications()
        {
            return _notifications.Count;
        }

        public List<Notification> GetAllErrorNotifications()
        {
            return _notifications.Where(n => n.Type == NotificationType.Error).ToList();
        }

        public int CountAllProcessedNotifications()
        {
            return _notifications.Count(n => n.Type == NotificationType.Processed);
        }

        public int CountAllErrorNotifications()
        {
            return _notifications.Count(n => n.Type == NotificationType.Error);
        }

        public bool HasNotification()
        {
            return _notifications.Any();
        }

        public bool HasErrorNotification()
        {
            return _notifications.Any(n => n.Type == NotificationType.Error);
        }
    }
}
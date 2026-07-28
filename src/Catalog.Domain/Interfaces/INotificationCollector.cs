using Catalog.Domain.Notifications;

namespace Catalog.Domain.Interfaces
{
    public interface INotificationCollector
    {
        bool HasNotification();

        bool HasErrorNotification();

        List<Notification> GetAllNotifications();

        int CountAllNotifications();

        List<Notification> GetAllErrorNotifications();

        int CountAllProcessedNotifications();

        int CountAllErrorNotifications();

        void AddNotification(Notification notification);
    }
}
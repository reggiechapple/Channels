using System.Collections.Generic;
using Channels.Data.Entities;

namespace Channels.Data.Repositories
{
    public interface INotificationRepository
    {
        List<UserNotification> GetUserNotifications(long userId);
        void Create(Notification notification, long channelId);
        void ReadNotification(long notificationId, long memberId);
    }
}
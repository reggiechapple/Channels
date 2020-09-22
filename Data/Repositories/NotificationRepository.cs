using System.Collections.Generic;
using System.Linq;
using Channels.Data.Entities;
using Channels.Data.Identity;
using Channels.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Channels.Data.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private ApplicationDbContext _context;
        private IHubContext<NotificationHub> _hubContext;

        public NotificationRepository(ApplicationDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public void Create(Notification notification, long channelId)
        {
            _context.Notifications.Add(notification);
            _context.SaveChanges();
            var channel = _context.Channels.Include(c => c.Members).ThenInclude(m => m.Member).ThenInclude(m => m.Identity).FirstOrDefault(c => c.Id == channelId);
            foreach (var member in channel.Members)
            {
                var userNotification = new UserNotification();
                userNotification.MemberId = member.MemberId;
                userNotification.NotificationId = notification.Id;

                _context.UserNotifications.Add(userNotification);
                _context.SaveChanges();
            }
            _hubContext.Clients.All.SendAsync("displayNotification","");
        }

        public List<UserNotification> GetUserNotifications(long userId)
        {
            return _context.UserNotifications.Where(u=>u.MemberId.Equals(userId) && !u.IsRead)
                                            .Include(n=>n.Notification)
                                            .ToList();
        }

        public void ReadNotification(long notificationId, long memberId)
        {
             var notification = _context.UserNotifications
                                        .FirstOrDefault(n=>n.MemberId.Equals(memberId) 
                                        && n.NotificationId==notificationId);
            notification.IsRead = true;
            _context.UserNotifications.Update(notification);
            _context.SaveChanges();
        }
    }
}
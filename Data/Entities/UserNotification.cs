using Channels.Data.Identity;

namespace Channels.Data.Entities
{
    public class UserNotification
    {
        public long NotificationId { get; set; }
        public Notification Notification { get; set; }
        public long MemberId { get; set; }
        public Member Member { get; set; }
        public bool IsRead { get; set; } = false;
    }
}
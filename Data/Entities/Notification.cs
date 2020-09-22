using System.Collections.Generic;

namespace Channels.Data.Entities
{
    public class Notification : Entity
    {
        public string Text { get; set; }
        public ICollection<UserNotification> Users { get; set; }
    }
}
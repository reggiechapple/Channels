using System.Collections.Generic;
using Channels.Data.Entities;

namespace Channels.Data.Identity
{
    public class Member : Profile
    {
        public ICollection<Channel> Channels { get; set; }
        public ICollection<ChannelMember> ChannelSessions { get; set; }
        public ICollection<ChannelMessage> ChannelMessages { get; set; }
        public ICollection<UserNotification> Notifications { get; set; }
    }
}
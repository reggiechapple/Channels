using Channels.Data.Identity;

namespace Channels.Data.Entities
{
    public class ChannelSubscriber
    {
        public long ChannelId { get; set; }
        public Channel Channel { get; set; }

        public long SubscriberId { get; set; }
        public Member Subscriber { get; set; }
    }
}
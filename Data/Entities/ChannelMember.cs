using Channels.Data.Identity;

namespace Channels.Data.Entities
{
    public class ChannelMember
    {
        public long ChannelId { get; set; }
        public Channel Channel { get; set; }

        public long MemberId { get; set; }
        public Member Member { get; set; }
    }
}
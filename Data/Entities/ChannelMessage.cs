using Channels.Data.Identity;

namespace Channels.Data.Entities
{
    public class ChannelMessage : Entity
    {
        public string Content { get; set; }
        
        public long ChannelId { get; set; }
        public Channel Channel { get; set; }

        public long MemberId { get; set; }
        public Member Member { get; set; }
    }
}
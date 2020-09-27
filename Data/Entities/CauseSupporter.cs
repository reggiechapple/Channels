using Channels.Data.Identity;

namespace Channels.Data.Entities
{
    public class CauseSupporter
    {
        public long CauseId { get; set; }
        public Cause Cause { get; set; }

        public long SupporterId { get; set; }
        public Member Supporter { get; set; }
    }
}
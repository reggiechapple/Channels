using Channels.Data.Identity;

namespace Channels.Data.Entities
{
    public class Donation : Entity
    {
        public decimal Amount { get; set; }
        public long CampaignId { get; set; }
        public Campaign Campaign { get; set; }
        public long DonorId { get; set; }
        public Member Donor { get; set; }
    }
}
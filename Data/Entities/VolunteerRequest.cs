using Channels.Data.Identity;

namespace Channels.Data.Entities
{
    public class VolunteerRequest : Entity
    {
        public bool Accepted { get; set; }
        public long CampaignId { get; set; }
        public Campaign Campaign { get; set; }
        public long VolunteerId { get; set; }
        public Member Volunteer { get; set; }
    }
}
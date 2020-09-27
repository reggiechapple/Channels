using System.Collections.Generic;
using Channels.Data.Identity;

namespace Channels.Data.Entities
{
    public class Campaign : Entity
    {
        public string Name { get; set; }
        public string ShortDesc { get; set; }
        public string LongDesc { get; set; }
        public decimal FundsNeeded { get; set; }
        public decimal FundsRaised { get; set; }
        public uint VolunteersNeeded { get; set; }
        public bool IsOpen { get; set; }
        public bool IsSuspended { get; set; } = false;
        public string ClosingStatement { get; set; }
        public long InitiatorId { get; set; }
        public Member Initiator { get; set; }
        public long CauseId { get; set; }
        public Cause Cause { get; set; }
        public ICollection<CampaignVolunteer> Volunteers { get; set; }
        public ICollection<Donation> Donations { get; set; }
        public ICollection<VolunteerRequest> VolunteerRequests { get; set; }
    }
}
using System;
using System.Collections.Generic;
using Channels.Data.Identity;

namespace Channels.Data.Entities
{
    public class Meeting : Entity
    {
        public string Name { get; set; }
        public string ShortDesc { get; set; }
        public string LongDesc { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsPrivate { get; set; }
        public long CoordinatorId { get; set; }
        public Member Coordinator { get; set; }
        public long? CampaignId { get; set; }
        public Campaign Campaign { get; set; }
        public ICollection<MeetingAttendee> MeetingAttendees { get; set; }
    }
}
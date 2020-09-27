using Channels.Data.Identity;

namespace Channels.Data.Entities
{
    public class MeetingAttendee
    {
        public long MeetingId { get; set; }
        public Meeting Meeting { get; set; }
        public long AttendeeId { get; set; }
        public Member Attendee { get; set; }
    }
}
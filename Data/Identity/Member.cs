using System.Collections.Generic;
using Channels.Data.Entities;

namespace Channels.Data.Identity
{
    public class Member : Profile
    {
        public ICollection<Channel> Channels { get; set; }
        public ICollection<ChannelSubscriber> Subscriptions { get; set; }
        public ICollection<ChannelMessage> ChannelMessages { get; set; }
        public ICollection<UserNotification> Notifications { get; set; }
        public ICollection<Campaign> Campaigns { get; set; }
        public ICollection<CauseSupporter> Causes { get; set; }
        public ICollection<CampaignVolunteer> Work { get; set; }
        public ICollection<Donation> Donations { get; set; }
        public ICollection<VolunteerRequest> VolunteerRequests { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Follow> Follows { get; set; }
        public ICollection<Follow> Following { get; set; }
        public ICollection<Meeting> Meetings { get; set; }
        public ICollection<MeetingAttendee> MeetingAttendance { get; set; }
    }
}
using System.Collections.Generic;
using Channels.Data.Entities;
using Channels.Data.Identity;

namespace Channels.Areas.Members.Models
{
    public class EventViewModel
    {
        public Member Member { get; set; }
        public ICollection<Meeting> Meetings { get; set; }
    }
}
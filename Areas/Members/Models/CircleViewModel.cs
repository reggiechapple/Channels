using System.Collections.Generic;
using Channels.Data.Entities;
using Channels.Data.Identity;

namespace Channels.Areas.Members.Models
{
    public class CircleViewModel
    {
        public Member Member { get; set; }
        public ICollection<Channel> Channels { get; set; }
    }
}
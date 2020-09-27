using System.Collections.Generic;
using Channels.Data.Entities;
using Channels.Data.Identity;

namespace Channels.Areas.Members.Models
{
    public class FeedViewModel
    {
        public Member Member { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}
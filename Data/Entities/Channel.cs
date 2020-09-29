using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Channels.Data.Identity;

namespace Channels.Data.Entities
{
    public class Channel : Entity
    {
        public string Name { get; set; }
        public string About { get; set; }
        public bool IsPrivate { get; set; }
        
        [Required]
        [Editable(false)]
        public string UUID { get; set; } = Guid.NewGuid().ToString();
        public long? OwnerId { get; set; }
        public Member Owner { get; set; }

        public ICollection<ChannelSubscriber> Subscribers { get; set; }
        public ICollection<ChannelMessage> Messages { get; set; }

    }
}
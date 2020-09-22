using System;
using System.ComponentModel.DataAnnotations;

namespace Channels.Data.Identity
{
    public class Profile : Entity
    {
        public string IdentityId { get; set; }
        public ApplicationUser Identity { get; set; }  // navigation property

        // [Required]
        // [Editable(false)]
        // public string Slug { get; set; } = Guid.NewGuid().ToString();
    }
}
using System.Collections.Generic;

namespace Channels.Data.Entities
{
    public enum Category
    {
        Animal = 1,
        Environment = 2,
        Human = 3
    }

    public class Cause : Entity
    {
        public string Name { get; set; }
        public Category Category { get; set; }
        public ICollection<Campaign> Campaigns { get; set; }
        public ICollection<CauseSupporter> Supporters { get; set; }
    }
}
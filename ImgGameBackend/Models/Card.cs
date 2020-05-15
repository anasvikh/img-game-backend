using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Imaginarium.Models
{
    public class Card
    {
        public int Id { get; set; }
        public string Src { get; set; }
        public int NumberInSet { get; set; }

        public int CardSetId { get; set; }
        public virtual CardSet CardSet { get; set; }

        public List<PlayCard> PlayCards { get; set; }
    }
}

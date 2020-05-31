using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Imaginarium.Models
{
    public class CardSet
    {
        public int Id { get; set; }
        public string NameEng { get; set; }
        public string NameRus { get; set; }
        public bool IsAvailable { get; set; }

        public virtual ICollection<Card> Cards { get; set; }

        public virtual ICollection<CardSetGame> CardSetGames { get; set; }

        public CardSet()
        {
            Cards = new List<Card>();
            CardSetGames = new List<CardSetGame>();
        }
    }
}

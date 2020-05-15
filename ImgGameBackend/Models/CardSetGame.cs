using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Imaginarium.Models
{
    public class CardSetGame
    {
        public int CardSetId { get; set; }
        public CardSet CardSet { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}

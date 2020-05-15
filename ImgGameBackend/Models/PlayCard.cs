using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Imaginarium.Models
{
    public class PlayCard
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int? Round { get; set; }
        public bool Used { get; set; }
        public List<string> VotedUsers { get; set; } = new List<string>();

        public int CardId { get; set; }
        public Card Card { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}

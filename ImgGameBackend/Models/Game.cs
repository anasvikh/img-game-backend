using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Imaginarium.Enums;

namespace Imaginarium.Models
{
    public class Game
    {
        public int Id { get; set; }
        public StatusType Status { get; set; } = StatusType.New;
        public int? Round { get; set; }
        public RoundType RoundType { get; set; }
        public int VotedOnRoundCount { get; set; }
        public List<User> Users { get; set; } = new List<User>();

        public List<PlayCard> PlayCards { get; set; } = new List<PlayCard>();

        public virtual ICollection<CardSetGame> CardSetGames { get; set; }

        public string ActivePlayerName { get; set; }
        public string Creator { get; set; }

        public Game()
        {
            CardSetGames = new List<CardSetGame>();
        }

        public void AddUser(string username, ChipColorEnum color, int order)
        {
            Users.Add(new User()
            {
                Name = username,
                ChipId = color,
                Order = order
            });
        }
    }

    public enum StatusType
    {
        New,
        Active,
        Ended
    }

    public enum RoundType
    {
        Choice,
        Guessing,
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Imaginarium.Enums;

namespace Imaginarium.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ConnectionId { get; set; }
        public int Points { get; set; }
        public ChipColorEnum ChipColor { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}

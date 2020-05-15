using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Imaginarium.Enums;

namespace Imaginarium.ViewModels
{
    public class RoundResultsViewModel
    {
        public PlayCardViewModel ActivePlayCard { get; set; }
        public List<PlayerResults> ResultsList { get; set; }
    }

    public class PlayerResults
    {
        public string Username { get; set; }
        public int RoundPoints { get; set; }
        public int TotalPoints { get; set; }
        public ChipColorEnum ChipColor { get; set; }
    }
}

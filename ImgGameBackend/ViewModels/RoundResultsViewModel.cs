using System;
using System.Collections.Generic;

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
        public int ChipId { get; set; }
    }
}

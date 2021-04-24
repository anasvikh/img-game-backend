using System.Collections.Generic;

namespace Imaginarium.ViewModels
{
    public class PlayCardViewModel
    {
        public int Id { get; set; }
        public string Src { get; set; }
        public int NumberInSet { get; set; }
    }

    public class PlayCardResultsViewModel: PlayCardViewModel
    {
        public List<UserResultViewModel> Players { get; set; }
        public UserResultViewModel CardOwner { get; set; }
    }

    public class UserResultViewModel
    {
        public string Name { get; set; }
        public int ChipId { get; set; }
        public bool IsCardOwner { get; set; }
    }
}

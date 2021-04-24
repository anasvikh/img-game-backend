namespace Imaginarium.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ConnectionId { get; set; }
        public int Points { get; set; }
        public int ChipId { get; set; }
        public int Order { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}

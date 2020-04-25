using System.Collections.Generic;

namespace server.Models.Entities
{
    public class Lobby
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public string CreatorId { get; set; }

        public List<Player> Players { get; }

        public Lobby()
        {
            Players = new List<Player>();
        }
    }
}
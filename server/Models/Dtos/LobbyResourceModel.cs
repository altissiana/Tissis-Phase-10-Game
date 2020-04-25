using System.Collections.Generic;

namespace server.Models.Dtos
{
    public class LobbyResourceModel
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public string Id { get; set; }
        public string CreatorId { get; set; }
        public List<PlayerResourceModel> Players { get; set; }

        public LobbyResourceModel()
        {
            Players = new List<PlayerResourceModel>();
        }
    }
}
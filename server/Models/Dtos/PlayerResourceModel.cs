using System.Collections.Generic;

namespace server.Models.Dtos
{
    public class PlayerResourceModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<CardResourceModel> Hand { get; set; }
        public PhaseResourceModel Phase { get; set; }
        public int Stage { get; set; }
        public bool HasCompletedPhase { get; set; }
    }
}
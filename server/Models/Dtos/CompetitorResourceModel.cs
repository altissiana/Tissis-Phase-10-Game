namespace server.Models.Dtos
{
    public class CompetitorResourceModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public PhaseResourceModel Phase { get; set; }
        public int Stage { get; set; }
        public bool HasCompletedPhase { get; set; }

        // Don't add List<Card> Hand; here. We don't want to expose that.
    }
}
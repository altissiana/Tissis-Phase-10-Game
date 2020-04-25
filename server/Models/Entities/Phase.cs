using System.Collections.Generic;

namespace server.Models.Entities
{
    public class Phase
    {
        public string PlayerId { get; set; }
        public List<CardCombination> CardCombinations { get; }
        public PhaseType PhaseType { get; set; }


        public Phase(string playerId, PhaseType phaseType, List<CardCombination> cardCombinations)
        {
            this.PlayerId = playerId;
            this.PhaseType = phaseType;
            this.CardCombinations = cardCombinations;
        }
    }
}
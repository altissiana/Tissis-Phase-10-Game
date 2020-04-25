using System.Collections.Generic;
using System.Linq;

namespace server.Models.Entities
{
    public class Player
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public List<Card> Hand { get; set; }
        public Phase Phase { get; set; }
        public Stage Stage { get; set; }

        public bool HasCompletedPhase { get; set; }
        public int RoundScore { get; set; }
        public int TotalScore { get; set; }

        public Player()
        {
            Hand = new List<Card>();
        }

        public void UpdateTotals()
        {
            int total = Hand.Sum(card => card.GetCardTotal());

            if (!HasCompletedPhase)
            {
                total += Phase.CardCombinations.Sum(combo => combo.Cards.Sum(card => card.GetCardTotal()));
            }

            RoundScore = total;
            TotalScore += RoundScore;
        }
    }
}
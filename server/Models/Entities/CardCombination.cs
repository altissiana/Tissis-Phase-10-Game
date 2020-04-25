using System.Collections.Generic;

namespace server.Models.Entities
{
    public abstract class CardCombination
    {
        public int Length { get; set; }
        public List<Card> Cards { get; set; }
        public CardCombinationType CardCombinationType { get; set; }

        protected CardCombination(int length)
        {
            Length = length;
            Cards = new List<Card>();
        }

        public abstract bool IsValid();
    }
}
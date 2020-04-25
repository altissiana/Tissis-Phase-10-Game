using System.Collections.Generic;
using System.Linq;

namespace server.Models.Entities
{
    public class SetCardCombination : CardCombination
    {
        public SetCardCombination(int length) : base(length) 
        {
            CardCombinationType = CardCombinationType.Set;
        }

        public override bool IsValid()
        {
            if (Cards.Count < Length) 
            {
                return false;
            }

            if (Cards.Exists(card => card.CardType == CardType.Skip))
            {
                return false;
            }

            Cards.Sort((a, b) => a.CardType - b.CardType);

            return Cards.All(card => card.CardType == Cards.First().CardType || card.CardType == CardType.Wild);
        }
    }
}
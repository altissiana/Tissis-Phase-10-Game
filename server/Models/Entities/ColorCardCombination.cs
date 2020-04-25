using System.Collections.Generic;
using System.Linq;

namespace server.Models.Entities
{
    public class ColorCardCombination : CardCombination
    {
        public ColorCardCombination(int length) : base(length)
        {
            CardCombinationType = CardCombinationType.Color;
        }

        public override bool IsValid()
        {
            return Cards.Count >= Length && 
                !Cards.Exists(card => card.CardType == CardType.Skip) &&
                Cards.All(card => card.CardColor == Cards.First().CardColor || card.CardType == CardType.Wild);
        }
    }
}
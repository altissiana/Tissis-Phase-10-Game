using System;

namespace server.Models.Entities
{
    public class Card
    {
        public CardType CardType { get; set; }
        public CardColor CardColor { get; set; }

        public Card(CardType cardType, CardColor cardColor) 
        {
            this.CardType = cardType;
            this.CardColor = cardColor;
        }

        public int GetCardTotal()
        {
            int total;

            switch(CardType)
            {
                case CardType.One:
                case CardType.Two:
                case CardType.Three:
                case CardType.Four:
                case CardType.Five:
                case CardType.Six:
                case CardType.Seven:
                case CardType.Eight:
                case CardType.Nine:
                    total = 5;
                    break;

                case CardType.Ten:
                case CardType.Eleven:
                case CardType.Twelve:
                    total = 10;
                    break;

                case CardType.Skip:
                    total = 15;
                    break;

                case CardType.Wild:
                    total = 25;
                    break;

                default:
                    total = 0;
                    break;
            }

            return total;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Card);
        }

        public bool Equals(Card other)
        {
            return other != null && CardType == other.CardType && CardColor == other.CardColor;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                const int HashingBase = (int) 2166136261;
                const int HashingMultiplier = 16777619;

                int hash = HashingBase;
                hash = (hash * HashingMultiplier) ^ (!Object.ReferenceEquals(null, CardType) ? CardType.GetHashCode() : 0);
                hash = (hash * HashingMultiplier) ^ (!Object.ReferenceEquals(null, CardColor) ? CardColor.GetHashCode() : 0);

                return hash;
            }
        }
    }
}
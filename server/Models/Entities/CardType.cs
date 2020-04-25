namespace server.Models.Entities
{
    public enum CardType
    {
        One = 1,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Eleven,
        Twelve,
        Wild,
        Skip
    }

    static class CardTypeExtensions
    {
        public static CardType Next(this CardType cardType)
        {
            switch (cardType)
            {
                case CardType.One:
                    return CardType.Two;
                case CardType.Two:
                    return CardType.Three;
                case CardType.Three:
                    return CardType.Four;
                case CardType.Four:
                    return CardType.Five;
                case CardType.Five:
                    return CardType.Six;
                case CardType.Six:
                    return CardType.Seven;
                case CardType.Seven:
                    return CardType.Eight;
                case CardType.Eight:
                    return CardType.Nine;
                case CardType.Nine:
                    return CardType.Ten;
                case CardType.Ten:
                    return CardType.Eleven;
                case CardType.Eleven:
                    return CardType.Twelve;
                case CardType.Twelve:
                    return CardType.Wild;
                case CardType.Wild:
                    return CardType.Skip;
                case CardType.Skip:
                    return CardType.One;
                default:
                    return CardType.One;
            }
        }
    }
}
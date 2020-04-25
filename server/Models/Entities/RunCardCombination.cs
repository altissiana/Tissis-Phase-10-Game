using System;
using System.Collections.Generic;
using System.Linq;

namespace server.Models.Entities
{
    public class RunCardCombination : CardCombination
    {
        public RunCardCombination(int length) : base(length) 
        {
            CardCombinationType = CardCombinationType.Run;
        }
 
        public override bool IsValid()
        {
            if (Cards.Count < Length)
            {
                return false;
            }

            Card firstNonWildCard = Cards.FirstOrDefault(card => card.CardType != CardType.Wild);

            if(firstNonWildCard == null)
            {
                return true;
            }
            else
            {
                CardType expectedCardType = firstNonWildCard.CardType;
                CardType originalExpectedCardType = firstNonWildCard.CardType;
                CardType minCardType = CardType.One;

                foreach(Card card in Cards)
                {
                    if (expectedCardType == CardType.Wild)
                    {
                        //We've gone too far
                        return false;
                    }
                    else if (card.CardType != CardType.Wild && card.CardType != expectedCardType)
                    {
                        return false;
                    }
                    else
                    {
                        minCardType = minCardType.Next();

                        if (card.CardType == CardType.Wild)
                        {
                            if(expectedCardType != originalExpectedCardType)
                            {
                                expectedCardType = expectedCardType.Next();
                            }

                            continue;
                        }
                        else
                        {
                            // card type is the expected card type
                            expectedCardType = expectedCardType.Next();

                            if (minCardType > expectedCardType)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace server.Models.Entities
{
    public class Deck
    {
        private Stack<Card> cards;
        private Random random;

        public Deck()
        {
            random = new Random();
            cards = new Stack<Card>();
        }

        public void Shuffle()
        {
            var values = cards.ToArray();
            cards.Clear();
            foreach(var value in values.OrderBy(x => random.Next()))
            {
                cards.Push(value);
            }
        }

        public Card GetCard()
        {
            Card card;
            cards.TryPop(out card);
            return card;
        }

        public Card PeekCard()
        {
            Card card;
            cards.TryPeek(out card);
            return card;
        }

        public IEnumerable<Card> GetCards(int numToGet)
        {
            for (int i = 0; i < numToGet; i++)
            {
                yield return GetCard();
            }
        }

        public void AddCard(Card card)
        {
            cards.Push(card);
        }

        public void BuildDeck()
        {
             for(int i = 1; i <= 12; i++)
            {
                for( int j = 0; j < 4; j++ )
                {
                    CardType cardType = (CardType)i;
                    CardColor cardColor = (CardColor)j;
                    Card card = new Card(cardType, cardColor);
                    cards.Push(card);
                    cards.Push(card);
                }
            }

            for(int i = 0; i < 8; i++)
            {
                Card card = new Card(CardType.Wild, CardColor.Any);
                cards.Push(card);
            }

            for(int i = 0; i < 4; i++)
            {
                Card card = new Card(CardType.Skip, CardColor.Any);
                cards.Push(card);
            }
        }
    }
}
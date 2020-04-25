using System.Collections.Generic;
using server.Repository;

namespace server.Models.Entities
{
    public class CardGame
    {
        private Deck deck;
        private Deck discardPile;
        public List<Player> Players { get; }
        private static int HandSize = 10;
        public string GameId { get; }
        public string ActivePlayerId { get; set; }
        public string StartPlayerId { get; set; }

        public CardGame(string id, List<Player> players)
        {
            GameId = id;
            this.Players = players;
            deck = new Deck();
            discardPile = new Deck();
        }

        public void DealRound()
        {
            discardPile = new Deck();
            deck = new Deck();
            deck.BuildDeck();
            deck.Shuffle();

            Players.ForEach(player => {
                player.Hand.Clear();
                player.Hand.AddRange(deck.GetCards(HandSize));
            });

            discardPile.AddCard(deck.GetCard());
        }

        public void SetActivePlayer(string playerId)
        {
            ActivePlayerId = playerId;
        }

        public Card PeekDiscardPile()
        {
            return discardPile.PeekCard();
        }

        public Card GetDiscardPile()
        {
            return discardPile.GetCard();
        }

        public void SetDiscardPile(Card card)
        {
            discardPile.AddCard(card);
        }

        public Card GetTopCard()
        {
            return deck.GetCard();
        }
    }
}
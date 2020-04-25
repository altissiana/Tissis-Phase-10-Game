using System;
using System.Collections.Generic;
using System.Linq;
using server.Models.Entities;

namespace server.Repository
{
    public class InMemoryCardGameRepository : ICardGameRepository
    {
        private List<CardGame> games;

        public InMemoryCardGameRepository()
        {
            games = new List<CardGame>();
        }

        public CardGame CreateNewGame(string gameId, List<Player> players)
        {
            CardGame cardGame = new CardGame(gameId, players);
            games.Add(cardGame);
            return cardGame;
        }

        public CardGame GetCardGame(string gameId)
        {
            return games.FirstOrDefault(game => game.GameId == gameId);
        }

        public void UpdateGame(CardGame cardGame)
        {
            var index = games.FindIndex(game => game.GameId == cardGame.GameId);
            games[index] = cardGame;
        }
    }
}
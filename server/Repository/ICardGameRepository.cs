using System.Collections.Generic;
using server.Models.Entities;

namespace server.Repository
{
    public interface ICardGameRepository
    {
        CardGame CreateNewGame(string gameId, List<Player> players);
        void UpdateGame(CardGame cardGame);
        CardGame GetCardGame(string gameId);
    }
}
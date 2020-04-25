using System.Collections.Generic;
using server.Models.Entities;

namespace server.Repository
{
    public interface ILobbyRepository
    {
         IEnumerable<Lobby> Get();
         Lobby Get(string lobbyId);
         Lobby Create(string name, int size, string playerName, string playerId);
         Lobby AddPlayer(string lobbyId, string playerName, string playerId);
    }
}
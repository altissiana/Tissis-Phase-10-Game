using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using server.Models.Entities;

namespace server.Repository
{
    public class InMemoryLobbyRepository : ILobbyRepository
    {
        private List<Lobby> lobbies;
        private ILogger<InMemoryLobbyRepository> logger;

        public InMemoryLobbyRepository(ILogger<InMemoryLobbyRepository> logger)
        {
            lobbies = new List<Lobby>();
            this.logger = logger;
        }

        public Lobby Create(string name, int size, string playerName, string playerId)
        {
            Lobby lobby = new Lobby();
            lobby.Name = name;
            lobby.Size = size;
            lobby.Id = DateTime.Now.Ticks.ToString();
            lobby.CreatorId = playerId;

            Player player = new Player();
            player.Name = playerName;
            player.Id = playerId;

            lobby.Players.Add(player);

            lobbies.Add(lobby);

            return lobby;
        }

        public IEnumerable<Lobby> Get()
        {
            return lobbies;
        }

        public Lobby Get(string lobbyId)
        {
            logger.LogInformation($"Received request in repo: {lobbyId}");
            foreach(Lobby selectedLobby in lobbies)
            {
                logger.LogInformation($"Lobby Id: {selectedLobby.Id}");
            }

            var lobby = lobbies.Where(foundLobby => foundLobby.Id == lobbyId).FirstOrDefault();

            if (lobby == null)
            {
                logger.LogInformation("Lobby is null");
            }
            else 
            {
                logger.LogInformation("Found lobby");
            }

            return lobby;
        }

        public Lobby AddPlayer(string lobbyId, string playerName, string playerId)
        {
            var lobby = lobbies.Where(foundLobby => foundLobby.Id == lobbyId).FirstOrDefault();

            if (lobby != null) 
            {
                if (lobby.Size > lobby.Players.Count && !lobby.Players.Exists(player => player.Id == playerId))
                {
                    lobby.Players.Add(new Player{
                        Id = playerId,
                        Name = playerName
                    });
                }
            }

            return lobby;
        }

        
    }
}
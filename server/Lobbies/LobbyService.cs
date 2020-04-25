using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using server.Hubs;
using server.Models.Dtos;
using server.Models.Entities;
using server.Repository;

namespace server.Lobbies
{
    public class LobbyService
    {
        private readonly ILobbyRepository lobbyRepository;
        private readonly IHubContext<LobbyHub, ILobbyHubClient> lobbyHub;
        private readonly IMapper mapper;

        public LobbyService(ILobbyRepository lobbyRepository, IHubContext<LobbyHub, ILobbyHubClient> lobbyHub, IMapper mapper)
        {
            this.lobbyRepository = lobbyRepository;
            this.lobbyHub = lobbyHub;
            this.mapper = mapper;
        }

        public IEnumerable<Lobby> GetLobbies()
        {
            return lobbyRepository.Get();
        }

        public Lobby GetLobby(string lobbyId)
        {
            return lobbyRepository.Get(lobbyId);
        }

        public Lobby CreateLobby(string name, int size, string playerName, string playerId)
        {
            var lobby = lobbyRepository.Create(name, size, playerName, playerId);
            
            lobbyHub.Clients.All.LobbyUpdate(mapper.Map<LobbyResourceModel>(lobby));
            return lobby;
        }

        public Lobby AddPlayer(string lobbyId, string playerName, string playerId)
        {
            var lobby = lobbyRepository.AddPlayer(lobbyId, playerName, playerId);
            lobbyHub.Clients.All.LobbyUpdate(mapper.Map<LobbyResourceModel>(lobby));
            return lobby;
        }
    }
}
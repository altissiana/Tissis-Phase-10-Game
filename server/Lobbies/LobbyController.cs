using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using server.Game;
using server.Models.Dtos;

namespace server.Lobbies
{
    [ApiController]
    [Route("api/[controller]")]
    public class LobbyController : ControllerBase
    {
        private readonly LobbyService lobbyService;
        private readonly GameService gameService;
        private readonly ILogger<LobbyController> logger;
        private readonly IMapper mapper;

        public LobbyController(LobbyService lobbyService, GameService gameService, IMapper mapper, ILogger<LobbyController> logger)
        {
            this.lobbyService = lobbyService;
            this.gameService = gameService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<LobbyResourceModel>> Get()
        {
            logger.LogInformation("Received request for lobbies");
            var response = lobbyService.GetLobbies().ToList();
            
            
            return mapper.Map<List<LobbyResourceModel>>(response);
        }

        [HttpGet("{lobbyId}")]
        public ActionResult<LobbyResourceModel> Get(string lobbyId)
        {
            logger.LogInformation($"Received lobby request: {lobbyId}");

            var lobby = lobbyService.GetLobby(lobbyId);

            return mapper.Map<LobbyResourceModel>(lobby);
        }

        [HttpPut("{lobbyId}")]
        public ActionResult<LobbyResourceModel> AddPlayer(string lobbyId, AddPlayerRequest request)
        {
            var lobby = lobbyService.AddPlayer(lobbyId, request.UserName, request.Email);
            return mapper.Map<LobbyResourceModel>(lobby);
        }

        [HttpPost("{lobbyId}/start")]
        public ActionResult Start(string lobbyId)
        {
            logger.LogInformation("start game");
            gameService.StartNewGame(lobbyId);
            return Ok();
        }

        [HttpPost]
        public ActionResult<LobbyResourceModel> Create(CreateLobbyRequest request)
        {
            var response = lobbyService.CreateLobby(request.Name, request.Size, request.PlayerName, request.PlayerId);

            return mapper.Map<LobbyResourceModel>(response);
        }
    }
}
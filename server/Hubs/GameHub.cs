using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using server.Game;
using server.Models.Dtos;
using server.Models.Entities;

namespace server.Hubs
{
    public interface IGameHubClient
    {
        Task Hand(List<CardResourceModel> hand);
        Task UpdateDiscardPile(CardResourceModel card);  
        Task UpdateActivePlayer(string playerId);  
        Task UpdateCompetitors(CompetitorResourceModel self);
        Task RequestCompetitors();
        Task NotifyRoundOver(List<ScoreCardResourceModel> totals);
        Task NotifyGameWon(List<ScoreCardResourceModel> totals);
        Task UpdateSelf();
    }
    
    public class GameHub : Hub<IGameHubClient>
    {
        private readonly GameService gameService;
        private readonly IMapper mapper;
        private readonly ILogger<GameHub> logger;
        public GameHub(GameService gameService, IMapper mapper, ILogger<GameHub> logger)
        {
            this.gameService = gameService;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task JoinGame(string gameId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
        }

        public async Task RequestHand(string gameId, string playerId)
        {
            List<Card> hand = gameService.GetHand(gameId, playerId);
            List<CardResourceModel> handResourceModel = mapper.Map<List<CardResourceModel>>(hand);
            await Clients.Caller.Hand(handResourceModel);
        }

        public PlayerResourceModel GetPlayer(string gameId, string playerId)
        {
            Player player = gameService.GetPlayer(gameId, playerId);
            var model = mapper.Map<PlayerResourceModel>(player);
            return model;
        }

        public async Task RequestDiscardPile(string gameId)
        {
            Card discardPile = gameService.PeekDiscardPile(gameId);
            await Clients.Caller.UpdateDiscardPile(mapper.Map<CardResourceModel>(discardPile));
        }

        public async Task RequestActivePlayer(string gameId)
        {
            string playerId = gameService.GetActivePlayer(gameId);
            await Clients.Caller.UpdateActivePlayer(playerId);
        }

        public async Task<CardResourceModel> GetDiscardPile(string gameId, string playerId)
        {
            Card discardPile = gameService.GetDiscardPile(gameId, playerId);
            Card newDiscardPile = gameService.PeekDiscardPile(gameId);
            await Clients.Group(gameId).UpdateDiscardPile(mapper.Map<CardResourceModel>(newDiscardPile));
            return mapper.Map<CardResourceModel>(discardPile);
        }

        public async Task SetDiscardPile(string gameId, string playerId, CardResourceModel card)
        {
            Card discardPile = mapper.Map<Card>(card);
            gameService.SetDiscardPile(gameId, playerId, discardPile);

            if (gameService.IsRoundOver(gameId, playerId))
            {
                gameService.CalculateRoundTotals(gameId);
                var totals = mapper.Map<List<ScoreCardResourceModel>>(gameService.GetPlayers(gameId));

                if(gameService.HasNextRound(gameId))
                {
                    gameService.StartNextRound(gameId);
                    
                    await Clients.Group(gameId).NotifyRoundOver(totals);
                }
                else
                {
                    await Clients.Group(gameId).NotifyGameWon(totals);
                }
            }
            else 
            {
                await Clients.Group(gameId).UpdateDiscardPile(card);
            }
            
        }

        public CardResourceModel GetTopCard(string gameId, string playerId)
        {
            Card card = gameService.GetTopCard(gameId, playerId);
            return mapper.Map<CardResourceModel>(card);
        }

        public PhaseResourceModel GetPhase(string gameId, string playerId)
        {
            Phase phase = gameService.GetPlayerPhase(gameId, playerId);

            return mapper.Map<PhaseResourceModel>(phase);
        }

        public bool CheckPhase(PhaseResourceModel phaseResourceModel)
        {
            Phase phase = mapper.Map<Phase>(phaseResourceModel);
            bool isValidPhase = gameService.CheckPhase(phase);
            logger.LogInformation($"Phase is valid: {isValidPhase}");
            return isValidPhase;
        }

        public async Task PlayPhase(string gameId, string playerId, PhaseResourceModel phaseResourceModel)
        {
            Phase phase = mapper.Map<Phase>(phaseResourceModel);
            gameService.PlayPhase(gameId, playerId, phase);
            Player player = gameService.GetPlayer(gameId, playerId);
            await Clients.OthersInGroup(gameId).UpdateCompetitors(mapper.Map<CompetitorResourceModel>(player));
        }

        public List<CompetitorResourceModel> GetCompetitors(string gameId, string playerId)
        {
            List<Player> competitors = gameService.GetCompetitors(gameId, playerId);
            return mapper.Map<List<CompetitorResourceModel>>(competitors);
        }

        public void UpdateHand(string gameId, string playerId, List<CardResourceModel> hand)
        {
            gameService.UpdatePlayerHand(gameId, playerId, mapper.Map<List<Card>>(hand));
        }

        public async Task UpdatePhase(string gameId, string playerId, PhaseResourceModel phase)
        {
            gameService.UpdatePlayerPhase(gameId, playerId, mapper.Map<Phase>(phase));
            await Clients.OthersInGroup(gameId).RequestCompetitors();
            await Clients.Group(gameId).UpdateSelf();
        }
    }
}
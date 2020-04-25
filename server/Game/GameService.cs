using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using server.Hubs;
using server.Lobbies;
using server.Models.Entities;
using server.Repository;

namespace server.Game
{
    public class GameService
    {
        private readonly LobbyService lobbyService;
        private readonly PhaseService phaseService;
        private readonly ICardGameRepository cardGameRepository;
        private readonly IHubContext<LobbyHub, ILobbyHubClient> lobbyHub;
        private readonly IHubContext<GameHub, IGameHubClient> gameHub;

        public GameService(LobbyService lobbyService, 
                            PhaseService phaseService, 
                            ICardGameRepository cardGameRepository, 
                            IHubContext<LobbyHub, ILobbyHubClient> lobbyHub,
                            IHubContext<GameHub, IGameHubClient> gameHub)
        {
            this.lobbyService = lobbyService;
            this.phaseService = phaseService;
            this.cardGameRepository = cardGameRepository;
            this.lobbyHub = lobbyHub;
            this.gameHub = gameHub;
        }
        
        public void StartNewGame(string lobbyId)
        {
            var lobby = lobbyService.GetLobby(lobbyId);

            if (lobby == null)
            {
                throw new NullReferenceException("Lobby could not be found");
            }
            else 
            {
                CardGame cardGame = cardGameRepository.CreateNewGame(lobbyId, lobby.Players);
                cardGame.Players.ForEach(player => {
                    player.Phase = phaseService.CreateNewPhase(player.Id, PhaseType.One);
                    player.Stage = Stage.Waiting;
                });
                cardGame.Players[0].Stage = Stage.Draw;
                cardGame.DealRound();
                cardGame.SetActivePlayer(cardGame.Players[0].Id);
                cardGame.StartPlayerId = cardGame.Players[0].Id;

                cardGameRepository.UpdateGame(cardGame);
                lobbyHub.Clients.Group(lobbyId).StartGame();            }
        } 

        public List<Card> GetHand(string gameId, string playerId)
        {
            CardGame cardGame = cardGameRepository.GetCardGame(gameId);

            if (cardGame == null) 
            {
                throw new NullReferenceException();
            }
            else
            {
                Player player = cardGame.Players.FirstOrDefault(player => player.Id == playerId);

                if (player == null)
                {
                    throw new NullReferenceException();
                }
                else 
                {
                    return player.Hand;
                }
            }
        }

        public void UpdatePlayerHand(string gameId, string playerId, List<Card> hand)
        {
            CardGame cardGame = cardGameRepository.GetCardGame(gameId);

            if (cardGame == null) 
            {
                throw new NullReferenceException();
            }
            else
            {
                Player player = cardGame.Players.FirstOrDefault(player => player.Id == playerId);

                if (player == null)
                {
                    throw new NullReferenceException();
                }
                else 
                {
                    player.Hand.Clear();
                    player.Hand.AddRange(hand);

                    cardGameRepository.UpdateGame(cardGame);
                }
            }
        }

        public void UpdatePlayerPhase(string gameId, string playerId, Phase phase)
        {
            CardGame cardGame = cardGameRepository.GetCardGame(gameId);

            if (cardGame == null) 
            {
                throw new NullReferenceException();
            }
            else
            {
                Player player = cardGame.Players.FirstOrDefault(player => player.Id == playerId);

                if (player == null)
                {
                    throw new NullReferenceException();
                }
                else 
                {
                    player.Phase = phase;

                    cardGameRepository.UpdateGame(cardGame);
                }
            }
        }

        public bool IsRoundOver(string gameId, string playerId)
        {
            CardGame cardGame = cardGameRepository.GetCardGame(gameId);

            if (cardGame == null) 
            {
                throw new NullReferenceException();
            }
            else
            {
                Player player = cardGame.Players.FirstOrDefault(player => player.Id == playerId);

                if (player == null)
                {
                    throw new NullReferenceException();
                }
                else 
                {
                    return player.Hand.Count == 0;
                }
            }
        }

        public void CalculateRoundTotals(string gameId)
        {
            CardGame cardGame = cardGameRepository.GetCardGame(gameId);

            if (cardGame == null) 
            {
                throw new NullReferenceException();
            }
            else
            {
                foreach(Player player in cardGame.Players)
                {
                    player.UpdateTotals();
                }
            }
        }

        public bool HasNextRound(string gameId)
        {
            CardGame cardGame = cardGameRepository.GetCardGame(gameId);

            if (cardGame == null) 
            {
                throw new NullReferenceException();
            }
            else
            {
                return !cardGame.Players.Any(player => player.Phase.PhaseType == PhaseType.Ten);
            }
        }

        public void StartNextRound(string gameId)
        {
            CardGame cardGame = cardGameRepository.GetCardGame(gameId);

            if (cardGame == null) 
            {
                throw new NullReferenceException();
            }
            else
            {
                cardGame.Players.ForEach(player => {
                    if (player.HasCompletedPhase)
                    {
                        int value = (int)player.Phase.PhaseType;
                        value++;
                        PhaseType nextPhase = (PhaseType)value;
                        player.Phase = phaseService.CreateNewPhase(player.Id, nextPhase);
                        player.HasCompletedPhase = false;
                    }
                    else
                    {
                        player.Phase = phaseService.CreateNewPhase(player.Id, player.Phase.PhaseType);
                    }

                    player.Stage = Stage.Waiting;
                });

                int startPlayerIndex = cardGame.Players.FindIndex(player => player.Id == cardGame.StartPlayerId);
                startPlayerIndex = (startPlayerIndex + 1) % cardGame.Players.Count;
                cardGame.Players[startPlayerIndex].Stage = Stage.Draw;
                cardGame.DealRound();
                cardGame.SetActivePlayer(cardGame.Players[startPlayerIndex].Id);
                cardGame.StartPlayerId = cardGame.Players[startPlayerIndex].Id;

                cardGameRepository.UpdateGame(cardGame);
            }
        }

        public Player GetPlayer(string gameId, string playerId)
        {
            CardGame cardGame = cardGameRepository.GetCardGame(gameId);

            if (cardGame == null) 
            {
                throw new NullReferenceException();
            }
            else
            {
                return cardGame.Players.First(player => player.Id == playerId);
            }
        }

        public List<Player> GetPlayers(string gameId)
        {
            CardGame cardGame = cardGameRepository.GetCardGame(gameId);

            if (cardGame == null) 
            {
                throw new NullReferenceException();
            }
            else
            {
                return cardGame.Players;
            }
        }

        public string GetActivePlayer(string gameId)
        {
            CardGame cardGame = cardGameRepository.GetCardGame(gameId);

            if (cardGame == null) 
            {
                throw new NullReferenceException();
            }
            else
            {
                return cardGame.ActivePlayerId;
            }
        }

        public Card PeekDiscardPile(string gameId)
        {
            CardGame cardGame = cardGameRepository.GetCardGame(gameId);

            if (cardGame == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                return cardGame.PeekDiscardPile();
            }
        }

        public Card GetDiscardPile(string gameId, string playerId)
        {
            CardGame cardGame = cardGameRepository.GetCardGame(gameId);

            if (cardGame == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                Player player = cardGame.Players.First(player => player.Id == playerId);

                Card card = cardGame.GetDiscardPile();
                player.Hand.Add(card);
                player.Stage = Stage.Discard;

                cardGameRepository.UpdateGame(cardGame);

                return card;
            }
        }

        public void SetDiscardPile(string gameId, string playerId, Card card)
        {
            CardGame cardGame = cardGameRepository.GetCardGame(gameId);

            if (cardGame == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                cardGame.SetDiscardPile(card);
                Player player = cardGame.Players.First(player => player.Id == playerId);
                player.Hand.Remove(card);
                player.Stage = Stage.Waiting;

                int activePlayerIndex = cardGame.Players.IndexOf(player);
                if (card.CardType == CardType.Skip)
                {
                    activePlayerIndex = (activePlayerIndex + 2) % cardGame.Players.Count;
                }
                else
                {
                    activePlayerIndex = (activePlayerIndex + 1) % cardGame.Players.Count;
                }
                

                cardGame.ActivePlayerId = cardGame.Players[activePlayerIndex].Id;
                cardGame.Players[activePlayerIndex].Stage = Stage.Draw;

                cardGameRepository.UpdateGame(cardGame);

                gameHub.Clients.Group(gameId).UpdateActivePlayer(cardGame.ActivePlayerId);
            }
        }

        public Card GetTopCard(string gameId, string playerId)
        {
            CardGame cardGame = cardGameRepository.GetCardGame(gameId);

            if (cardGame == null)
            {
                throw new NullReferenceException("Card game cannot be null");
            }
            else 
            {
                Card card = cardGame.GetTopCard();
                Player player = cardGame.Players.First(player => player.Id == playerId);
                player.Hand.Add(card);
                player.Stage = Stage.Discard;

                cardGameRepository.UpdateGame(cardGame);

                return card;
            }
        }

        public Phase GetPlayerPhase(string gameId, string playerId)
        {
            CardGame cardGame = cardGameRepository.GetCardGame(gameId);

            if (cardGame == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                Player player = cardGame.Players.First(player => player.Id == playerId);

                return player.Phase;
            }
        }

        public bool CheckPhase(Phase phase)
        {
            return phase.CardCombinations.All(combo => combo.IsValid());
        }

        public void PlayPhase(string gameId, string playerId, Phase phase)
        {
            CardGame cardGame = cardGameRepository.GetCardGame(gameId);

            if (cardGame == null)
            {
                throw new NullReferenceException();
            }
            else 
            {
                Player player = cardGame.Players.First(player => player.Id == playerId);
                player.Phase = phase;
                player.HasCompletedPhase = true;

                foreach(CardCombination combo in phase.CardCombinations)
                {
                    foreach(Card card in combo.Cards)
                    {
                        player.Hand.Remove(card);
                    }
                }

                cardGameRepository.UpdateGame(cardGame);
            }
        }

        public List<Player> GetCompetitors(string gameId, string playerId)
        {
            CardGame cardGame = cardGameRepository.GetCardGame(gameId);

            if (cardGame == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                List<Player> competitors = cardGame.Players.Where(player => player.Id != playerId).ToList();

                return competitors;
            }
        }
    }
}
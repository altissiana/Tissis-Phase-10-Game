using System;
using AutoMapper;
using server.Models.Dtos;
using server.Models.Entities;

namespace server.Mappers
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            CreateMap<Lobby, LobbyResourceModel>();
            CreateMap<LobbyResourceModel, Lobby>();
            CreateMap<Player, PlayerResourceModel>();
            CreateMap<PlayerResourceModel, Player>();
            CreateMap<Player, CompetitorResourceModel>();
            CreateMap<Player, ScoreCardResourceModel>();
            CreateMap<Card, CardResourceModel>();
            CreateMap<CardResourceModel, Card>();
            CreateMap<Phase, PhaseResourceModel>();
            CreateMap<PhaseResourceModel, Phase>();
            CreateMap<CardCombination, CardCombinationResourceModel>();
            CreateMap<CardCombinationResourceModel, CardCombination>()
                .ConstructUsing(source => CardCombinationCreator(source));
        }

        private CardCombination CardCombinationCreator(CardCombinationResourceModel model)
        {
            CardCombination cardCombination;

            switch ((CardCombinationType)model.CardCombinationType)
            {
                case CardCombinationType.Set: 
                    cardCombination = new SetCardCombination(model.Length);
                    break;
                case CardCombinationType.Run:
                    cardCombination = new RunCardCombination(model.Length);
                    break;
                case CardCombinationType.Color:
                    cardCombination = new ColorCardCombination(model.Length);
                    break;
                default:
                    throw new Exception("Couldn't map Card Combination");
            }

            return cardCombination;
        }
    }
}
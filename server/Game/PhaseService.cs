using System;
using System.Collections.Generic;
using System.Linq;
using server.Models.Entities;

namespace server.Game
{
    public class PhaseService
    {
        public Phase CreateNewPhase(string playerId, PhaseType phaseType)
        {
            Phase phase;

            switch (phaseType)
            {
                case PhaseType.One:
                    phase = CreatePhaseOne(playerId);
                    break;
                case PhaseType.Two:
                    phase = CreatePhaseTwo(playerId);
                    break;
                case PhaseType.Three:
                    phase = CreatePhaseThree(playerId);
                    break;
                case PhaseType.Four:
                    phase = CreatePhaseFour(playerId);
                    break;
                case PhaseType.Five:
                    phase = CreatePhaseFive(playerId);
                    break;
                case PhaseType.Six:
                    phase = CreatePhaseSix(playerId);
                    break;
                case PhaseType.Seven:
                    phase = CreatePhaseSeven(playerId);
                    break;
                case PhaseType.Eight:
                    phase = CreatePhaseEight(playerId);
                    break;
                case PhaseType.Nine:
                    phase = CreatePhaseNine(playerId);
                    break;
                case PhaseType.Ten:
                    phase = CreatePhaseTen(playerId);
                    break;
                default:
                    throw new Exception("Invalid phase type");
            }

            return phase;
        }

        public bool IsValid(Phase phase)
        {
            return phase.CardCombinations.All(combo => combo.IsValid());
        }

        private Phase CreatePhaseOne(string playerId)
        {
            List<CardCombination> combos = new List<CardCombination>
            {
                new SetCardCombination(3),
                new SetCardCombination(3)
            };

            return new Phase(playerId, PhaseType.One, combos);
        }

        private Phase CreatePhaseTwo(string playerId)
        {
            List<CardCombination> combos = new List<CardCombination>
            {
                new SetCardCombination(3),
                new RunCardCombination(4)
            };

            return new Phase(playerId, PhaseType.Two, combos);
        }

        private Phase CreatePhaseThree(string playerId)
        {
            List<CardCombination> combos = new List<CardCombination>
            {
                new SetCardCombination(4),
                new RunCardCombination(4)
            };

            return new Phase(playerId, PhaseType.Three, combos);
        }

        private Phase CreatePhaseFour(string playerId)
        {
            List<CardCombination> combos = new List<CardCombination>
            {
                new RunCardCombination(7)
            };

            return new Phase(playerId, PhaseType.Four, combos);
        }

        private Phase CreatePhaseFive(string playerId)
        {
            List<CardCombination> combos = new List<CardCombination>
            {
                new RunCardCombination(8)
            };

            return new Phase(playerId, PhaseType.Five, combos);
        }

        private Phase CreatePhaseSix(string playerId)
        {
            List<CardCombination> combos = new List<CardCombination>
            {
                new RunCardCombination(9)
            };

            return new Phase(playerId, PhaseType.Six, combos);
        }

        private Phase CreatePhaseSeven(string playerId)
        {
            List<CardCombination> combos = new List<CardCombination>
            {
                new SetCardCombination(4),
                new SetCardCombination(4)
            };

            return new Phase(playerId, PhaseType.Seven, combos);
        }

        private Phase CreatePhaseEight(string playerId)
        {
            List<CardCombination> combos = new List<CardCombination>
            {
                new ColorCardCombination(7)
            };

            return new Phase(playerId, PhaseType.Eight, combos);
        }

        private Phase CreatePhaseNine(string playerId)
        {
            List<CardCombination> combos = new List<CardCombination>
            {
                new SetCardCombination(5),
                new SetCardCombination(2)
            };

            return new Phase(playerId, PhaseType.Nine, combos);
        }

        private Phase CreatePhaseTen(string playerId)
        {
            List<CardCombination> combos = new List<CardCombination>
            {
                new SetCardCombination(5),
                new SetCardCombination(3)
            };

            return new Phase(playerId, PhaseType.Ten, combos);
        }
    }
}
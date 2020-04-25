using System;
using System.Collections.Generic;
using server.Models.Entities;
using Xunit;

namespace server_test
{
    public class RunCardCombinationTest
    {
        public class IsValidShould
        {
            [Fact]
            public void ReturnFalseWhenNotLongEnough()
            {
                RunCardCombination combo = new RunCardCombination(3);
                
                Assert.False(combo.IsValid());
            }

            [Fact]
            public void ReturnTrueForSimpleCase()
            {
                RunCardCombination combo = new RunCardCombination(3);
                combo.Cards.AddRange(new List<Card>(){
                    new Card(CardType.One, CardColor.Green),
                    new Card(CardType.Two, CardColor.Blue),
                    new Card(CardType.Three, CardColor.Red)
                });

                Assert.True(combo.IsValid());
            }

            [Fact]
            public void ReturnTrueForAllWilds()
            {
                RunCardCombination combo = new RunCardCombination(3);
                combo.Cards.AddRange(new List<Card>(){
                    new Card(CardType.Wild, CardColor.Any),
                    new Card(CardType.Wild, CardColor.Any),
                    new Card(CardType.Wild, CardColor.Any)
                });

                Assert.True(combo.IsValid());
            }

            [Fact]
            public void ReturnTrueWhenFirstCardIsWild()
            {
                RunCardCombination combo = new RunCardCombination(3);
                combo.Cards.AddRange(new List<Card>(){
                    new Card(CardType.Wild, CardColor.Any),
                    new Card(CardType.Six, CardColor.Any),
                    new Card(CardType.Seven, CardColor.Any)
                });

                Assert.True(combo.IsValid());
            }

            [Fact]
            public void ReturnTrueWhenMiddleCardIsWild()
            {
                RunCardCombination combo = new RunCardCombination(3);
                combo.Cards.AddRange(new List<Card>(){
                    new Card(CardType.Five, CardColor.Any),
                    new Card(CardType.Wild, CardColor.Any),
                    new Card(CardType.Seven, CardColor.Any)
                });

                Assert.True(combo.IsValid());
            }

            [Fact]
            public void ReturnTrueWhenLastCardIsWild()
            {
                RunCardCombination combo = new RunCardCombination(3);
                combo.Cards.AddRange(new List<Card>(){
                    new Card(CardType.Five, CardColor.Any),
                    new Card(CardType.Six, CardColor.Any),
                    new Card(CardType.Wild, CardColor.Any)
                });

                Assert.True(combo.IsValid());
            }

            [Fact]
            public void ReturnTrueWhenFirstTwoCardsAreWild()
            {
                RunCardCombination combo = new RunCardCombination(3);
                combo.Cards.AddRange(new List<Card>(){
                    new Card(CardType.Wild, CardColor.Any),
                    new Card(CardType.Wild, CardColor.Any),
                    new Card(CardType.Three, CardColor.Any)
                });

                Assert.True(combo.IsValid());
            }

            [Fact]
            public void ReturnTrueWhenFirstAndLastCardsAreWild()
            {
                RunCardCombination combo = new RunCardCombination(3);
                combo.Cards.AddRange(new List<Card>(){
                    new Card(CardType.Wild, CardColor.Any),
                    new Card(CardType.Two, CardColor.Any),
                    new Card(CardType.Wild, CardColor.Any)
                });

                Assert.True(combo.IsValid());
            }

            [Fact]
            public void ReturnTrueWhenLastTwoCardsAreWild()
            {
                RunCardCombination combo = new RunCardCombination(3);
                combo.Cards.AddRange(new List<Card>(){
                    new Card(CardType.One, CardColor.Any),
                    new Card(CardType.Wild, CardColor.Any),
                    new Card(CardType.Wild, CardColor.Any)
                });

                Assert.True(combo.IsValid());
            }

            [Fact]
            public void ReturnFalseWhenNotValidRun()
            {
                RunCardCombination combo = new RunCardCombination(3);
                combo.Cards.AddRange(new List<Card>(){
                    new Card(CardType.One, CardColor.Any),
                    new Card(CardType.Two, CardColor.Any),
                    new Card(CardType.Four, CardColor.Any)
                });

                Assert.False(combo.IsValid());
            }

            [Fact]
            public void ReturnFalseWhenNotValidRunWithWild()
            {
                RunCardCombination combo = new RunCardCombination(3);
                combo.Cards.AddRange(new List<Card>(){
                    new Card(CardType.One, CardColor.Any),
                    new Card(CardType.Wild, CardColor.Any),
                    new Card(CardType.Four, CardColor.Any)
                });

                Assert.False(combo.IsValid());
            }

            [Fact]
            public void ReturnFalseWhenLastNumberIsNotValid()
            {
                RunCardCombination combo = new RunCardCombination(3);
                combo.Cards.AddRange(new List<Card>(){
                    new Card(CardType.Wild, CardColor.Any),
                    new Card(CardType.Wild, CardColor.Any),
                    new Card(CardType.One, CardColor.Any)
                });

                Assert.False(combo.IsValid());
            }

            [Fact]
            public void ReturnsTrueWhenValidWithWildsInMiddle()
            {
                RunCardCombination combo = new RunCardCombination(5);
                combo.Cards.AddRange(new List<Card>(){
                    new Card(CardType.Four, CardColor.Any),
                    new Card(CardType.Wild, CardColor.Any),
                    new Card(CardType.Wild, CardColor.Any),
                    new Card(CardType.Wild, CardColor.Any),
                    new Card(CardType.Eight, CardColor.Any)
                });

                Assert.True(combo.IsValid());
            }

            [Fact]
            public void ReturnsFalseWhenNotValidWithWildsInMiddleLowNumber()
            {
                RunCardCombination combo = new RunCardCombination(5);
                combo.Cards.AddRange(new List<Card>(){
                    new Card(CardType.Four, CardColor.Any),
                    new Card(CardType.Wild, CardColor.Any),
                    new Card(CardType.Wild, CardColor.Any),
                    new Card(CardType.Wild, CardColor.Any),
                    new Card(CardType.Seven, CardColor.Any)
                });

                Assert.False(combo.IsValid());
            }

            [Fact]
            public void ReturnsFalseWhenNotValidWithWildsInMiddleHighNumber()
            {
                RunCardCombination combo = new RunCardCombination(5);
                combo.Cards.AddRange(new List<Card>(){
                    new Card(CardType.Four, CardColor.Any),
                    new Card(CardType.Wild, CardColor.Any),
                    new Card(CardType.Wild, CardColor.Any),
                    new Card(CardType.Wild, CardColor.Any),
                    new Card(CardType.Nine, CardColor.Any)
                });

                Assert.False(combo.IsValid());
            }

            [Fact]
            public void ReturnsFalseWhenRunTooHigh()
            {
                RunCardCombination combo = new RunCardCombination(3);
                combo.Cards.AddRange(new List<Card>(){
                    new Card(CardType.Twelve, CardColor.Any),
                    new Card(CardType.Wild, CardColor.Any),
                    new Card(CardType.Wild, CardColor.Any)
                });

                Assert.False(combo.IsValid());
            }

            [Fact]
            public void ReturnsFalseWhenContainsSkipCard()
            {
                RunCardCombination combo = new RunCardCombination(3);
                combo.Cards.AddRange(new List<Card>(){
                    new Card(CardType.One, CardColor.Any),
                    new Card(CardType.Two, CardColor.Any),
                    new Card(CardType.Skip, CardColor.Any)
                });

                Assert.False(combo.IsValid());
            }
        }
    }
}

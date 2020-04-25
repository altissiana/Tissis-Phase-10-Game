using System;
using System.Collections.Generic;
using server.Models.Entities;
using Xunit;

namespace server_test
{
    public class PlayerTest
    {
        public class UpdateTotalsShould
        {
            [Fact]
            public void SetTotalTo25WhenWild()
            {
                Player player = new Player
                {
                    Hand = new List<Card>
                    {
                        new Card(CardType.Wild, CardColor.Any)
                    }
                };

                player.UpdateTotals();

                Assert.Equal(player.RoundScore, 25);
                Assert.Equal(player.TotalScore, 25);
            }

            [Fact]
            public void SetComplicatedHandTo85() 
            {
                Player player = new Player
                {
                    Hand = new List<Card>
                    {
                        new Card(CardType.Two, CardColor.Any),
                        new Card(CardType.Three, CardColor.Any),
                        new Card(CardType.Three, CardColor.Any),
                        new Card(CardType.Four, CardColor.Any),
                        new Card(CardType.Eight, CardColor.Any),
                        new Card(CardType.Eight, CardColor.Any),
                        new Card(CardType.Ten, CardColor.Any),
                        new Card(CardType.Eleven, CardColor.Any),
                        new Card(CardType.Twelve, CardColor.Any),
                        new Card(CardType.Wild, CardColor.Any)
                    }
                };

                player.UpdateTotals();

                Assert.Equal(player.RoundScore, 85);
                Assert.Equal(player.TotalScore, 85);
            }
        }
    }
}

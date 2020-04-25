using System;
using System.Collections.Generic;
using server.Models.Entities;
using Xunit;

namespace server_test
{
    public class SetCardCombinationTest
    {
        public class IsValidShould
        {
            [Fact]
            public void ReturnTrueWithTwoWilds()
            {
                RunCardCombination combo = new RunCardCombination(3);
                combo.Cards.AddRange(new List<Card>(){
                    new Card(CardType.Wild, CardColor.Green),
                    new Card(CardType.Wild, CardColor.Blue),
                    new Card(CardType.Eight, CardColor.Red)
                });
                
                Assert.True(combo.IsValid());
            }
        }
    }
}

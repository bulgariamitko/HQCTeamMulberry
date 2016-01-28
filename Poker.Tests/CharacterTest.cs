namespace Poker.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Poker.Models.Characters;

    [TestClass]
    public class CharacterTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Name_SetNull_ShouldThrowException()
        {
            var player = new Player(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void StartCard_SetNegative_ShouldThrowException()
        {
            var player = new Player("player");

            player.StartCard = -2;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void StartCard_SetMoreThenExistingCards_ShouldThrowException()
        {
            var player = new Player("player");

            player.StartCard = 120;
        }

        [TestMethod]
        public void Chips_SetNegative_ShouldSetToZero()
        {
            var player = new Player("player");
            player.Chips = -120;

            Assert.AreEqual(0, player.Chips);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Panel_SetNull_ShouldThrowException()
        {
            var player = new Player("player");

            player.Panel = null;
        }
    }
}

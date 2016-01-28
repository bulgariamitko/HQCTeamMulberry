namespace Poker.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using Poker.Data;

    [TestClass]
    public class GameDatabaseTest
    {
        private GameDatabase testedGameDatabase;

        [TestInitialize]
        public void TestInitialize()
        {
            this.testedGameDatabase = new GameDatabase();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PlayersGameStatus_SetNull_ShouldThrowException()
        {
            this.testedGameDatabase.PlayersGameStatus = null;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Win_SetNull_ShouldThrowException()
        {
            this.testedGameDatabase.Win = null;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CheckWinners_SetNull_ShouldThrowException()
        {
            this.testedGameDatabase.CheckWinners = null;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Chips_SetNull_ShouldThrowException()
        {
            this.testedGameDatabase.Chips = null;
        }
    }
}

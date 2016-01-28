// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameDatabaseTest.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the GameDatabaseTest type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Poker.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Poker.Data;

    /// <summary>
    /// The game database test.
    /// </summary>
    [TestClass]
    public class GameDatabaseTest
    {
        /// <summary>
        /// The tested game database.
        /// </summary>
        private GameDatabase testedGameDatabase;

        /// <summary>
        /// The test initialize.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            this.testedGameDatabase = new GameDatabase();
        }

        /// <summary>
        /// The players game status_ set null_ should throw exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PlayersGameStatus_SetNull_ShouldThrowException()
        {
            this.testedGameDatabase.PlayersGameStatus = null;
        }

        /// <summary>
        /// The win_ set null_ should throw exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Win_SetNull_ShouldThrowException()
        {
            this.testedGameDatabase.Win = null;
        }

        /// <summary>
        /// The check winners_ set null_ should throw exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CheckWinners_SetNull_ShouldThrowException()
        {
            this.testedGameDatabase.CheckWinners = null;
        }

        /// <summary>
        /// The chips_ set null_ should throw exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Chips_SetNull_ShouldThrowException()
        {
            this.testedGameDatabase.Chips = null;
        }
    }
}

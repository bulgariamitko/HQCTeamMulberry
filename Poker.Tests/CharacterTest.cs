// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CharacterTest.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the CharacterTest type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Poker.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Poker.Models.Characters;

    /// <summary>
    /// The character test.
    /// </summary>
    [TestClass]
    public class CharacterTest
    {
        /// <summary>
        /// The name_ set null_ should throw exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Name_SetNull_ShouldThrowException()
        {
            var player = new Player(null);
        }

        /// <summary>
        /// The start card_ set negative_ should throw exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void StartCard_SetNegative_ShouldThrowException()
        {
            var player = new Player("player");

            player.StartCard = -2;
        }

        /// <summary>
        /// The start card_ set more then existing cards_ should throw exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void StartCard_SetMoreThenExistingCards_ShouldThrowException()
        {
            var player = new Player("player");

            player.StartCard = 120;
        }

        /// <summary>
        /// The chips_ set negative_ should set to zero.
        /// </summary>
        [TestMethod]
        public void Chips_SetNegative_ShouldSetToZero()
        {
            var player = new Player("player");
            player.Chips = -120;

            Assert.AreEqual(0, player.Chips);
        }

        /// <summary>
        /// The panel_ set null_ should throw exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Panel_SetNull_ShouldThrowException()
        {
            var player = new Player("player");

            player.Panel = null;
        }
    }
}

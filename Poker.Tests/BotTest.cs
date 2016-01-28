// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BotTest.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the BotTest type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Poker.Tests
{
    using System;
    using System.Windows.Forms;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Poker.Models.Characters;

    /// <summary>
    /// The bot test.
    /// </summary>
    [TestClass]
    public class BotTest
    {
        /// <summary>
        /// The vertical location coordinate_ set negative_ should throw exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void VerticalLocationCoordinate_SetNegative_ShouldThrowException()
        {
            var testedBot = new Bot("tester", 1, -1, 15, AnchorStyles.Bottom, AnchorStyles.Left);
        }

        /// <summary>
        /// The horizontal location coordinate_ set negative_ should throw exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void HorizontalLocationCoordinate_SetNegative_ShouldThrowException()
        {
            var testedBot = new Bot("tester", 1, 420, -1, AnchorStyles.Bottom, AnchorStyles.Left);
        }
        
    }
}

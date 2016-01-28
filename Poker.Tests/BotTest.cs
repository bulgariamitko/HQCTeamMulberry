namespace Poker.Tests
{
    using System;
    using System.Windows.Forms;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Poker.Models.Characters;

    [TestClass]
    public class BotTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void VerticalLocationCoordinate_SetNegative_ShouldThrowException()
        {
            var testedBot = new Bot("tester", 1, -1, 15, AnchorStyles.Bottom, AnchorStyles.Left);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void HorizontalLocationCoordinate_SetNegative_ShouldThrowException()
        {
            var testedBot = new Bot("tester", 1, 420, -1, AnchorStyles.Bottom, AnchorStyles.Left);
        }
        
    }
}

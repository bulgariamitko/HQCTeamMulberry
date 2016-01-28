// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerMoveTest.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the PlayerMoveTest type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Poker.Tests
{
    using System.Windows.Forms;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;

    using Moq;

    using Poker.Models.Characters;
    using Poker.Interfaces;

    /// <summary>
    /// The player move test.
    /// </summary>
    [TestClass]
    public class PlayerMoveTest
    {
        /// <summary>
        /// The tested player move.
        /// </summary>
        private PlayerMove testedPlayerMove;

        /// <summary>
        /// The player.
        /// </summary>
        private Player player;

        /// <summary>
        /// The test label.
        /// </summary>
        private Label testLabel;

        /// <summary>
        /// The rising.
        /// </summary>
        private bool rising;

        /// <summary>
        /// The raising.
        /// </summary>
        private bool raising;

        /// <summary>
        /// The needed chips to call.
        /// </summary>
        private int neededChipsToCall;

        /// <summary>
        /// The pot status.
        /// </summary>
        private TextBox potStatus;

        /// <summary>
        /// The raise.
        /// </summary>
        private int raise;

        /// <summary>
        /// The test initialize.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            this.testedPlayerMove = new PlayerMove();
            this.player = new Player("player");
            this.testLabel = new Label();
            this.testLabel.Text = "Test";
            this.rising = false;
            this.raising = true;
            this.neededChipsToCall = 10;
            this.potStatus = new TextBox();
            this.potStatus.Text = "10";
            this.raise = 5;
        }

        /// <summary>
        /// The fold_ manipulate player and label and rising_ should not throw exception.
        /// </summary>
        [TestMethod]
        public void Fold_ManipulatePlayerAndLabelAndRising_ShouldNotThrowException()
        {
            this.player.CanMakeTurn = true;
            this.player.OutOfChips = false;
            this.rising = true;

            this.testedPlayerMove.Fold(this.player, this.testLabel, ref this.rising);

            Assert.AreEqual(false, this.rising);
            Assert.AreEqual("Fold", this.testLabel.Text);
            Assert.AreEqual(false, this.player.CanMakeTurn);
            Assert.AreEqual(true, this.player.OutOfChips);
        }

        /// <summary>
        /// The check_ should manipulate player and label and raising_ should not throw exception.
        /// </summary>
        [TestMethod]
        public void Check_ShouldManipulatePlayerAndLabelAndRaising_ShouldNotThrowException()
        {
            this.player.CanMakeTurn = true;

            this.testedPlayerMove.Check(this.player, this.testLabel, ref this.raising);

            Assert.AreEqual(false, this.raising);
            Assert.AreEqual("Check", this.testLabel.Text);
            Assert.AreEqual(false, this.player.CanMakeTurn);
        }

        /// <summary>
        /// The call_ should manipulate player label text box raising_ should not throw exception.
        /// </summary>
        [TestMethod]
        public void Call_ShouldManipulatePlayerLabelTextBoxRaising_ShouldNotThrowException()
        {
            this.player.CanMakeTurn = true;
            this.player.Chips = 100;

            this.testedPlayerMove.Call(
                this.player,
                this.testLabel,
                ref this.raising,
                ref this.neededChipsToCall,
                this.potStatus);

            Assert.AreEqual(false, this.raising);
            Assert.AreEqual("Call 10", this.testLabel.Text);
            Assert.AreEqual(false, this.player.CanMakeTurn);
            Assert.AreEqual(90, this.player.Chips);
            Assert.AreEqual("20", this.potStatus.Text);
        }

        /// <summary>
        /// The raised_ should manipulate player label text box raising_ should not throw exception.
        /// </summary>
        [TestMethod]
        public void Raised_ShouldManipulatePlayerLabelTextBoxRaising_ShouldNotThrowException()
        {
            this.player.CanMakeTurn = true;
            this.player.Chips = 100;

            this.testedPlayerMove.Raised(
                this.player,
                this.testLabel,
                ref this.raising,
                ref this.raise,
                ref this.neededChipsToCall,
                this.potStatus);

            Assert.AreEqual(false, this.player.CanMakeTurn);
            Assert.AreEqual("Raise 5", this.testLabel.Text);
            Assert.AreEqual("15", this.potStatus.Text);
            Assert.AreEqual(5, this.neededChipsToCall);
            Assert.AreEqual(true, this.raising);
            Assert.AreEqual(false, this.player.CanMakeTurn);
        }

        [TestMethod]
        public void HP_ShouldManipulatePlayerLabelTextBoxRaising_ShouldNotThrowException()
        {
            var fakeRandomGenerator = new Mock<IRandomGenerator>();
            fakeRandomGenerator.Setup(r => r.RandomFromTo(It.IsAny<int>(), It.IsAny<int>())).Returns(1);

            this.testedPlayerMove.HP(this.player, this.testLabel, 1, 1, ref this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising, fakeRandomGenerator.Object);

            //TODO
        }
    }
}

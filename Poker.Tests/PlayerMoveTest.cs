namespace Poker.Tests
{
    using System;
    using System.Windows.Forms;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Poker.Models.Characters;
    using Models;

    [TestClass]
    public class PlayerMoveTest
    {
        private PlayerMove testedPlayerMove;

        private Player player;

        private Label testLabel;

        private bool rising;

        private bool raising;

        private int neededChipsToCall;

        private TextBox potStatus;

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
        }

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

        [TestMethod]
        public void Check_ShouldManipulatePlayerAndLabelAndRaising_ShouldNotThrowException()
        {
            this.player.CanMakeTurn = true;

            this.testedPlayerMove.Check(this.player, this.testLabel, ref this.raising);

            Assert.AreEqual(false, this.raising);
            Assert.AreEqual("Check", this.testLabel.Text);
            Assert.AreEqual(false, this.player.CanMakeTurn);
        }

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
    }
}

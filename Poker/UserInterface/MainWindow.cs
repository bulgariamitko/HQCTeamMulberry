namespace Poker
{
    using Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Poker.Data;
    using Poker.Models;
    using Poker.Models.Characters;

    [SuppressMessage("ReSharper", "SuggestVarOrType_BuiltInTypes")]
    public partial class MainWindow : Form
    {
        #region Variables
        public const int DefaultSetOfCards = 52;
        public const int DefaultCardsInGame = 17;
        public const int DefaultNumberOfBots = 5;
        public const int DefaultCartsOnBoard = 5;

        private ProgressBar progressBar;
        private readonly IPlayer player;
        private readonly IList<IBot> bots;
        private readonly IWinningHandType winningHandType;
        private readonly IDatabase gameDatabase;

        private Type sorted;
        private string[] cardsImageLocation;
        private int[] reservedGameCardsIndexes;
        private Image[] gameCardsAsImages;
        private PictureBox[] cardsPictureBoxArray;
        private Timer timer;
        private Timer updates;
        private Bitmap backImage;
        private Label[] botStatusLabels;

        private int Nm;

        private int neededChipsToCall;
        private int foldedPlayers;
        private double type;
        private int rounds;
        private int raise;
        private bool chipsAreAdded;
        private bool changed;
        private int height;
        private int width;
        private int winners;
        private int flop;
        private int turn;
        private int river;
        private int end;
        private int maxLeft;
        private int last;
        private int raisedTurn;
        private bool restart;
        private bool raising;
        private int secondsLeft;
        private int i;
        private int bigBlindValue;
        private int smallBlindValue;
        private int up;
        private int turnCount;

        #endregion
        public MainWindow()
        {
            this.progressBar = new ProgressBar();
            this.player = new Player("Player");
            this.bots = new List<IBot>()
                            {
                                new Bot("Bot 1", 2, 420, 15, AnchorStyles.Bottom, AnchorStyles.Left),
                                new Bot("Bot 2", 4, 65, 75, AnchorStyles.Top, AnchorStyles.Left),
                                new Bot("Bot 3", 6, 25, 590, AnchorStyles.Top, 0),
                                new Bot("Bot 4", 8, 65, 1115, AnchorStyles.Top, AnchorStyles.Right),
                                new Bot("Bot 5", 10, 420, 1160, AnchorStyles.Bottom, AnchorStyles.Right)
                            };
            this.winningHandType = new WinningHandType();
            this.gameDatabase = new GameDatabase();
            this.cardsImageLocation = Directory.GetFiles("..\\..\\Resources\\Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
            this.reservedGameCardsIndexes = new int[DefaultCardsInGame];
            this.gameCardsAsImages = new Image[DefaultSetOfCards];
            this.cardsPictureBoxArray = new PictureBox[DefaultSetOfCards];
            this.timer = new Timer();
            this.updates = new Timer();
            this.backImage = new Bitmap("..\\..\\Resources\\Assets\\Back\\Back.png");
            this.botStatusLabels = new Label[DefaultNumberOfBots];
            

            this.neededChipsToCall = 500;
            this.foldedPlayers = DefaultNumberOfBots;
            this.turnCount = 0;
            this.winners = 0;
            this.flop = 1;
            this.turn = 2;
            this.river = 3;
            this.end = 4;
            this.maxLeft = 6;
            this.raisedTurn = 1;
            this.last = 123;
            this.secondsLeft = 60;
            this.rounds = 0;
            this.raise = 0;
            this.bigBlindValue = 500;
            this.smallBlindValue = 250;
            this.up = 10000000;
            this.neededChipsToCall = this.bigBlindValue;
            this.restart = false;
            this.raising = false;

            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.updates.Start();
            this.InitializeComponent();

            this.width = this.Width;
            this.height = this.Height;
            this.Shuffle();

            this.textBoxPot.Enabled = false;
            this.textBoxPlayerChips.Enabled = false;
            this.textBoxBotOneChips.Enabled = false;
            this.textBoxBotTwoChips.Enabled = false;
            this.textBoxBotThreeChips.Enabled = false;
            this.textBoxBotFourChips.Enabled = false;
            this.textBoxBotFiveChips.Enabled = false;
            this.textBoxPlayerChips.Text = "Chips : " + this.player.Chips.ToString();
            this.textBoxBotOneChips.Text = "Chips : " + this.bots[0].Chips.ToString();
            this.textBoxBotTwoChips.Text = "Chips : " + this.bots[1].Chips.ToString();
            this.textBoxBotThreeChips.Text = "Chips : " + this.bots[2].Chips.ToString();
            this.textBoxBotFourChips.Text = "Chips : " + this.bots[3].Chips.ToString();
            this.textBoxBotFiveChips.Text = "Chips : " + this.bots[4].Chips.ToString();
            this.timer.Interval = 1 * 1 * 1000;
            this.timer.Tick += TimerTick;
            this.updates.Interval = 1 * 1 * 100;
            this.updates.Tick += UpdateTick;
            this.textBoxBigBlind.Visible = true;
            this.textBoxSmallBlind.Visible = true;
            this.buttonBigBlind.Visible = true;
            this.buttonSmallBlind.Visible = true;
            this.textBoxBigBlind.Visible = true;
            this.textBoxSmallBlind.Visible = true;
            this.buttonBigBlind.Visible = true;
            this.buttonSmallBlind.Visible = true;
            this.textBoxBigBlind.Visible = false;
            this.textBoxSmallBlind.Visible = false;
            this.buttonBigBlind.Visible = false;
            this.buttonSmallBlind.Visible = false;
            this.textBoxRaise.Text = (this.bigBlindValue * 2).ToString();
        }

        async Task Shuffle()
        {
            this.gameDatabase.PlayersGameStatus.Add(this.player.OutOfChips);
            this.gameDatabase.PlayersGameStatus.Add(this.bots[0].OutOfChips);
            this.gameDatabase.PlayersGameStatus.Add(this.bots[1].OutOfChips);
            this.gameDatabase.PlayersGameStatus.Add(this.bots[2].OutOfChips);
            this.gameDatabase.PlayersGameStatus.Add(this.bots[3].OutOfChips);
            this.gameDatabase.PlayersGameStatus.Add(this.bots[4].OutOfChips);

            this.buttonCall.Enabled = false;
            this.buttonRaise.Enabled = false;
            this.buttonFold.Enabled = false;
            this.buttonCheck.Enabled = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            bool check = false;
            int horizontal = 580, vertical = 480;
            Random r = new Random();
            for (i = this.cardsImageLocation.Length; i > 0; i--)
            {
                int j = r.Next(i);
                var k = cardsImageLocation[j];
                cardsImageLocation[j] = cardsImageLocation[i - 1];
                cardsImageLocation[i - 1] = k;
            }

            for (int cardNumber = 0; cardNumber < DefaultCardsInGame; cardNumber++)
            {
                this.gameCardsAsImages[cardNumber] = Image.FromFile(this.cardsImageLocation[cardNumber]);
                var charsToRemove = new string[] { "..\\..\\Resources\\Assets\\Cards\\", ".png" };
                foreach (var c in charsToRemove)
                {
                    this.cardsImageLocation[cardNumber] = this.cardsImageLocation[cardNumber].Replace(c, string.Empty);
                }

                this.reservedGameCardsIndexes[cardNumber] = int.Parse(this.cardsImageLocation[cardNumber]) - 1;
                this.cardsPictureBoxArray[cardNumber] = new PictureBox();
                this.cardsPictureBoxArray[cardNumber].SizeMode = PictureBoxSizeMode.StretchImage;
                this.cardsPictureBoxArray[cardNumber].Height = 130;
                this.cardsPictureBoxArray[cardNumber].Width = 80;
                this.Controls.Add(this.cardsPictureBoxArray[cardNumber]);
                this.cardsPictureBoxArray[cardNumber].Name = "pb" + cardNumber.ToString();
                await Task.Delay(200);

                #region Throwing Cards
                if (cardNumber < 2)
                {
                    if (this.cardsPictureBoxArray[0].Tag != null)
                    {
                        this.cardsPictureBoxArray[1].Tag = this.reservedGameCardsIndexes[1];
                    }

                    this.cardsPictureBoxArray[0].Tag = this.reservedGameCardsIndexes[0];
                    this.cardsPictureBoxArray[cardNumber].Image = this.gameCardsAsImages[cardNumber];
                    this.cardsPictureBoxArray[cardNumber].Anchor = AnchorStyles.Bottom;
                    this.cardsPictureBoxArray[cardNumber].Location = new Point(horizontal, vertical);
                    horizontal += this.cardsPictureBoxArray[cardNumber].Width;
                    this.Controls.Add(this.player.Panel);
                    this.player.InitializePanel(new Point(
                        this.cardsPictureBoxArray[0].Left - 10, 
                        this.cardsPictureBoxArray[0].Top - 10));
                }
                
                for (int currentBotNumber = 0; currentBotNumber < this.bots.Count; currentBotNumber++)
                {
                    if (this.bots[currentBotNumber].Chips > 0)
                    {
                        this.foldedPlayers--;
                        int currentBotStartCard = this.bots[currentBotNumber].StartCard;
                        if ((cardNumber >= currentBotStartCard) && (cardNumber < currentBotStartCard + 2))
                        {
                            if (this.cardsPictureBoxArray[currentBotStartCard].Tag != null)
                            {
                                this.cardsPictureBoxArray[currentBotStartCard + 1].Tag = this.reservedGameCardsIndexes[currentBotStartCard + 1];
                            }

                            this.cardsPictureBoxArray[currentBotStartCard].Tag = this.reservedGameCardsIndexes[currentBotStartCard];
                            if (!check)
                            {
                                vertical = this.bots[currentBotNumber].VerticalLocationCoordinate;
                                horizontal = this.bots[currentBotNumber].HorizontalLocationCoordinate;
                            }

                            check = true;
                            this.cardsPictureBoxArray[cardNumber].Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
                            this.cardsPictureBoxArray[cardNumber].Image = this.backImage;
                            this.cardsPictureBoxArray[cardNumber].Location = new Point(horizontal, vertical);
                            horizontal += this.cardsPictureBoxArray[cardNumber].Width;
                            this.cardsPictureBoxArray[cardNumber].Visible = true;
                            this.Controls.Add(this.bots[currentBotNumber].Panel);
                            this.bots[currentBotNumber].InitializePanel(new Point(
                                this.cardsPictureBoxArray[currentBotStartCard].Left - 10, 
                                this.cardsPictureBoxArray[currentBotStartCard].Top - 10));
                            this.bots[currentBotNumber].Panel.Visible = false;
                            if (cardNumber == (currentBotStartCard + 1))
                            {
                                check = false;
                            }
                        }
                    }
                }
                
                if (cardNumber >= 12)
                {
                    this.cardsPictureBoxArray[12].Tag = this.reservedGameCardsIndexes[12];
                    if (cardNumber > 12)
                    {
                        this.cardsPictureBoxArray[13].Tag = this.reservedGameCardsIndexes[13];
                    }

                    if (cardNumber > 13)
                    {
                        this.cardsPictureBoxArray[14].Tag = this.reservedGameCardsIndexes[14];
                    }

                    if (cardNumber > 14)
                    {
                        this.cardsPictureBoxArray[15].Tag = this.reservedGameCardsIndexes[15];
                    }

                    if (cardNumber > 15)
                    {
                        this.cardsPictureBoxArray[16].Tag = this.reservedGameCardsIndexes[16];
                    }

                    if (!check)
                    {
                        horizontal = 410;
                        vertical = 265;
                    }

                    check = true;
                    if (this.cardsPictureBoxArray[cardNumber] != null)
                    {
                        this.cardsPictureBoxArray[cardNumber].Anchor = AnchorStyles.None;
                        this.cardsPictureBoxArray[cardNumber].Image = this.backImage;
                        this.cardsPictureBoxArray[cardNumber].Location = new Point(horizontal, vertical);
                        horizontal += 110;
                    }
                }
                #endregion

                this.botStatusLabels[0] = this.botOneStatus;
                this.botStatusLabels[1] = this.botTwoStatus;
                this.botStatusLabels[2] = this.botThreeStatus;
                this.botStatusLabels[3] = this.botFourStatus;
                this.botStatusLabels[4] = this.botFiveStatus;

                for (int currentBotNumber = 0; currentBotNumber < this.bots.Count; currentBotNumber++)
                {
                    int currentBotStartCard = this.bots[currentBotNumber].StartCard;
                    if (this.bots[currentBotNumber].Chips <= 0)
                    {
                        this.bots[currentBotNumber].OutOfChips = true;
                        this.cardsPictureBoxArray[currentBotStartCard].Visible = false;
                        this.cardsPictureBoxArray[currentBotStartCard + 1].Visible = false;
                    }
                    else
                    {
                        this.bots[currentBotNumber].OutOfChips = false;
                        if (cardNumber == (currentBotStartCard + 1))
                        {
                            if (this.cardsPictureBoxArray[currentBotStartCard + 1] != null)
                            {
                                this.cardsPictureBoxArray[currentBotStartCard].Visible = true;
                                this.cardsPictureBoxArray[currentBotStartCard + 1].Visible = true;
                            }
                        }
                    }
                }
                
                if (cardNumber == 16)
                {
                    if (!this.restart)
                    {
                        MaximizeBox = true;
                        MinimizeBox = true;
                    }
                    this.timer.Start();
                }

                this.i = cardNumber + 1;
            }

            if (this.foldedPlayers == DefaultNumberOfBots)
            {
                DialogResult dialogResult = MessageBox.Show("Would You Like To Play Again ?", "You Won , Congratulations ! ", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Application.Restart();
                }
                else if (dialogResult == DialogResult.No)
                {
                    Application.Exit();
                }
            }
            else
            {
                this.foldedPlayers = 5;
            }

            if (this.i == DefaultCardsInGame)
            {
                this.buttonRaise.Enabled = true;
                this.buttonCall.Enabled = true;
                this.buttonRaise.Enabled = true;
                this.buttonRaise.Enabled = true;
                this.buttonFold.Enabled = true;
            }
        }

        async Task Turns()
        {
            #region Rotating
            if (!this.player.OutOfChips)
            {
                if (this.player.CanMakeTurn)
                {
                    this.FixCall(playerStatus, this.player, 1);
                    this.progressBarTimer.Visible = true;
                    this.progressBarTimer.Value = 1000;
                    this.secondsLeft = 60;
                    this.up = 10000000;
                    this.timer.Start();
                    this.buttonRaise.Enabled = true;
                    this.buttonCall.Enabled = true;
                    this.buttonRaise.Enabled = true;
                    this.buttonRaise.Enabled = true;
                    this.buttonFold.Enabled = true;
                    this.turnCount++;
                    this.FixCall(this.playerStatus, this.player, 2);
                }
            }

            if (this.player.OutOfChips || !this.player.CanMakeTurn)
            {
                await AllIn();
                if (this.player.OutOfChips && !this.player.Folded)
                {
                    if (this.buttonCall.Text.Contains("All in") == false || this.buttonRaise.Text.Contains("All in") == false)
                    {
                        this.gameDatabase.PlayersGameStatus.RemoveAt(0);
                        this.gameDatabase.PlayersGameStatus.Insert(0, null);
                        this.maxLeft--;
                        this.player.Folded = true;
                    }
                }

                await CheckRaise(0, 0);
                this.progressBarTimer.Visible = false;
                this.buttonRaise.Enabled = false;
                this.buttonCall.Enabled = false;
                this.buttonRaise.Enabled = false;
                this.buttonRaise.Enabled = false;
                this.buttonFold.Enabled = false;
                this.timer.Stop();
                
                for (int currentBotNumber = 0; currentBotNumber < this.bots.Count; currentBotNumber++)
                {
                    this.bots[currentBotNumber].CanMakeTurn = true;
                    if (!this.bots[currentBotNumber].OutOfChips)
                    {
                        if (this.bots[currentBotNumber].CanMakeTurn)
                        {
                            int currentBotStartCard = this.bots[currentBotNumber].StartCard;
                            this.FixCall(this.botStatusLabels[currentBotNumber], this.bots[currentBotNumber], 1);
                            this.FixCall(this.botStatusLabels[currentBotNumber], this.bots[currentBotNumber], 2);
                            this.Rules(currentBotStartCard, currentBotStartCard + 1, this.bots[currentBotNumber]);
                            string msg = this.bots[currentBotNumber].Name + "'s Turn";
                            MessageBox.Show(msg);
                            this.AI(currentBotStartCard, currentBotStartCard + 1, this.botStatusLabels[currentBotNumber], 0, this.bots[currentBotNumber]);
                            this.turnCount++;
                            this.last = 1;
                            this.bots[currentBotNumber].CanMakeTurn = false;
                        }
                    }

                    if (this.bots[currentBotNumber].OutOfChips && !this.bots[currentBotNumber].Folded)
                    {
                        this.gameDatabase.PlayersGameStatus.RemoveAt(currentBotNumber + 1);
                        this.gameDatabase.PlayersGameStatus.Insert(currentBotNumber + 1, null);
                        this.maxLeft--;
                        this.bots[currentBotNumber].Folded = true;
                    }

                    if (this.bots[currentBotNumber].OutOfChips || !this.bots[currentBotNumber].CanMakeTurn)
                    {
                        await this.CheckRaise(currentBotNumber + 1, currentBotNumber + 1);
                    }
                }
                
                this.player.CanMakeTurn = true;
                if (this.player.OutOfChips && !this.player.Folded)
                {
                    if (this.buttonCall.Text.Contains("All in") == false || this.buttonRaise.Text.Contains("All in") == false)
                    {
                        this.gameDatabase.PlayersGameStatus.RemoveAt(0);
                        this.gameDatabase.PlayersGameStatus.Insert(0, null);
                        this.maxLeft--;
                        this.player.Folded = true;
                    }
                }
                #endregion

                await AllIn();
                if (!this.restart)
                {
                    await Turns();
                }

                this.restart = false;
            }
        }

        private void Rules(int card1, int card2, ICharacter currentPlayer)
        {
            if (!currentPlayer.OutOfChips || card1 == 0 && card2 == 1 && this.playerStatus.Text.Contains("Fold") == false)
            {
                #region Variables
                bool done = false;
                bool vf = false;
                int[] cardsOnBoard = new int[DefaultCartsOnBoard];
                int[] straight = new int[7];
                straight[0] = this.reservedGameCardsIndexes[card1];
                straight[1] = this.reservedGameCardsIndexes[card2];
                cardsOnBoard[0] = straight[2] = this.reservedGameCardsIndexes[12];
                cardsOnBoard[1] = straight[3] = this.reservedGameCardsIndexes[13];
                cardsOnBoard[2] = straight[4] = this.reservedGameCardsIndexes[14];
                cardsOnBoard[3] = straight[5] = this.reservedGameCardsIndexes[15];
                cardsOnBoard[4] = straight[6] = this.reservedGameCardsIndexes[16];
                var a = straight.Where(o => o % 4 == 0).ToArray();
                var b = straight.Where(o => o % 4 == 1).ToArray();
                var c = straight.Where(o => o % 4 == 2).ToArray();
                var d = straight.Where(o => o % 4 == 3).ToArray();
                var st1 = a.Select(o => o / 4).Distinct().ToArray();
                var st2 = b.Select(o => o / 4).Distinct().ToArray();
                var st3 = c.Select(o => o / 4).Distinct().ToArray();
                var st4 = d.Select(o => o / 4).Distinct().ToArray();
                Array.Sort(straight);
                Array.Sort(st1);
                Array.Sort(st2);
                Array.Sort(st3);
                Array.Sort(st4);
                #endregion

                for (i = 0; i < 16; i++)
                {
                    if (reservedGameCardsIndexes[i] == int.Parse(cardsPictureBoxArray[card1].Tag.ToString()) && reservedGameCardsIndexes[i + 1] == int.Parse(cardsPictureBoxArray[card2].Tag.ToString()))
                    {
                        this.RPairFromHand(currentPlayer);

                        #region Pair or Two Pair from Table current = 2 || 0
                        this.RPairTwoPair(currentPlayer);
                        #endregion

                        #region Two Pair current = 2
                        this.RTwoPair(currentPlayer);
                        #endregion

                        #region Three of a kind current = 3
                        this.RThreeOfAKind(currentPlayer, straight);
                        #endregion

                        #region Straight current = 4
                        this.RStraight(currentPlayer, straight);
                        #endregion

                        #region Flush current = 5 || 5.5
                        this.RFlush(currentPlayer, ref vf, cardsOnBoard);
                        #endregion

                        #region Full House current = 6
                        this.RFullHouse(currentPlayer, ref done, straight);
                        #endregion

                        #region Four of a Kind current = 7
                        this.RFourOfAKind(currentPlayer, straight);
                        #endregion

                        #region Straight Flush current = 8 || 9
                        this.RStraightFlush(currentPlayer, st1, st2, st3, st4);
                        #endregion

                        #region High Card current = -1
                        this.RHighCard(currentPlayer);
                        #endregion
                    }
                }
            }
        }

        private void RStraightFlush(ICharacter currentPlayer, int[] st1, int[] st2, int[] st3, int[] st4)
        {
            if (currentPlayer.Type >= -1)
            {
                if (st1.Length >= 5)
                {
                    if (st1[0] + 4 == st1[4])
                    {
                        currentPlayer.Type = 8;
                        currentPlayer.Power = (st1.Max()) / 4 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 8 });
                        this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (st1[0] == 0 && st1[1] == 9 && st1[2] == 10 && st1[3] == 11 && st1[0] + 12 == st1[4])
                    {
                        currentPlayer.Type = 9;
                        currentPlayer.Power = (st1.Max()) / 4 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 9 });
                        this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (st2.Length >= 5)
                {
                    if (st2[0] + 4 == st2[4])
                    {
                        currentPlayer.Type = 8;
                        currentPlayer.Power = (st2.Max()) / 4 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 8 });
                        this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (st2[0] == 0 && st2[1] == 9 && st2[2] == 10 && st2[3] == 11 && st2[0] + 12 == st2[4])
                    {
                        currentPlayer.Type = 9;
                        currentPlayer.Power = (st2.Max()) / 4 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 9 });
                        this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (st3.Length >= 5)
                {
                    if (st3[0] + 4 == st3[4])
                    {
                        currentPlayer.Type = 8;
                        currentPlayer.Power = (st3.Max()) / 4 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 8 });
                        this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (st3[0] == 0 && st3[1] == 9 && st3[2] == 10 && st3[3] == 11 && st3[0] + 12 == st3[4])
                    {
                        currentPlayer.Type = 9;
                        currentPlayer.Power = (st3.Max() / 4) + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 9 });
                        this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (st4.Length >= 5)
                {
                    if (st4[0] + 4 == st4[4])
                    {
                        currentPlayer.Type = 8;
                        currentPlayer.Power = (st4.Max()) / 4 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 8 });
                        this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (st4[0] == 0 && st4[1] == 9 && st4[2] == 10 && st4[3] == 11 && st4[0] + 12 == st4[4])
                    {
                        currentPlayer.Type = 9;
                        currentPlayer.Power = (st4.Max()) / 4 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 9 });
                        this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        private void RFourOfAKind(ICharacter currentPlayer, int[] straight)
        {
            if (currentPlayer.Type >= -1)
            {
                for (int j = 0; j <= 3; j++)
                {
                    if (straight[j] / 4 == straight[j + 1] / 4 && straight[j] / 4 == straight[j + 2] / 4 &&
                        straight[j] / 4 == straight[j + 3] / 4)
                    {
                        currentPlayer.Type = 7;
                        currentPlayer.Power = (straight[j] / 4) * 4 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 7 });
                        this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (straight[j] / 4 == 0 && straight[j + 1] / 4 == 0 && straight[j + 2] / 4 == 0 && straight[j + 3] / 4 == 0)
                    {
                        currentPlayer.Type = 7;
                        currentPlayer.Power = 13 * 4 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 7 });
                        this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }
        private void RFullHouse(ICharacter currentPlayer, ref bool done, int[] straight)
        {
            if (currentPlayer.Type >= -1)
            {
                this.type = currentPlayer.Power;
                for (int j = 0; j <= 12; j++)
                {
                    var fh = straight.Where(o => o / 4 == j).ToArray();
                    if (fh.Length == 3 || done)
                    {
                        if (fh.Length == 2)
                        {
                            if (fh.Max() / 4 == 0)
                            {
                                currentPlayer.Type = 6;
                                currentPlayer.Power = 13 * 2 + currentPlayer.Type * 100;
                                this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 6 });
                                this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                                break;
                            }

                            if (fh.Max() / 4 > 0)
                            {
                                currentPlayer.Type = 6;
                                currentPlayer.Power = fh.Max() / 4 * 2 + currentPlayer.Type * 100;
                                this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 6 });
                                this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                                break;
                            }
                        }

                        if (!done)
                        {
                            if (fh.Max() / 4 == 0)
                            {
                                currentPlayer.Power = 13;
                                done = true;
                                j = -1;
                            }
                            else
                            {
                                currentPlayer.Power = fh.Max() / 4;
                                done = true;
                                j = -1;
                            }
                        }
                    }
                }

                if (currentPlayer.Type != 6)
                {
                    currentPlayer.Power = this.type;
                }
            }
        }
        private void RFlush(ICharacter currentPlayer, ref bool vf, int[] cardsOnBoard)
        {
            if (currentPlayer.Type >= -1)
            {
                var f1 = cardsOnBoard.Where(o => o % 4 == 0).ToArray();
                var f2 = cardsOnBoard.Where(o => o % 4 == 1).ToArray();
                var f3 = cardsOnBoard.Where(o => o % 4 == 2).ToArray();
                var f4 = cardsOnBoard.Where(o => o % 4 == 3).ToArray();
                if (f1.Length == 3 || f1.Length == 4)
                {
                    if (this.reservedGameCardsIndexes[i] % 4 == this.reservedGameCardsIndexes[i + 1] % 4 && 
                        this.reservedGameCardsIndexes[i] % 4 == f1[0] % 4)
                    {
                        if (this.reservedGameCardsIndexes[i] / 4 > f1.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = this.reservedGameCardsIndexes[i] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (this.reservedGameCardsIndexes[i + 1] / 4 > f1.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = this.reservedGameCardsIndexes[i + 1] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (this.reservedGameCardsIndexes[i] / 4 < f1.Max() / 4 && this.reservedGameCardsIndexes[i + 1] / 4 < f1.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f1.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f1.Length == 4)//different cards in hand
                {
                    if (this.reservedGameCardsIndexes[i] % 4 != this.reservedGameCardsIndexes[i + 1] % 4 && 
                        this.reservedGameCardsIndexes[i] % 4 == f1[0] % 4)
                    {
                        if (this.reservedGameCardsIndexes[i] / 4 > f1.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = this.reservedGameCardsIndexes[i] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f1.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }

                    if (this.reservedGameCardsIndexes[i + 1] % 4 != this.reservedGameCardsIndexes[i] % 4 &&
                        this.reservedGameCardsIndexes[i + 1] % 4 == f1[0] % 4)
                    {
                        if (this.reservedGameCardsIndexes[i + 1] / 4 > f1.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = this.reservedGameCardsIndexes[i + 1] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f1.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f1.Length == 5)
                {
                    if (this.reservedGameCardsIndexes[i] % 4 == f1[0] % 4 && this.reservedGameCardsIndexes[i] / 4 > f1.Min() / 4)
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = this.reservedGameCardsIndexes[i] + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                        this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (this.reservedGameCardsIndexes[i + 1] % 4 == f1[0] % 4 && this.reservedGameCardsIndexes[i + 1] / 4 > f1.Min() / 4)
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = reservedGameCardsIndexes[i + 1] + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                        this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (this.reservedGameCardsIndexes[i] / 4 < f1.Min() / 4 && this.reservedGameCardsIndexes[i + 1] / 4 < f1.Min())
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = f1.Max() + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                        this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f2.Length == 3 || f2.Length == 4)
                {
                    if (this.reservedGameCardsIndexes[i] % 4 == this.reservedGameCardsIndexes[i + 1] % 4 && 
                        this.reservedGameCardsIndexes[i] % 4 == f2[0] % 4)
                    {
                        if (this.reservedGameCardsIndexes[i] / 4 > f2.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = this.reservedGameCardsIndexes[i] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        if (this.reservedGameCardsIndexes[i + 1] / 4 > f2.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = reservedGameCardsIndexes[i + 1] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (this.reservedGameCardsIndexes[i] / 4 < f2.Max() / 4 && this.reservedGameCardsIndexes[i + 1] / 4 < f2.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f2.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f2.Length == 4)//different cards in hand
                {
                    if (this.reservedGameCardsIndexes[i] % 4 != this.reservedGameCardsIndexes[i + 1] % 4 &&
                        this.reservedGameCardsIndexes[i] % 4 == f2[0] % 4)
                    {
                        if (this.reservedGameCardsIndexes[i] / 4 > f2.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = this.reservedGameCardsIndexes[i] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f2.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }

                    if (this.reservedGameCardsIndexes[i + 1] % 4 != this.reservedGameCardsIndexes[i] % 4 && 
                        this.reservedGameCardsIndexes[i + 1] % 4 == f2[0] % 4)
                    {
                        if (this.reservedGameCardsIndexes[i + 1] / 4 > f2.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = this.reservedGameCardsIndexes[i + 1] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f2.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f2.Length == 5)
                {
                    if (this.reservedGameCardsIndexes[i] % 4 == f2[0] % 4 && this.reservedGameCardsIndexes[i] / 4 > f2.Min() / 4)
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = this.reservedGameCardsIndexes[i] + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                        this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (this.reservedGameCardsIndexes[i + 1] % 4 == f2[0] % 4 && this.reservedGameCardsIndexes[i + 1] / 4 > f2.Min() / 4)
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = this.reservedGameCardsIndexes[i + 1] + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                        this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (this.reservedGameCardsIndexes[i] / 4 < f2.Min() / 4 && this.reservedGameCardsIndexes[i + 1] / 4 < f2.Min())
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = f2.Max() + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                        this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f3.Length == 3 || f3.Length == 4)
                {
                    if (this.reservedGameCardsIndexes[i] % 4 == this.reservedGameCardsIndexes[i + 1] % 4 &&
                        this.reservedGameCardsIndexes[i] % 4 == f3[0] % 4)
                    {
                        if (this.reservedGameCardsIndexes[i] / 4 > f3.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = reservedGameCardsIndexes[i] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (this.reservedGameCardsIndexes[i + 1] / 4 > f3.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = this.reservedGameCardsIndexes[i + 1] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (this.reservedGameCardsIndexes[i] / 4 < f3.Max() / 4 && this.reservedGameCardsIndexes[i + 1] / 4 < f3.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f3.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f3.Length == 4)//different cards in hand
                {
                    if (this.reservedGameCardsIndexes[i] % 4 != this.reservedGameCardsIndexes[i + 1] % 4 &&
                        this.reservedGameCardsIndexes[i] % 4 == f3[0] % 4)
                    {
                        if (this.reservedGameCardsIndexes[i] / 4 > f3.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = this.reservedGameCardsIndexes[i] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f3.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }

                    if (this.reservedGameCardsIndexes[i + 1] % 4 != this.reservedGameCardsIndexes[i] % 4 && 
                        this.reservedGameCardsIndexes[i + 1] % 4 == f3[0] % 4)
                    {
                        if (this.reservedGameCardsIndexes[i + 1] / 4 > f3.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = this.reservedGameCardsIndexes[i + 1] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f3.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f3.Length == 5)
                {
                    if (this.reservedGameCardsIndexes[i] % 4 == f3[0] % 4 && this.reservedGameCardsIndexes[i] / 4 > f3.Min() / 4)
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = this.reservedGameCardsIndexes[i] + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                        this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (this.reservedGameCardsIndexes[i + 1] % 4 == f3[0] % 4 && this.reservedGameCardsIndexes[i + 1] / 4 > f3.Min() / 4)
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = this.reservedGameCardsIndexes[i + 1] + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                        this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (this.reservedGameCardsIndexes[i] / 4 < f3.Min() / 4 && this.reservedGameCardsIndexes[i + 1] / 4 < f3.Min())
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = f3.Max() + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                        this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f4.Length == 3 || f4.Length == 4)
                {
                    if (this.reservedGameCardsIndexes[i] % 4 == this.reservedGameCardsIndexes[i + 1] % 4 &&
                        this.reservedGameCardsIndexes[i] % 4 == f4[0] % 4)
                    {
                        if (this.reservedGameCardsIndexes[i] / 4 > f4.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = this.reservedGameCardsIndexes[i] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (this.reservedGameCardsIndexes[i + 1] / 4 > f4.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = this.reservedGameCardsIndexes[i + 1] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (this.reservedGameCardsIndexes[i] / 4 < f4.Max() / 4 && this.reservedGameCardsIndexes[i + 1] / 4 < f4.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f4.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f4.Length == 4)//different cards in hand
                {
                    if (this.reservedGameCardsIndexes[i] % 4 != this.reservedGameCardsIndexes[i + 1] % 4 &&
                        this.reservedGameCardsIndexes[i] % 4 == f4[0] % 4)
                    {
                        if (this.reservedGameCardsIndexes[i] / 4 > f4.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = this.reservedGameCardsIndexes[i] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f4.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }

                    if (this.reservedGameCardsIndexes[i + 1] % 4 != this.reservedGameCardsIndexes[i] % 4 && 
                        this.reservedGameCardsIndexes[i + 1] % 4 == f4[0] % 4)
                    {
                        if (this.reservedGameCardsIndexes[i + 1] / 4 > f4.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = this.reservedGameCardsIndexes[i + 1] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f4.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f4.Length == 5)
                {
                    if (this.reservedGameCardsIndexes[i] % 4 == f4[0] % 4 && this.reservedGameCardsIndexes[i] / 4 > f4.Min() / 4)
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = this.reservedGameCardsIndexes[i] + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                        this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (this.reservedGameCardsIndexes[i + 1] % 4 == f4[0] % 4 && this.reservedGameCardsIndexes[i + 1] / 4 > f4.Min() / 4)
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = this.reservedGameCardsIndexes[i + 1] + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                        this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (this.reservedGameCardsIndexes[i] / 4 < f4.Min() / 4 && this.reservedGameCardsIndexes[i + 1] / 4 < f4.Min())
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = f4.Max() + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                        this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }
                
                if (f1.Length > 0)//ace
                {
                    if (this.reservedGameCardsIndexes[i] / 4 == 0 && this.reservedGameCardsIndexes[i] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        currentPlayer.Type = 5.5;
                        currentPlayer.Power = 13 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5.5 });
                        this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (this.reservedGameCardsIndexes[i + 1] / 4 == 0 && this.reservedGameCardsIndexes[i + 1] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        currentPlayer.Type = 5.5;
                        currentPlayer.Power = 13 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5.5 });
                        this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (f2.Length > 0)
                {
                    if (this.reservedGameCardsIndexes[i] / 4 == 0 && this.reservedGameCardsIndexes[i] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        currentPlayer.Type = 5.5;
                        currentPlayer.Power = 13 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5.5 });
                        this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (this.reservedGameCardsIndexes[i + 1] / 4 == 0 && this.reservedGameCardsIndexes[i + 1] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        currentPlayer.Type = 5.5;
                        currentPlayer.Power = 13 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5.5 });
                        this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (f3.Length > 0)
                {
                    if (this.reservedGameCardsIndexes[i] / 4 == 0 && this.reservedGameCardsIndexes[i] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        currentPlayer.Type = 5.5;
                        currentPlayer.Power = 13 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5.5 });
                        this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (this.reservedGameCardsIndexes[i + 1] / 4 == 0 && this.reservedGameCardsIndexes[i + 1] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        currentPlayer.Type = 5.5;
                        currentPlayer.Power = 13 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5.5 });
                        this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (f4.Length > 0)
                {
                    if (this.reservedGameCardsIndexes[i] / 4 == 0 && this.reservedGameCardsIndexes[i] % 4 == f4[0] % 4 && vf && f4.Length > 0)
                    {
                        currentPlayer.Type = 5.5;
                        currentPlayer.Power = 13 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5.5 });
                        this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (this.reservedGameCardsIndexes[i + 1] / 4 == 0 && this.reservedGameCardsIndexes[i + 1] % 4 == f4[0] % 4 && vf)
                    {
                        currentPlayer.Type = 5.5;
                        currentPlayer.Power = 13 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5.5 });
                        this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        private void RStraight(ICharacter currentPlayer, int[] straight)
        {
            if (currentPlayer.Type >= -1)
            {
                var op = straight.Select(o => o / 4).Distinct().ToArray();
                for (int j = 0; j < op.Length - 4; j++)
                {
                    if (op[j] + 4 == op[j + 4])
                    {
                        if (op.Max() - 4 == op[j])
                        {
                            currentPlayer.Type = 4;
                            currentPlayer.Power = op.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 4 });
                            this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        }
                        else
                        {
                            currentPlayer.Type = 4;
                            currentPlayer.Power = op[j + 4] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 4 });
                            this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        }
                    }

                    if (op[j] == 0 && op[j + 1] == 9 && op[j + 2] == 10 && op[j + 3] == 11 && op[j + 4] == 12)
                    {
                        currentPlayer.Type = 4;
                        currentPlayer.Power = 13 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 4 });
                        this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        private void RThreeOfAKind(ICharacter currentPlayer, int[] straight)
        {
            if (currentPlayer.Type >= -1)
            {
                for (int j = 0; j <= 12; j++)
                {
                    var fh = straight.Where(o => o / 4 == j).ToArray();
                    if (fh.Length == 3)
                    {
                        if (fh.Max() / 4 == 0)
                        {
                            currentPlayer.Type = 3;
                            currentPlayer.Power = 13 * 3 + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 3 });
                            this.sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                        else
                        {
                            currentPlayer.Type = 3;
                            currentPlayer.Power = fh[0] / 4 + fh[1] / 4 + fh[2] / 4 + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 3 });
                            this.sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                    }
                }
            }
        }

        private void RTwoPair(ICharacter currentPlayer) //ref double current, ref double power
        {
            if (currentPlayer.Type >= -1)
            {
                bool msgbox = false;
                for (int tc = 16; tc >= 12; tc--)
                {
                    int max = tc - 12;
                    if (this.reservedGameCardsIndexes[i] / 4 != this.reservedGameCardsIndexes[i + 1] / 4)
                    {
                        for (int k = 1; k <= max; k++)
                        {
                            if (tc - k < 12)
                            {
                                max--;
                            }

                            if (tc - k >= 12)
                            {
                                if (this.reservedGameCardsIndexes[i] / 4 == this.reservedGameCardsIndexes[tc] / 4 && 
                                    this.reservedGameCardsIndexes[i + 1] / 4 == this.reservedGameCardsIndexes[tc - k] / 4 || 
                                    this.reservedGameCardsIndexes[i + 1] / 4 == this.reservedGameCardsIndexes[tc] / 4 && 
                                    this.reservedGameCardsIndexes[i] / 4 == this.reservedGameCardsIndexes[tc - k] / 4)
                                {
                                    if (!msgbox)
                                    {
                                        if (this.reservedGameCardsIndexes[i] / 4 == 0)
                                        {
                                            currentPlayer.Type = 2;
                                            currentPlayer.Power = 13 * 4 + (this.reservedGameCardsIndexes[i + 1] / 4) * 2 + currentPlayer.Type * 100;
                                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 2 });
                                            this.sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current)
                                                .ThenByDescending(op => op.Power).First();
                                        }

                                        if (this.reservedGameCardsIndexes[i + 1] / 4 == 0)
                                        {
                                            currentPlayer.Type = 2;
                                            currentPlayer.Power = 13 * 4 + (this.reservedGameCardsIndexes[i] / 4) * 2 + currentPlayer.Type * 100;
                                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 2 });
                                            this.sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current)
                                                .ThenByDescending(op => op.Power).First();
                                        }

                                        if (this.reservedGameCardsIndexes[i + 1] / 4 != 0 && this.reservedGameCardsIndexes[i] / 4 != 0)
                                        {
                                            currentPlayer.Type = 2;
                                            currentPlayer.Power = (this.reservedGameCardsIndexes[i] / 4) * 2 + 
                                                (this.reservedGameCardsIndexes[i + 1] / 4) * 2 + currentPlayer.Type * 100;
                                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 2 });
                                            this.sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current)
                                                .ThenByDescending(op => op.Power).First();
                                        }
                                    }

                                    msgbox = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void RPairTwoPair(ICharacter currentPlayer) //ref double current, ref double power
        {
            if (currentPlayer.Type >= -1)
            {
                bool msgbox = false;
                bool msgbox1 = false;
                for (int tc = 16; tc >= 12; tc--)
                {
                    int max = tc - 12;
                    for (int k = 1; k <= max; k++)
                    {
                        if (tc - k < 12)
                        {
                            max--;
                        }

                        if (tc - k >= 12)
                        {
                            if (this.reservedGameCardsIndexes[tc] / 4 == this.reservedGameCardsIndexes[tc - k] / 4)
                            {
                                if (this.reservedGameCardsIndexes[tc] / 4 != this.reservedGameCardsIndexes[i] / 4 &&
                                    this.reservedGameCardsIndexes[tc] / 4 != this.reservedGameCardsIndexes[i + 1] / 4 &&
                                    currentPlayer.Type == 1)
                                {
                                    if (!msgbox)
                                    {
                                        if (this.reservedGameCardsIndexes[i + 1] / 4 == 0)
                                        {
                                            currentPlayer.Type = 2;
                                            currentPlayer.Power = (this.reservedGameCardsIndexes[i] / 4) * 2 + 13 * 4 + currentPlayer.Type * 100;
                                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 2 });
                                            this.sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current)
                                                .ThenByDescending(op => op.Power).First();
                                        }

                                        if (this.reservedGameCardsIndexes[i] / 4 == 0)
                                        {
                                            currentPlayer.Type = 2;
                                            currentPlayer.Power = (this.reservedGameCardsIndexes[i + 1] / 4) * 2 + 13 * 4 + currentPlayer.Type * 100;
                                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 2 });
                                            this.sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current)
                                                .ThenByDescending(op => op.Power).First();
                                        }

                                        if (this.reservedGameCardsIndexes[i + 1] / 4 != 0)
                                        {
                                            currentPlayer.Type = 2;
                                            currentPlayer.Power = (this.reservedGameCardsIndexes[tc] / 4) * 2 +
                                                (this.reservedGameCardsIndexes[i + 1] / 4) * 2 + currentPlayer.Type * 100;
                                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 2 });
                                            this.sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current)
                                                .ThenByDescending(op => op.Power).First();
                                        }

                                        if (this.reservedGameCardsIndexes[i] / 4 != 0)
                                        {
                                            currentPlayer.Type = 2;
                                            currentPlayer.Power = (this.reservedGameCardsIndexes[tc] / 4) * 2 + 
                                                (this.reservedGameCardsIndexes[i] / 4) * 2 + currentPlayer.Type * 100;
                                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 2 });
                                            this.sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current)
                                                .ThenByDescending(op => op.Power).First();
                                        }
                                    }

                                    msgbox = true;
                                }

                                if (currentPlayer.Type == -1)
                                {
                                    if (!msgbox1)
                                    {
                                        if (this.reservedGameCardsIndexes[i] / 4 > this.reservedGameCardsIndexes[i + 1] / 4)
                                        {
                                            if (this.reservedGameCardsIndexes[tc] / 4 == 0)
                                            {
                                                currentPlayer.Type = 0;
                                                currentPlayer.Power = 13 + this.reservedGameCardsIndexes[i] / 4 + currentPlayer.Type * 100;
                                                this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 1 });
                                                this.sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current)
                                                    .ThenByDescending(op => op.Power).First();
                                            }
                                            else
                                            {
                                                currentPlayer.Type = 0;
                                                currentPlayer.Power = this.reservedGameCardsIndexes[tc] / 4 + 
                                                    this.reservedGameCardsIndexes[i] / 4 + currentPlayer.Type * 100;
                                                this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 1 });
                                                this.sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current)
                                                    .ThenByDescending(op => op.Power).First();
                                            }
                                        }
                                        else
                                        {
                                            if (this.reservedGameCardsIndexes[tc] / 4 == 0)
                                            {
                                                currentPlayer.Type = 0;
                                                currentPlayer.Power = 13 + this.reservedGameCardsIndexes[i + 1] + currentPlayer.Type * 100;
                                                this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 1 });
                                                this.sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current)
                                                    .ThenByDescending(op => op.Power).First();
                                            }
                                            else
                                            {
                                                currentPlayer.Type = 0;
                                                currentPlayer.Power = this.reservedGameCardsIndexes[tc] / 4 +
                                                    this.reservedGameCardsIndexes[i + 1] / 4 + currentPlayer.Type * 100;
                                                this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 1 });
                                                this.sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current)
                                                    .ThenByDescending(op => op.Power).First();
                                            }
                                        }
                                    }

                                    msgbox1 = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void RPairFromHand(ICharacter currentPlayer) //ref double current, ref double power
        {
            if (currentPlayer.Type >= -1)
            {
                bool msgbox = false;
                if (this.reservedGameCardsIndexes[i] / 4 == this.reservedGameCardsIndexes[i + 1] / 4)
                {
                    if (!msgbox)
                    {
                        if (this.reservedGameCardsIndexes[i] / 4 == 0)
                        {
                            currentPlayer.Type = 1;
                            currentPlayer.Power = 13 * 4 + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 1 });
                            this.sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                        else
                        {
                            currentPlayer.Type = 1;
                            currentPlayer.Power = (this.reservedGameCardsIndexes[i + 1] / 4) * 4 + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 1 });
                            this.sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                    }

                    msgbox = true;
                }

                for (int tc = 16; tc >= 12; tc--)
                {
                    if (this.reservedGameCardsIndexes[i + 1] / 4 == this.reservedGameCardsIndexes[tc] / 4)
                    {
                        if (!msgbox)
                        {
                            if (this.reservedGameCardsIndexes[i + 1] / 4 == 0)
                            {
                                currentPlayer.Type = 1;
                                currentPlayer.Power = 13 * 4 + this.reservedGameCardsIndexes[i] / 4 + currentPlayer.Type * 100;
                                this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 1 });
                                this.sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                            else
                            {
                                currentPlayer.Type = 1;
                                currentPlayer.Power = (this.reservedGameCardsIndexes[i + 1] / 4) * 4 + 
                                    this.reservedGameCardsIndexes[i] / 4 + currentPlayer.Type * 100;
                                this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 1 });
                                this.sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                        }

                        msgbox = true;
                    }

                    if (this.reservedGameCardsIndexes[i] / 4 == this.reservedGameCardsIndexes[tc] / 4)
                    {
                        if (!msgbox)
                        {
                            if (this.reservedGameCardsIndexes[i] / 4 == 0)
                            {
                                currentPlayer.Type = 1;
                                currentPlayer.Power = 13 * 4 + this.reservedGameCardsIndexes[i + 1] / 4 + currentPlayer.Type * 100;
                                this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 1 });
                                this.sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                            else
                            {
                                currentPlayer.Type = 1;
                                currentPlayer.Power = (this.reservedGameCardsIndexes[tc] / 4) * 4 +
                                    this.reservedGameCardsIndexes[i + 1] / 4 + currentPlayer.Type * 100;
                                this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 1 });
                                this.sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                        }

                        msgbox = true;
                    }
                }
            }
        }

        private void RHighCard(ICharacter currentPlayer)
        {
            if (currentPlayer.Type == -1)
            {
                if (this.reservedGameCardsIndexes[i] / 4 > this.reservedGameCardsIndexes[i + 1] / 4)
                {
                    currentPlayer.Type = -1;
                    currentPlayer.Power = this.reservedGameCardsIndexes[i] / 4;
                    this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = -1 });
                    this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
                else
                {
                    currentPlayer.Type = -1;
                    currentPlayer.Power = this.reservedGameCardsIndexes[i + 1] / 4;
                    this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = -1 });
                    this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }

                if (this.reservedGameCardsIndexes[i] / 4 == 0 || this.reservedGameCardsIndexes[i + 1] / 4 == 0)
                {
                    currentPlayer.Type = -1;
                    currentPlayer.Power = 13;
                    this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = -1 });
                    this.sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
            }
        }

        public void Winner(ICharacter currentPlayer, string lastly)
        {
            if (lastly == " ")
            {
                lastly = "Bot 5";
            }

            for (int j = 0; j <= 16; j++)
            {
                //await Task.Delay(5);
                if (this.cardsPictureBoxArray[j].Visible)
                {
                    this.cardsPictureBoxArray[j].Image = this.gameCardsAsImages[j];
                } 
            }

            if (currentPlayer.Type == this.sorted.Current)
            {
                if (currentPlayer.Power == this.sorted.Power)
                {
                    this.winners++;
                    this.gameDatabase.CheckWinners.Add(currentPlayer.Name);
                    if (currentPlayer.Type == -1)
                    {
                        MessageBox.Show(currentPlayer.Name + " High Card ");
                    }

                    if (currentPlayer.Type == 1 || currentPlayer.Type == 0)
                    {
                        MessageBox.Show(currentPlayer.Name + " Pair ");
                    }

                    if (currentPlayer.Type == 2)
                    {
                        MessageBox.Show(currentPlayer.Name + " Two Pair ");
                    }

                    if (currentPlayer.Type == 3)
                    {
                        MessageBox.Show(currentPlayer.Name + " Three of a Kind ");
                    }

                    if (currentPlayer.Type == 4)
                    {
                        MessageBox.Show(currentPlayer.Name + " Straight ");
                    }

                    if (currentPlayer.Type == 5 || currentPlayer.Type == 5.5)
                    {
                        MessageBox.Show(currentPlayer.Name + " Flush ");
                    }

                    if (currentPlayer.Type == 6)
                    {
                        MessageBox.Show(currentPlayer.Name + " Full House ");
                    }

                    if (currentPlayer.Type == 7)
                    {
                        MessageBox.Show(currentPlayer.Name + " Four of a Kind ");
                    }

                    if (currentPlayer.Type == 8)
                    {
                        MessageBox.Show(currentPlayer.Name + " Straight Flush ");
                    }

                    if (currentPlayer.Type == 9)
                    {
                        MessageBox.Show(currentPlayer.Name + " Royal Flush ! ");
                    }
                }
            }

            if (currentPlayer.Name == lastly)//lastfixed
            {
                if (this.winners > 1)
                {
                    if (this.gameDatabase.CheckWinners.Contains(this.player.Name))
                    {
                        this.player.Chips += int.Parse(this.textBoxPot.Text) / this.winners;
                        this.textBoxPlayerChips.Text = this.player.ToString();
                    }

                    if (this.gameDatabase.CheckWinners.Contains(this.bots[0].Name))
                    {
                        this.bots[0].Chips += int.Parse(this.textBoxPot.Text) / this.winners;
                        this.textBoxBotOneChips.Text = this.bots[0].Chips.ToString();
                    }

                    if (this.gameDatabase.CheckWinners.Contains(this.bots[1].Name))
                    {
                        this.bots[1].Chips += int.Parse(this.textBoxPot.Text) / this.winners;
                        this.textBoxBotTwoChips.Text = this.bots[1].Chips.ToString();
                    }

                    if (this.gameDatabase.CheckWinners.Contains(this.bots[2].Name))
                    {
                        this.bots[2].Chips += int.Parse(this.textBoxPot.Text) / this.winners;
                        this.textBoxBotThreeChips.Text = this.bots[2].Chips.ToString();
                    }

                    if (this.gameDatabase.CheckWinners.Contains(this.bots[3].Name))
                    {
                        this.bots[3].Chips += int.Parse(this.textBoxPot.Text) / this.winners;
                        this.textBoxBotFourChips.Text = this.bots[3].Chips.ToString();
                    }

                    if (this.gameDatabase.CheckWinners.Contains(this.bots[4].Name))
                    {
                        this.bots[4].Chips += int.Parse(this.textBoxPot.Text) / this.winners;
                        this.textBoxBotFiveChips.Text = this.bots[4].Chips.ToString();
                    }
                }

                if (this.winners == 1)
                {
                    if (this.gameDatabase.CheckWinners.Contains(this.player.Name))
                    {
                        this.player.Chips += int.Parse(this.textBoxPot.Text);
                    }

                    if (this.gameDatabase.CheckWinners.Contains(this.bots[0].Name))
                    {
                        this.bots[0].Chips += int.Parse(this.textBoxPot.Text);
                    }

                    if (this.gameDatabase.CheckWinners.Contains(this.bots[1].Name))
                    {
                        this.bots[1].Chips += int.Parse(this.textBoxPot.Text);
                    }

                    if (this.gameDatabase.CheckWinners.Contains(this.bots[2].Name))
                    {
                        this.bots[2].Chips += int.Parse(this.textBoxPot.Text);
                    }

                    if (this.gameDatabase.CheckWinners.Contains(this.bots[3].Name))
                    {
                        this.bots[3].Chips += int.Parse(this.textBoxPot.Text);
                    }

                    if (this.gameDatabase.CheckWinners.Contains(this.bots[4].Name))
                    {
                        this.bots[4].Chips += int.Parse(this.textBoxPot.Text);
                    }
                }
            }
        }

        async Task CheckRaise(int currentTurn, int raiseTurn)
        {
            if (this.raising)
            {
                this.turnCount = 0;
                this.raising = false;
                this.raisedTurn = currentTurn;
                this.changed = true;
            }
            else
            {
                if (this.turnCount >= (this.maxLeft - 1) || !this.changed && this.turnCount == this.maxLeft)
                {
                    if (currentTurn == this.raisedTurn - 1 || !this.changed && this.turnCount == this.maxLeft ||
                        this.raisedTurn == 0 && currentTurn == 5)
                    {
                        this.changed = false;
                        this.turnCount = 0;
                        this.raise = 0;
                        this.neededChipsToCall = 0;
                        this.raisedTurn = 123;
                        this.rounds++;

                        if (!this.player.OutOfChips)
                        {
                            this.playerStatus.Text = string.Empty;
                        }

                        for (int currantBot = 0; currantBot < this.botStatusLabels.Length; currantBot++)
                        {
                            if (!this.bots[currantBot].OutOfChips)
                            {
                                this.botStatusLabels[currantBot].Text = string.Empty;
                            }
                        }
                    }
                }
            }

            if (this.rounds == this.flop)
            {
                for (int j = 12; j <= 14; j++)
                {
                    if (this.cardsPictureBoxArray[j].Image != this.gameCardsAsImages[j])
                    {
                        this.cardsPictureBoxArray[j].Image = this.gameCardsAsImages[j];
                        this.player.Call = 0; this.player.Raise = 0;
                        this.bots[0].Call = 0; this.bots[0].Raise = 0;
                        this.bots[1].Call = 0; this.bots[1].Raise = 0;
                        this.bots[2].Call = 0; this.bots[2].Raise = 0;
                        this.bots[3].Call = 0; this.bots[3].Raise = 0;
                        this.bots[4].Call = 0; this.bots[4].Raise = 0;
                    }
                }
            }

            if (this.rounds == this.turn)
            {
                for (int j = 14; j <= 15; j++)
                {
                    if (this.cardsPictureBoxArray[j].Image != this.gameCardsAsImages[j])
                    {
                        this.cardsPictureBoxArray[j].Image = this.gameCardsAsImages[j];
                        this.player.Call = 0; this.player.Raise = 0;
                        this.bots[0].Call = 0; this.bots[0].Raise = 0;
                        this.bots[1].Call = 0; this.bots[1].Raise = 0;
                        this.bots[2].Call = 0; this.bots[2].Raise = 0;
                        this.bots[3].Call = 0; this.bots[3].Raise = 0;
                        this.bots[4].Call = 0; this.bots[4].Raise = 0;
                    }
                }
            }

            if (this.rounds == this.river)
            {
                for (int j = 15; j <= 16; j++)
                {
                    if (this.cardsPictureBoxArray[j].Image != this.gameCardsAsImages[j])
                    {
                        this.cardsPictureBoxArray[j].Image = this.gameCardsAsImages[j];
                        this.player.Call = 0; this.player.Raise = 0;
                        this.bots[0].Call = 0; this.bots[0].Raise = 0;
                        this.bots[1].Call = 0; this.bots[1].Raise = 0;
                        this.bots[2].Call = 0; this.bots[2].Raise = 0;
                        this.bots[3].Call = 0; this.bots[3].Raise = 0;
                        this.bots[4].Call = 0; this.bots[4].Raise = 0;
                    }
                }
            }

            if (this.rounds == this.end && this.maxLeft == 6)
            {
                string fixedLast = "qwerty";
                if (!this.playerStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Player";
                    this.Rules(0, 1, this.player);
                }

                if (!this.botOneStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 1";
                    this.Rules(2, 3, this.bots[0]);
                }

                if (!this.botTwoStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 2";
                    this.Rules(4, 5, this.bots[1]);
                }

                if (!this.botThreeStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 3";
                    this.Rules(6, 7, this.bots[2]);
                }

                if (!this.botFourStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 4";
                    this.Rules(8, 9, this.bots[3]);
                }

                if (!this.botFiveStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 5";
                    this.Rules(10, 11, this.bots[4]);
                }

                this.Winner(this.player, fixedLast);
                for (int currentBot = 0; currentBot < this.bots.Count; currentBot++)
                {
                    this.Winner(this.bots[currentBot], fixedLast);
                }

                this.restart = true;
                this.player.CanMakeTurn = true;
                this.player.OutOfChips = false;
                for (int currentBot = 0; currentBot < this.bots.Count; currentBot++)
                {
                    this.bots[currentBot].OutOfChips = false;
                }

                if (this.player.Chips <= 0)
                {
                    AddChips chipAdder = new AddChips();
                    chipAdder.ShowDialog();
                    if (chipAdder.A != 0)
                    {
                        this.player.Chips = chipAdder.A;
                        for (int currentBot = 0; currentBot < this.bots.Count; currentBot++)
                        {
                            this.bots[currentBot].Chips += chipAdder.A;
                        }

                        this.player.OutOfChips = false;
                        this.player.CanMakeTurn = true;
                        this.buttonRaise.Enabled = true;
                        this.buttonFold.Enabled = true;
                        this.buttonCheck.Enabled = true;
                        this.buttonRaise.Text = "Raise";
                    }
                }

                this.player.Panel.Visible = false;
                for (int currentBot = 0; currentBot < this.bots.Count; currentBot++)
                {
                    this.bots[currentBot].Panel.Visible = false;
                }

                this.player.Call = 0;
                this.player.Raise = 0;
                for (int currentBot = 0; currentBot < this.bots.Count; currentBot++)
                {
                    this.bots[currentBot].Call = 0;
                    this.bots[currentBot].Raise = 0;
                }

                this.last = 0;
                this.neededChipsToCall = this.bigBlindValue;
                this.raise = 0;
                this.gameDatabase.PlayersGameStatus.Clear();
                this.rounds = 0;
                this.player.Power = 0;
                this.player.Type = -1;
                this.type = 0;
                for (int currentBot = 0; currentBot < this.bots.Count; currentBot++)
                {
                    this.bots[currentBot].Power = 0;
                    this.bots[currentBot].Type = -1;
                }

                this.gameDatabase.Chips.Clear();
                this.gameDatabase.CheckWinners.Clear();
                this.winners = 0;
                this.gameDatabase.Win.Clear();
                this.sorted.Current = 0;
                this.sorted.Power = 0;
                for (int currentCard = 0; currentCard < this.cardsPictureBoxArray.Length; currentCard++)
                {
                    this.cardsPictureBoxArray[currentCard].Image = null;
                    this.cardsPictureBoxArray[currentCard].Invalidate();
                    this.cardsPictureBoxArray[currentCard].Visible = false;
                }

                this.textBoxPot.Text = "0";
                this.playerStatus.Text = string.Empty;
                await Shuffle();
                await Turns();
            }
        }

        void FixCall(Label status, ICharacter currentPlayer, int options) //ref int cCall, ref int cRaise
        {
            if (this.rounds != 4)
            {
                if (options == 1)
                {
                    if (status.Text.Contains("Raise"))
                    {
                        var changeRaise = status.Text.Substring(6);
                        currentPlayer.Raise = int.Parse(changeRaise);
                    }

                    if (status.Text.Contains("Call"))
                    {
                        var changeCall = status.Text.Substring(5);
                        currentPlayer.Call = int.Parse(changeCall);
                    }

                    if (status.Text.Contains("Check"))
                    {
                        currentPlayer.Raise = 0;
                        currentPlayer.Call = 0;
                    }
                }

                if (options == 2)
                {
                    if (currentPlayer.Raise != this.raise && currentPlayer.Raise <= this.raise)
                    {
                        this.neededChipsToCall = Convert.ToInt32(this.raise) - currentPlayer.Raise;
                    }

                    if (currentPlayer.Call != this.neededChipsToCall || currentPlayer.Call <= this.neededChipsToCall)
                    {
                        this.neededChipsToCall = this.neededChipsToCall - currentPlayer.Call;
                    }

                    if ((currentPlayer.Raise) == this.raise && (this.raise > 0))
                    {
                        this.neededChipsToCall = 0;
                        this.buttonCall.Enabled = false;
                        this.buttonCall.Text = "Callisfuckedup";
                    }
                }
            }
        }

        async Task AllIn()
        {
            #region Allin
            if (this.player.Chips <= 0 && !this.chipsAreAdded)
            {
                if (this.playerStatus.Text.Contains("Raise"))
                {
                    this.gameDatabase.Chips.Add(this.player.Chips);
                    this.chipsAreAdded = true;
                }

                if (this.playerStatus.Text.Contains("Call"))
                {
                    this.gameDatabase.Chips.Add(this.player.Chips);
                    this.chipsAreAdded = true;
                }
            }

            this.chipsAreAdded = false;
            for (int currentBot = 0; currentBot < this.bots.Count; currentBot++)
            {
                if (this.bots[currentBot].Chips <= 0 && !this.bots[currentBot].OutOfChips)
                {
                    if (!this.chipsAreAdded)
                    {
                        this.gameDatabase.Chips.Add(this.bots[currentBot].Chips);
                        this.chipsAreAdded = true;
                    }
                    this.chipsAreAdded = false;
                }
            }

            this.chipsAreAdded = true;

            if (this.gameDatabase.Chips.ToArray().Length == this.maxLeft)
            {
                await Finish(2);
            }
            else
            {
                this.gameDatabase.Chips.Clear();
            }
            #endregion

            var abc = this.gameDatabase.PlayersGameStatus.Count(x => x == false);

            #region LastManStanding
            if (abc == 1)
            {
                int index = this.gameDatabase.PlayersGameStatus.IndexOf(false);
                if (index == 0)
                {
                    this.player.Chips += int.Parse(this.textBoxPot.Text);
                    this.textBoxPlayerChips.Text = this.player.Chips.ToString();
                    this.player.Panel.Visible = true;
                    MessageBox.Show("Player Wins");
                }
                else if ((0 < index) && (index <= DefaultNumberOfBots))
                {
                    int currentBot = index - 1;
                    this.bots[currentBot].Chips += int.Parse(this.textBoxPot.Text);
                    this.textBoxPlayerChips.Text = this.bots[currentBot].Chips.ToString();
                    this.bots[currentBot].Panel.Visible = true;
                    string msg = this.bots[currentBot].Name + " Wins";
                    MessageBox.Show(msg);
                }
                
                for (int j = 0; j <= 16; j++)
                {
                    this.cardsPictureBoxArray[j].Visible = false;
                }

                await Finish(1);
            }

            this.chipsAreAdded = false;
            #endregion

            #region FiveOrLessLeft
            if (abc < 6 && abc > 1 && this.rounds >= this.end)
            {
                await this.Finish(2);
            }
            #endregion
        }

        async Task Finish(int n)
        {
            if (n == 2)
            {
                this.FixWinners();
            }

            this.player.Panel.Visible = false;
            this.player.Power = 0;
            this.player.Type = -1;
            this.player.CanMakeTurn = true;
            this.player.OutOfChips = false;
            this.player.Folded = false;
            this.player.Call = 0;
            this.player.Raise = 0;

            for (int currentBot = 0; currentBot < this.bots.Count; currentBot++)
            {
                this.bots[currentBot].Panel.Visible = false;
                this.bots[currentBot].Power = 0;
                this.bots[currentBot].Type = -1;
                this.bots[currentBot].CanMakeTurn = false;
                this.bots[currentBot].OutOfChips = false;
                this.bots[currentBot].Folded = false;
                this.bots[currentBot].Call = 0;
                this.bots[currentBot].Raise = 0;
            }

            this.neededChipsToCall = this.bigBlindValue;
            this.raise = 0;
            this.foldedPlayers = 5;
            this.type = 0;
            this.rounds = 0;
            this.restart = false;
            this.raising = false;
            this.height = 0;
            this.width = 0;
            this.winners = 0;
            this.flop = 1;
            this.turn = 2;
            this.river = 3;
            this.end = 4;
            this.maxLeft = 6;
            this.last = 123;
            this.raisedTurn = 1;
            this.gameDatabase.PlayersGameStatus.Clear();
            this.gameDatabase.CheckWinners.Clear();
            this.gameDatabase.Chips.Clear();
            this.gameDatabase.Win.Clear();
            this.sorted.Current = 0;
            this.sorted.Power = 0;
            this.textBoxPot.Text = "0";
            this.secondsLeft = 60;
            this.up = 10000000;
            this.turnCount = 0;
            this.playerStatus.Text = string.Empty;
            this.botOneStatus.Text = string.Empty;
            this.botTwoStatus.Text = string.Empty;
            this.botThreeStatus.Text = string.Empty;
            this.botFourStatus.Text = string.Empty;
            this.botFiveStatus.Text = string.Empty;

            if (this.player.Chips <= 0)
            {
                AddChips f2 = new AddChips();
                f2.ShowDialog();
                if (f2.A != 0)
                {
                    this.player.Chips = f2.A;
                    for (int currentBot = 0; currentBot < this.bots.Count; currentBot++)
                    {
                        this.bots[currentBot].Chips += f2.A;
                    }

                    this.player.OutOfChips = false;
                    this.player.CanMakeTurn = true;
                    this.buttonRaise.Enabled = true;
                    this.buttonFold.Enabled = true;
                    this.buttonCheck.Enabled = true;
                    this.buttonRaise.Text = "Raise";
                }
            }

            this.cardsImageLocation = Directory.GetFiles("..\\..\\Resources\\Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
            for (int os = 0; os < DefaultCardsInGame; os++)
            {
                this.cardsPictureBoxArray[os].Image = null;
                this.cardsPictureBoxArray[os].Invalidate();
                this.cardsPictureBoxArray[os].Visible = false;
            }

            await this.Shuffle();
        }

        public void FixWinners()
        {
            this.gameDatabase.Win.Clear();
            this.sorted.Current = 0;
            this.sorted.Power = 0;
            string fixedLast = "qwerty";
            if (!this.playerStatus.Text.Contains("Fold"))
            {
                fixedLast = "Player";
                this.Rules(0, 1, this.player);
            }
            
            for (int currentBotNumber = 0; currentBotNumber < this.bots.Count; currentBotNumber++)
            {
                if (!this.botStatusLabels[currentBotNumber].Text.Contains("Fold"))
                {
                    fixedLast = this.bots[currentBotNumber].Name;
                    int firstNumber = this.bots[currentBotNumber].StartCard;
                    int secondNumber = firstNumber + 1;
                    this.Rules(firstNumber, secondNumber, this.bots[currentBotNumber]);
                }
            }
            
            this.Winner(this.player, fixedLast);
            for (int currentBotNumber = 0; currentBotNumber < this.bots.Count; currentBotNumber++)
            {
                this.Winner(this.bots[currentBotNumber], fixedLast);
            }
        }
        
        private void AI(int card1, int card2, Label sStatus, int name, ICharacter player)
        {
            if (!player.OutOfChips)
            {
                if (player.Type == -1)
                {
                    this.winningHandType.HighCard(player, sStatus, ref this.neededChipsToCall, this.textBoxPot, ref this.raise, ref this.raising);
                }

                if (player.Type == 0)
                {
                    this.winningHandType.PairTable(player, sStatus, ref this.neededChipsToCall, this.textBoxPot, ref this.raise, ref this.raising);
                }

                if (player.Type == 1)
                {
                    this.winningHandType.PairHand(player, sStatus, ref this.neededChipsToCall, this.textBoxPot, ref this.raise, ref this.raising, ref this.rounds);
                }

                if (player.Type == 2)
                {
                    this.winningHandType.TwoPair(player, sStatus, ref this.neededChipsToCall, this.textBoxPot, ref this.raise, ref this.raising, ref this.rounds);
                }

                if (player.Type == 3)
                {
                    this.winningHandType.ThreeOfAKind(player, sStatus, name, ref this.neededChipsToCall, this.textBoxPot, ref this.raise, ref this.raising, ref this.rounds);
                }

                if (player.Type == 4)
                {
                    this.winningHandType.Straight(player, sStatus, name, ref this.neededChipsToCall, this.textBoxPot, ref this.raise, ref this.raising, ref this.rounds);
                }

                if (player.Type == 5 || player.Type == 5.5)
                {
                    this.winningHandType.Flush(player, sStatus, name, ref this.neededChipsToCall, this.textBoxPot, ref this.raise, ref this.raising, ref this.rounds);
                }

                if (player.Type == 6)
                {
                    this.winningHandType.FullHouse(player, sStatus, name, ref this.neededChipsToCall, this.textBoxPot, ref this.raise, ref this.raising, ref this.rounds);
                }

                if (player.Type == 7)
                {
                    this.winningHandType.FourOfAKind(player, sStatus, name, ref this.neededChipsToCall, this.textBoxPot, ref this.raise, ref this.raising, ref this.rounds);
                }

                if (player.Type == 8 || player.Type == 9)
                {
                    this.winningHandType.StraightFlush(player, sStatus, name, ref this.neededChipsToCall, this.textBoxPot, ref this.raise, ref this.raising, ref this.rounds);
                }
            }

            if (player.OutOfChips)
            {
                this.cardsPictureBoxArray[card1].Visible = false;
                this.cardsPictureBoxArray[card2].Visible = false;
            }
        }

        #region UI
        private async void TimerTick(object sender, object e)
        {
            if (this.progressBarTimer.Value <= 0)
            {
                this.player.OutOfChips = true;
                await this.Turns();
            }

            if (this.secondsLeft > 0)
            {
                this.secondsLeft--;
                this.progressBarTimer.Value = (this.secondsLeft / 6) * 100;
            }
        }

        private void UpdateTick(object sender, object e)
        {
            if (this.player.Chips <= 0)
            {
                this.textBoxPlayerChips.Text = "Chips: 0";
            }

            if (this.bots[0].Chips <= 0)
            {
                this.textBoxBotOneChips.Text = "Chips: 0";
            }

            if (this.bots[1].Chips <= 0)
            {
                this.textBoxBotTwoChips.Text = "Chips: 0";
            }

            if (this.bots[2].Chips <= 0)
            {
                this.textBoxBotThreeChips.Text = "Chips: 0";
            }

            if (this.bots[3].Chips <= 0)
            {
                this.textBoxBotFourChips.Text = "Chips: 0";
            }

            if (this.bots[4].Chips <= 0)
            {
                this.textBoxBotFiveChips.Text = "Chips: 0";
            }

            this.textBoxPlayerChips.Text = "Chips : " + this.player.Chips.ToString();
            this.textBoxBotOneChips.Text = "Chips : " + this.bots[0].Chips.ToString();
            this.textBoxBotTwoChips.Text = "Chips : " + this.bots[1].Chips.ToString();
            this.textBoxBotThreeChips.Text = "Chips : " + this.bots[2].Chips.ToString();
            this.textBoxBotFourChips.Text = "Chips : " + this.bots[3].Chips.ToString();
            this.textBoxBotFiveChips.Text = "Chips : " + this.bots[4].Chips.ToString();

            if (this.player.Chips <= 0)
            {
                this.player.CanMakeTurn = false;
                this.player.OutOfChips = true;
                this.buttonCall.Enabled = false;
                this.buttonRaise.Enabled = false;
                this.buttonFold.Enabled = false;
                this.buttonCheck.Enabled = false;
            }

            if (this.up > 0)
            {
                this.up--;
            }

            if (this.player.Chips >= this.neededChipsToCall)
            {
                this.buttonCall.Text = "Call " + this.neededChipsToCall.ToString();
            }
            else
            {
                this.buttonCall.Text = "All in";
                this.buttonRaise.Enabled = false;
            }

            if (this.neededChipsToCall > 0)
            {
                this.buttonCheck.Enabled = false;
            }

            if (this.neededChipsToCall <= 0)
            {
                this.buttonCheck.Enabled = true;
                this.buttonCall.Text = "Call";
                this.buttonCall.Enabled = false;
            }

            if (this.player.Chips <= 0)
            {
                this.buttonRaise.Enabled = false;
            }

            int parsedValue;

            if (this.textBoxRaise.Text != string.Empty && int.TryParse(this.textBoxRaise.Text, out parsedValue))
            {
                if (this.player.Chips <= int.Parse(this.textBoxRaise.Text))
                {
                    this.buttonRaise.Text = "All in";
                }
                else
                {
                    this.buttonRaise.Text = "Raise";
                }
            }

            if (this.player.Chips < this.neededChipsToCall)
            {
                this.buttonRaise.Enabled = false;
            }
        }

        private async void bFold_Click(object sender, EventArgs e)
        {
            this.playerStatus.Text = "Fold";
            this.player.CanMakeTurn = false;
            this.player.OutOfChips = true;
            await this.Turns();
        }

        private async void bCheck_Click(object sender, EventArgs e)
        {
            if (this.neededChipsToCall <= 0)
            {
                this.player.CanMakeTurn = false;
                this.playerStatus.Text = "Check";
            }
            else
            {
                this.buttonCheck.Enabled = false;
            }

            await this.Turns();
        }

        private async void bCall_Click(object sender, EventArgs e)
        {
            this.Rules(0, 1, this.player);
            if (this.player.Chips >= this.neededChipsToCall)
            {
                this.player.Chips -= this.neededChipsToCall;
                this.textBoxPlayerChips.Text = "Chips: " + this.player.Chips.ToString();
                if (this.textBoxPot.Text != string.Empty)
                {
                    this.textBoxPot.Text = (int.Parse(this.textBoxPot.Text) + this.neededChipsToCall).ToString();
                }
                else
                {
                    this.textBoxPot.Text = this.neededChipsToCall.ToString();
                }

                this.player.CanMakeTurn = false;
                this.playerStatus.Text = "Call " + this.neededChipsToCall;
                this.player.Call = this.neededChipsToCall;
            }
            else if (this.player.Chips <= this.neededChipsToCall && this.neededChipsToCall > 0)
            {
                this.textBoxPot.Text = (int.Parse(this.textBoxPot.Text) + this.player.Chips).ToString();
                this.playerStatus.Text = "All in " + this.player.Chips;
                this.player.Chips = 0;
                this.textBoxPlayerChips.Text = "Chips: " + this.player.Chips.ToString();
                this.player.CanMakeTurn = false;
                this.buttonFold.Enabled = false;
                this.player.Call = this.player.Chips;
            }

            await this.Turns();
        }

        private async void bRaise_Click(object sender, EventArgs e)
        {
            this.Rules(0, 1, this.player);
            int parsedValue;
            if (this.textBoxRaise.Text != string.Empty && int.TryParse(this.textBoxRaise.Text, out parsedValue))
            {
                if (this.player.Chips > this.neededChipsToCall)
                {
                    if (this.raise * 2 > int.Parse(this.textBoxRaise.Text))
                    {
                        this.textBoxRaise.Text = (this.raise * 2).ToString();
                        MessageBox.Show("You must raise atleast twice as the current raise !");

                        return;
                    }
                    else
                    {
                        if (this.player.Chips >= int.Parse(this.textBoxRaise.Text))
                        {
                            this.neededChipsToCall = int.Parse(this.textBoxRaise.Text);
                            this.raise = int.Parse(this.textBoxRaise.Text);
                            this.playerStatus.Text = "Raise " + this.neededChipsToCall.ToString();
                            this.textBoxPot.Text = (int.Parse(this.textBoxPot.Text) + this.neededChipsToCall).ToString();
                            this.buttonCall.Text = "Call";
                            this.player.Chips -= int.Parse(this.textBoxRaise.Text);
                            this.raising = true;
                            this.last = 0;
                            this.player.Raise = Convert.ToInt32(this.raise);
                        }
                        else
                        {
                            this.neededChipsToCall = this.player.Chips;
                            this.raise = this.player.Chips;
                            this.textBoxPot.Text = (int.Parse(this.textBoxPot.Text) + this.player.Chips).ToString();
                            this.playerStatus.Text = "Raise " + this.neededChipsToCall.ToString();
                            this.player.Chips = 0;
                            this.raising = true;
                            this.last = 0;
                            this.player.Raise = Convert.ToInt32(raise);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("This is a number only field");

                return;
            }

            this.player.CanMakeTurn = false;
            await this.Turns();
        }

        private void bAdd_Click(object sender, EventArgs e)
        {
            if (this.textBoxAdd.Text != string.Empty)
            {
                this.player.Chips += int.Parse(this.textBoxAdd.Text);
                for (int currentBotNumber = 0; currentBotNumber < this.bots.Count; currentBotNumber++)
                {
                    this.bots[currentBotNumber].Chips += int.Parse(this.textBoxAdd.Text);
                }
            }

            this.textBoxPlayerChips.Text = "Chips : " + this.player.Chips.ToString();
        }

        private void bOptions_Click(object sender, EventArgs e)
        {
            this.textBoxBigBlind.Text = this.bigBlindValue.ToString();
            this.textBoxSmallBlind.Text = this.smallBlindValue.ToString();
            if (this.textBoxBigBlind.Visible == false)
            {
                this.textBoxBigBlind.Visible = true;
                this.textBoxSmallBlind.Visible = true;
                this.buttonBigBlind.Visible = true;
                this.buttonSmallBlind.Visible = true;
            }
            else
            {
                this.textBoxBigBlind.Visible = false;
                this.textBoxSmallBlind.Visible = false;
                this.buttonBigBlind.Visible = false;
                this.buttonSmallBlind.Visible = false;
            }
        }

        private void bSB_Click(object sender, EventArgs e)
        {
            int parsedValue;
            if (this.textBoxSmallBlind.Text.Contains(",") || this.textBoxSmallBlind.Text.Contains("."))
            {
                string msg = "The Small Blind can be only round number !";
                MessageBox.Show(msg);
                this.textBoxSmallBlind.Text = this.smallBlindValue.ToString();

                return;
            }

            if (!int.TryParse(this.textBoxSmallBlind.Text, out parsedValue))
            {
                string msg = "This is a number only field";
                MessageBox.Show(msg);
                this.textBoxSmallBlind.Text = this.smallBlindValue.ToString();

                return;
            }

            if (int.Parse(this.textBoxSmallBlind.Text) > 100000)
            {
                string msg = "The maximum of the Small Blind is 100 000 $";
                MessageBox.Show(msg);
                this.textBoxSmallBlind.Text = this.smallBlindValue.ToString();
            }

            if (int.Parse(this.textBoxSmallBlind.Text) < 250)
            {
                string msg = "The minimum of the Small Blind is 250 $";
                MessageBox.Show(msg);
            }

            if (int.Parse(this.textBoxSmallBlind.Text) >= 250 && int.Parse(this.textBoxSmallBlind.Text) <= 100000)
            {
                this.smallBlindValue = int.Parse(this.textBoxSmallBlind.Text);
                string msg = "The changes have been saved ! They will become available the next hand you play. ";
                MessageBox.Show(msg);
            }
        }

        private void bBB_Click(object sender, EventArgs e)
        {
            int parsedValue;
            if (this.textBoxBigBlind.Text.Contains(",") || this.textBoxBigBlind.Text.Contains("."))
            {
                string msg = "The Big Blind can be only round number !";
                MessageBox.Show(msg);
                this.textBoxBigBlind.Text = this.bigBlindValue.ToString();

                return;
            }

            if (!int.TryParse(this.textBoxSmallBlind.Text, out parsedValue))
            {
                string msg = "This is a number only field";
                MessageBox.Show(msg);
                this.textBoxSmallBlind.Text = this.bigBlindValue.ToString();

                return;
            }

            if (int.Parse(this.textBoxBigBlind.Text) > 200000)
            {
                string msg = "The maximum of the Big Blind is 200 000";
                MessageBox.Show(msg);
                this.textBoxBigBlind.Text = this.bigBlindValue.ToString();
            }

            if (int.Parse(this.textBoxBigBlind.Text) < 500)
            {
                string msg = "The minimum of the Big Blind is 500 $";
                MessageBox.Show(msg);
            }

            if (int.Parse(this.textBoxBigBlind.Text) >= 500 && int.Parse(this.textBoxBigBlind.Text) <= 200000)
            {
                this.bigBlindValue = int.Parse(this.textBoxBigBlind.Text);
                string msg = "The changes have been saved ! They will become available the next hand you play. ";
                MessageBox.Show(msg);
            }
        }

        private void Layout_Change(object sender, LayoutEventArgs e)
        {
            this.width = this.Width;
            this.height = this.Height;
        }
        #endregion
    }
}
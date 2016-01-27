namespace Poker
{
    using Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Poker.Data;
    using Poker.Models;
    using Poker.Models.Characters;

    public partial class MainWindow : Form
    {
        #region Variables
        public const int DefaultSetOfCards = 52;
        public const int DefaultCardsInGame = 17;

        private ProgressBar progressBar;
        private readonly IPlayer player;
        private readonly IList<IBot> bots;
        private readonly IWinningHandType winningHandType;
        private readonly IDatabase gameDatabase;

        private Type _sorted;
        private string[] cardsImageLocation;
        private int[] reservedGameCardsIndexes;
        private Image[] gameCardsAsImages;
        private PictureBox[] cardsPictureBoxArray;
        private Timer _timer;
        private Timer _updates;
        private Bitmap backImage;

        private int Nm;

        private int neededChipsToCall;
        private int foldedPlayers;
        private double type;
        private int rounds;
        private int raise;
        private bool chipsAreAdded;
        private bool changed;
        private int _height;
        private int _width;
        private int _winners;
        private int _flop;
        private int _turn;
        private int _river;
        private int _end;
        private int _maxLeft;
        private int _last;
        private int _raisedTurn;
        private bool _restart;
        private bool raising;
        private int secondsLeft;
        private int i;
        private int bigBlindValue;
        private int smallBlindValue;
        private int _up;
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
            this._timer = new Timer();
            this._updates = new Timer();
            this.backImage = new Bitmap("..\\..\\Resources\\Assets\\Back\\Back.png");

            this.neededChipsToCall = 500;
            this.foldedPlayers = 5;
            this.turnCount = 0;
            this._winners = 0;
            this._flop = 1;
            this._turn = 2;
            this._river = 3;
            this._end = 4;
            this._maxLeft = 6;
            this._raisedTurn = 1;
            this._last = 123;
            this.secondsLeft = 60;
            this.rounds = 0;
            this.raise = 0;
            this.bigBlindValue = 500;
            this.smallBlindValue = 250;
            this._up = 10000000;
            this.neededChipsToCall = this.bigBlindValue;
            this._restart = false;
            this.raising = false;

            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this._updates.Start();
            this.InitializeComponent();

            this._width = this.Width;
            this._height = this.Height;
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
            this._timer.Interval = (1 * 1 * 1000);
            this._timer.Tick += TimerTick;
            this._updates.Interval = (1 * 1 * 100);
            this._updates.Tick += UpdateTick;
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
            for (i = cardsImageLocation.Length; i > 0; i--)
            {
                int j = r.Next(i);
                var k = cardsImageLocation[j];
                cardsImageLocation[j] = cardsImageLocation[i - 1];
                cardsImageLocation[i - 1] = k;
            }

            for (int cardNumber = 0; cardNumber < 17; cardNumber++)
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
                    this.cardsPictureBoxArray[cardNumber].Anchor = (AnchorStyles.Bottom);
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
                            this.cardsPictureBoxArray[cardNumber].Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
                            this.cardsPictureBoxArray[cardNumber].Image = this.backImage;
                            this.cardsPictureBoxArray[cardNumber].Location = new Point(horizontal, vertical);
                            horizontal += this.cardsPictureBoxArray[cardNumber].Width;
                            this.cardsPictureBoxArray[cardNumber].Visible = true;
                            this.Controls.Add(this.bots[currentBotNumber].Panel);
                            this.bots[currentBotNumber].InitializePanel(new Point(
                                cardsPictureBoxArray[currentBotStartCard].Left - 10, 
                                cardsPictureBoxArray[currentBotStartCard].Top - 10));
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
                        this.cardsPictureBoxArray[cardNumber].Image = backImage;
                        this.cardsPictureBoxArray[cardNumber].Location = new Point(horizontal, vertical);
                        horizontal += 110;
                    }
                }
                #endregion

                if (this.bots[0].Chips <= 0)
                {
                    this.bots[0].OutOfChips = true;
                    cardsPictureBoxArray[2].Visible = false;
                    cardsPictureBoxArray[3].Visible = false;
                }
                else
                {
                    this.bots[0].OutOfChips = false;
                    if (cardNumber == 3)
                    {
                        if (cardsPictureBoxArray[3] != null)
                        {
                            cardsPictureBoxArray[2].Visible = true;
                            cardsPictureBoxArray[3].Visible = true;
                        }
                    }
                }
                if (this.bots[1].Chips <= 0)
                {
                    this.bots[1].OutOfChips = true;
                    cardsPictureBoxArray[4].Visible = false;
                    cardsPictureBoxArray[5].Visible = false;
                }
                else
                {
                    this.bots[1].OutOfChips = false;
                    if (cardNumber == 5)
                    {
                        if (cardsPictureBoxArray[5] != null)
                        {
                            cardsPictureBoxArray[4].Visible = true;
                            cardsPictureBoxArray[5].Visible = true;
                        }
                    }
                }
                if (this.bots[2].Chips <= 0)
                {
                    this.bots[2].OutOfChips = true;
                    cardsPictureBoxArray[6].Visible = false;
                    cardsPictureBoxArray[7].Visible = false;
                }
                else
                {
                    this.bots[2].OutOfChips = false;
                    if (cardNumber == 7)
                    {
                        if (cardsPictureBoxArray[7] != null)
                        {
                            cardsPictureBoxArray[6].Visible = true;
                            cardsPictureBoxArray[7].Visible = true;
                        }
                    }
                }
                if (this.bots[3].Chips <= 0)
                {
                    this.bots[3].OutOfChips = true;
                    cardsPictureBoxArray[8].Visible = false;
                    cardsPictureBoxArray[9].Visible = false;
                }
                else
                {
                    this.bots[3].OutOfChips = false;
                    if (cardNumber == 9)
                    {
                        if (cardsPictureBoxArray[9] != null)
                        {
                            cardsPictureBoxArray[8].Visible = true;
                            cardsPictureBoxArray[9].Visible = true;
                        }
                    }
                }
                if (this.bots[4].Chips <= 0)
                {
                    this.bots[4].OutOfChips = true;
                    cardsPictureBoxArray[10].Visible = false;
                    cardsPictureBoxArray[11].Visible = false;
                }
                else
                {
                    this.bots[4].OutOfChips = false;
                    if (cardNumber == 11)
                    {
                        if (cardsPictureBoxArray[11] != null)
                        {
                            cardsPictureBoxArray[10].Visible = true;
                            cardsPictureBoxArray[11].Visible = true;
                        }
                    }
                }
                if (cardNumber == 16)
                {
                    if (!_restart)
                    {
                        MaximizeBox = true;
                        MinimizeBox = true;
                    }
                    _timer.Start();
                }

                this.i = cardNumber + 1;
            }
            if (foldedPlayers == 5)
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
                foldedPlayers = 5;
            }
            if (i == 17)
            {
                buttonRaise.Enabled = true;
                buttonCall.Enabled = true;
                buttonRaise.Enabled = true;
                buttonRaise.Enabled = true;
                buttonFold.Enabled = true;
            }
        }
        /* TODO metod za razdavane na kartite na bots
        private void DealCardsForBots(IBot currentBot, int cardNumber, Bitmap backImage,
            ref bool check, ref int horizontal, ref int vertical)
        {
            if (currentBot.Chips > 0)
            {
                this.foldedPlayers--;
                if (cardNumber >= currentBot.StartCard && cardNumber < currentBot.StartCard + 2)
                {
                    if (this.cardsPictureBoxArray[currentBot.StartCard].Tag != null)
                    {
                        this.cardsPictureBoxArray[currentBot.StartCard + 1].Tag = this.reservedGameCardsIndexes[currentBot.StartCard + 1];
                    }
                    this.cardsPictureBoxArray[currentBot.StartCard].Tag = this.reservedGameCardsIndexes[currentBot.StartCard];
                    if (!check)
                    {
                        vertical = currentBot.VerticalLocationCoordinate;
                        horizontal = currentBot.HorizontalLocationCoordinate;
                    }

                    check = true;
                    this.cardsPictureBoxArray[cardNumber].Anchor = currentBot.GetAnchorStyles();
                    this.cardsPictureBoxArray[cardNumber].Image = backImage;
                    this.cardsPictureBoxArray[cardNumber].Location = new Point(horizontal, vertical);
                    horizontal += this.cardsPictureBoxArray[cardNumber].Width;
                    this.cardsPictureBoxArray[cardNumber].Visible = true;
                    this.Controls.Add(currentBot.Panel);
                    currentBot.Panel.Visible = false;
                    currentBot.InitializePanel(new Point(
                        this.cardsPictureBoxArray[currentBot.StartCard].Left - 10,
                        this.cardsPictureBoxArray[currentBot.StartCard].Top - 10));

                    if (cardNumber == currentBot.StartCard + 1)
                    {
                        check = false;
                    }
                    
                }
            }
        }
        */
        async Task Turns()
        {
            #region Rotating
            if (!this.player.OutOfChips)
            {
                if (this.player.CanMakeTurn)
                {
                    this.FixCall(playerStatus, this.player, 1);
                    //MessageBox.Show("Player's Turn");
                    this.progressBarTimer.Visible = true;
                    this.progressBarTimer.Value = 1000;
                    this.secondsLeft = 60;
                    _up = 10000000;
                    _timer.Start();
                    buttonRaise.Enabled = true;
                    buttonCall.Enabled = true;
                    buttonRaise.Enabled = true;
                    buttonRaise.Enabled = true;
                    buttonFold.Enabled = true;
                    this.turnCount++;
                    FixCall(playerStatus, this.player, 2);
                }
            }
            if (this.player.OutOfChips || !this.player.CanMakeTurn)
            {
                await AllIn();
                if (this.player.OutOfChips && !this.player.Folded)
                {
                    if (buttonCall.Text.Contains("All in") == false || buttonRaise.Text.Contains("All in") == false)
                    {
                        this.gameDatabase.PlayersGameStatus.RemoveAt(0);
                        this.gameDatabase.PlayersGameStatus.Insert(0, null);
                        _maxLeft--;
                        this.player.Folded = true;
                    }
                }
                await CheckRaise(0, 0);
                progressBarTimer.Visible = false;
                buttonRaise.Enabled = false;
                buttonCall.Enabled = false;
                buttonRaise.Enabled = false;
                buttonRaise.Enabled = false;
                buttonFold.Enabled = false;
                _timer.Stop();
                this.bots[0].CanMakeTurn = true; //botOneTurn
                if (!this.bots[0].OutOfChips)
                {
                    if (this.bots[0].CanMakeTurn)
                    {
                        FixCall(this.botOneStatus, this.bots[0], 1);
                        FixCall(this.botOneStatus, this.bots[0], 2);
                        Rules(2, 3, this.bots[0]); //this.bots[0].Name, this.bots[0].Type, this.bots[0].Power, this.bots[0].OutOfChips
                        MessageBox.Show("Bot 1's Turn");
                        AI(2, 3, this.botOneStatus, 0, this.bots[0]);
                        this.turnCount++;
                        _last = 1;
                        this.bots[0].CanMakeTurn = false;
                        this.bots[1].CanMakeTurn = true;
                    }
                }
                if (this.bots[0].OutOfChips && !this.bots[0].Folded)
                {
                    this.gameDatabase.PlayersGameStatus.RemoveAt(1);
                    this.gameDatabase.PlayersGameStatus.Insert(1, null);
                    _maxLeft--;
                    this.bots[0].Folded = true;
                }
                if (this.bots[0].OutOfChips || !this.bots[0].CanMakeTurn)
                {
                    await CheckRaise(1, 1);
                    this.bots[1].CanMakeTurn = true;
                }
                if (!this.bots[1].OutOfChips)
                {
                    if (this.bots[1].CanMakeTurn)
                    {
                        FixCall(this.botTwoStatus, this.bots[1], 1);
                        FixCall(this.botTwoStatus, this.bots[1], 2);
                        Rules(4, 5, this.bots[1]);
                        MessageBox.Show("Bot 2's Turn");
                        AI(4, 5, this.botTwoStatus, 1, this.bots[1]);
                        this.turnCount++;
                        _last = 2;
                        this.bots[1].CanMakeTurn = false;
                        this.bots[2].CanMakeTurn = true;
                    }
                }
                if (this.bots[1].OutOfChips && !this.bots[1].Folded)
                {
                    this.gameDatabase.PlayersGameStatus.RemoveAt(2);
                    this.gameDatabase.PlayersGameStatus.Insert(2, null);
                    _maxLeft--;
                    this.bots[1].Folded = true;
                }
                if (this.bots[1].OutOfChips || !this.bots[1].CanMakeTurn)
                {
                    await CheckRaise(2, 2);
                    this.bots[2].CanMakeTurn = true;
                }
                if (!this.bots[2].OutOfChips)
                {
                    if (this.bots[2].CanMakeTurn)
                    {
                        FixCall(this.botThreeStatus, this.bots[2], 1);
                        FixCall(this.botThreeStatus, this.bots[2], 2);
                        Rules(6, 7, this.bots[2]);
                        MessageBox.Show("Bot 3's Turn");
                        AI(6, 7, this.botThreeStatus, 2, this.bots[2]);
                        this.turnCount++;
                        _last = 3;
                        this.bots[2].CanMakeTurn = false;
                        this.bots[3].CanMakeTurn = true;
                    }
                }
                if (this.bots[2].OutOfChips && !this.bots[2].Folded)
                {
                    this.gameDatabase.PlayersGameStatus.RemoveAt(3);
                    this.gameDatabase.PlayersGameStatus.Insert(3, null);
                    _maxLeft--;
                    this.bots[2].Folded = true;
                }
                if (this.bots[2].OutOfChips || !this.bots[2].CanMakeTurn)
                {
                    await CheckRaise(3, 3);
                    this.bots[3].CanMakeTurn = true;
                }
                if (!this.bots[3].OutOfChips)
                {
                    if (this.bots[3].CanMakeTurn)
                    {
                        FixCall(this.botFourStatus, this.bots[3], 1);
                        FixCall(this.botFourStatus, this.bots[3], 2);
                        Rules(8, 9, this.bots[3]);
                        MessageBox.Show("Bot 4's Turn");
                        AI(8, 9, this.botFourStatus, 3, this.bots[3]);
                        this.turnCount++;
                        _last = 4;
                        this.bots[3].CanMakeTurn = false;
                        this.bots[4].CanMakeTurn = true;
                    }
                }
                if (this.bots[3].OutOfChips && !this.bots[3].Folded)
                {
                    this.gameDatabase.PlayersGameStatus.RemoveAt(4);
                    this.gameDatabase.PlayersGameStatus.Insert(4, null);
                    _maxLeft--;
                    this.bots[3].Folded = true;
                }
                if (this.bots[3].OutOfChips || !this.bots[3].CanMakeTurn)
                {
                    await CheckRaise(4, 4);
                    this.bots[4].CanMakeTurn = true;
                }
                if (!this.bots[4].OutOfChips)
                {
                    if (this.bots[4].CanMakeTurn)
                    {
                        FixCall(this.botFiveStatus, this.bots[4], 1);
                        FixCall(this.botFiveStatus, this.bots[4], 2);
                        Rules(10, 11, this.bots[4]);
                        MessageBox.Show("Bot 5's Turn");
                        AI(10, 11, this.botFiveStatus, 4, this.bots[4]);
                        this.turnCount++;
                        _last = 5;
                        this.bots[4].CanMakeTurn = false;
                    }
                }
                if (this.bots[4].OutOfChips && !this.bots[4].Folded)
                {
                    this.gameDatabase.PlayersGameStatus.RemoveAt(5);
                    this.gameDatabase.PlayersGameStatus.Insert(5, null);
                    _maxLeft--;
                    this.bots[4].Folded = true;
                }
                if (this.bots[4].OutOfChips || !this.bots[4].CanMakeTurn)
                {
                    await CheckRaise(5, 5);
                    this.player.CanMakeTurn = true;
                }
                if (this.player.OutOfChips && !this.player.Folded)
                {
                    if (buttonCall.Text.Contains("All in") == false || buttonRaise.Text.Contains("All in") == false)
                    {
                        this.gameDatabase.PlayersGameStatus.RemoveAt(0);
                        this.gameDatabase.PlayersGameStatus.Insert(0, null);
                        _maxLeft--;
                        this.player.Folded = true;
                    }
                }
                #endregion
                await AllIn();
                if (!_restart)
                {
                    await Turns();
                }
                _restart = false;
            }
        }

        private void Rules(int card1, int card2, ICharacter currentPlayer)//string currentText, double current, double power, bool foldedTurn
        {
            if (card1 == 0 && card2 == 1)
            {
            }
            if (!currentPlayer.OutOfChips || card1 == 0 && card2 == 1 && playerStatus.Text.Contains("Fold") == false)
            {
                #region Variables
                bool done = false, vf = false;
                int[] cardsOnBoard = new int[5];
                int[] straight = new int[7];
                straight[0] = reservedGameCardsIndexes[card1];
                straight[1] = reservedGameCardsIndexes[card2];
                cardsOnBoard[0] = straight[2] = reservedGameCardsIndexes[12];
                cardsOnBoard[1] = straight[3] = reservedGameCardsIndexes[13];
                cardsOnBoard[2] = straight[4] = reservedGameCardsIndexes[14];
                cardsOnBoard[3] = straight[5] = reservedGameCardsIndexes[15];
                cardsOnBoard[4] = straight[6] = reservedGameCardsIndexes[16];
                var a = straight.Where(o => o % 4 == 0).ToArray();
                var b = straight.Where(o => o % 4 == 1).ToArray();
                var c = straight.Where(o => o % 4 == 2).ToArray();
                var d = straight.Where(o => o % 4 == 3).ToArray();
                var st1 = a.Select(o => o / 4).Distinct().ToArray();
                var st2 = b.Select(o => o / 4).Distinct().ToArray();
                var st3 = c.Select(o => o / 4).Distinct().ToArray();
                var st4 = d.Select(o => o / 4).Distinct().ToArray();
                Array.Sort(straight); Array.Sort(st1); Array.Sort(st2); Array.Sort(st3); Array.Sort(st4);
                #endregion
                for (i = 0; i < 16; i++)
                {
                    if (reservedGameCardsIndexes[i] == int.Parse(cardsPictureBoxArray[card1].Tag.ToString()) && reservedGameCardsIndexes[i + 1] == int.Parse(cardsPictureBoxArray[card2].Tag.ToString()))
                    {
                        //Pair from Hand current = 1

                        RPairFromHand(currentPlayer);

                        #region Pair or Two Pair from Table current = 2 || 0
                        RPairTwoPair(currentPlayer);
                        #endregion

                        #region Two Pair current = 2
                        RTwoPair(currentPlayer);
                        #endregion

                        #region Three of a kind current = 3
                        RThreeOfAKind(currentPlayer, straight); //ref currentPlayer.Type, ref currentPlayer.Power, straight
                        #endregion

                        #region Straight current = 4
                        RStraight(currentPlayer, straight);
                        #endregion

                        #region Flush current = 5 || 5.5
                        RFlush(currentPlayer, ref vf, cardsOnBoard);
                        #endregion

                        #region Full House current = 6
                        RFullHouse(currentPlayer, ref done, straight);
                        #endregion

                        #region Four of a Kind current = 7
                        RFourOfAKind(currentPlayer, straight);
                        #endregion

                        #region Straight Flush current = 8 || 9
                        RStraightFlush(currentPlayer, st1, st2, st3, st4);
                        #endregion

                        #region High Card current = -1
                        RHighCard(currentPlayer);
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
                        _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (st1[0] == 0 && st1[1] == 9 && st1[2] == 10 && st1[3] == 11 && st1[0] + 12 == st1[4])
                    {
                        currentPlayer.Type = 9;
                        currentPlayer.Power = (st1.Max()) / 4 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 9 });
                        _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (st2.Length >= 5)
                {
                    if (st2[0] + 4 == st2[4])
                    {
                        currentPlayer.Type = 8;
                        currentPlayer.Power = (st2.Max()) / 4 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 8 });
                        _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (st2[0] == 0 && st2[1] == 9 && st2[2] == 10 && st2[3] == 11 && st2[0] + 12 == st2[4])
                    {
                        currentPlayer.Type = 9;
                        currentPlayer.Power = (st2.Max()) / 4 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 9 });
                        _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (st3.Length >= 5)
                {
                    if (st3[0] + 4 == st3[4])
                    {
                        currentPlayer.Type = 8;
                        currentPlayer.Power = (st3.Max()) / 4 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 8 });
                        _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (st3[0] == 0 && st3[1] == 9 && st3[2] == 10 && st3[3] == 11 && st3[0] + 12 == st3[4])
                    {
                        currentPlayer.Type = 9;
                        currentPlayer.Power = (st3.Max() / 4) + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 9 });
                        _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (st4.Length >= 5)
                {
                    if (st4[0] + 4 == st4[4])
                    {
                        currentPlayer.Type = 8;
                        currentPlayer.Power = (st4.Max()) / 4 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 8 });
                        _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (st4[0] == 0 && st4[1] == 9 && st4[2] == 10 && st4[3] == 11 && st4[0] + 12 == st4[4])
                    {
                        currentPlayer.Type = 9;
                        currentPlayer.Power = (st4.Max()) / 4 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 9 });
                        _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                        _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (straight[j] / 4 == 0 && straight[j + 1] / 4 == 0 && straight[j + 2] / 4 == 0 && straight[j + 3] / 4 == 0)
                    {
                        currentPlayer.Type = 7;
                        currentPlayer.Power = 13 * 4 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 7 });
                        _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                                _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                                break;
                            }
                            if (fh.Max() / 4 > 0)
                            {
                                currentPlayer.Type = 6;
                                currentPlayer.Power = fh.Max() / 4 * 2 + currentPlayer.Type * 100;
                                this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 6 });
                                _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                    if (reservedGameCardsIndexes[i] % 4 == reservedGameCardsIndexes[i + 1] % 4 && reservedGameCardsIndexes[i] % 4 == f1[0] % 4)
                    {
                        if (reservedGameCardsIndexes[i] / 4 > f1.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = reservedGameCardsIndexes[i] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        if (reservedGameCardsIndexes[i + 1] / 4 > f1.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = reservedGameCardsIndexes[i + 1] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (reservedGameCardsIndexes[i] / 4 < f1.Max() / 4 && reservedGameCardsIndexes[i + 1] / 4 < f1.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f1.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f1.Length == 4)//different cards in hand
                {
                    if (reservedGameCardsIndexes[i] % 4 != reservedGameCardsIndexes[i + 1] % 4 && reservedGameCardsIndexes[i] % 4 == f1[0] % 4)
                    {
                        if (reservedGameCardsIndexes[i] / 4 > f1.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = reservedGameCardsIndexes[i] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f1.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                    if (reservedGameCardsIndexes[i + 1] % 4 != reservedGameCardsIndexes[i] % 4 && reservedGameCardsIndexes[i + 1] % 4 == f1[0] % 4)
                    {
                        if (reservedGameCardsIndexes[i + 1] / 4 > f1.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = reservedGameCardsIndexes[i + 1] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f1.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f1.Length == 5)
                {
                    if (reservedGameCardsIndexes[i] % 4 == f1[0] % 4 && reservedGameCardsIndexes[i] / 4 > f1.Min() / 4)
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = reservedGameCardsIndexes[i] + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                        _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    if (reservedGameCardsIndexes[i + 1] % 4 == f1[0] % 4 && reservedGameCardsIndexes[i + 1] / 4 > f1.Min() / 4)
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = reservedGameCardsIndexes[i + 1] + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                        _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (reservedGameCardsIndexes[i] / 4 < f1.Min() / 4 && reservedGameCardsIndexes[i + 1] / 4 < f1.Min())
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = f1.Max() + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                        _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f2.Length == 3 || f2.Length == 4)
                {
                    if (reservedGameCardsIndexes[i] % 4 == reservedGameCardsIndexes[i + 1] % 4 && reservedGameCardsIndexes[i] % 4 == f2[0] % 4)
                    {
                        if (reservedGameCardsIndexes[i] / 4 > f2.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = reservedGameCardsIndexes[i] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        if (reservedGameCardsIndexes[i + 1] / 4 > f2.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = reservedGameCardsIndexes[i + 1] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (reservedGameCardsIndexes[i] / 4 < f2.Max() / 4 && reservedGameCardsIndexes[i + 1] / 4 < f2.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f2.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f2.Length == 4)//different cards in hand
                {
                    if (reservedGameCardsIndexes[i] % 4 != reservedGameCardsIndexes[i + 1] % 4 && reservedGameCardsIndexes[i] % 4 == f2[0] % 4)
                    {
                        if (reservedGameCardsIndexes[i] / 4 > f2.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = reservedGameCardsIndexes[i] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f2.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                    if (reservedGameCardsIndexes[i + 1] % 4 != reservedGameCardsIndexes[i] % 4 && reservedGameCardsIndexes[i + 1] % 4 == f2[0] % 4)
                    {
                        if (reservedGameCardsIndexes[i + 1] / 4 > f2.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = reservedGameCardsIndexes[i + 1] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f2.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f2.Length == 5)
                {
                    if (reservedGameCardsIndexes[i] % 4 == f2[0] % 4 && reservedGameCardsIndexes[i] / 4 > f2.Min() / 4)
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = reservedGameCardsIndexes[i] + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                        _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    if (reservedGameCardsIndexes[i + 1] % 4 == f2[0] % 4 && reservedGameCardsIndexes[i + 1] / 4 > f2.Min() / 4)
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = reservedGameCardsIndexes[i + 1] + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                        _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (reservedGameCardsIndexes[i] / 4 < f2.Min() / 4 && reservedGameCardsIndexes[i + 1] / 4 < f2.Min())
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = f2.Max() + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                        _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f3.Length == 3 || f3.Length == 4)
                {
                    if (reservedGameCardsIndexes[i] % 4 == reservedGameCardsIndexes[i + 1] % 4 && reservedGameCardsIndexes[i] % 4 == f3[0] % 4)
                    {
                        if (reservedGameCardsIndexes[i] / 4 > f3.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = reservedGameCardsIndexes[i] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        if (reservedGameCardsIndexes[i + 1] / 4 > f3.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = reservedGameCardsIndexes[i + 1] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (reservedGameCardsIndexes[i] / 4 < f3.Max() / 4 && reservedGameCardsIndexes[i + 1] / 4 < f3.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f3.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f3.Length == 4)//different cards in hand
                {
                    if (reservedGameCardsIndexes[i] % 4 != reservedGameCardsIndexes[i + 1] % 4 && reservedGameCardsIndexes[i] % 4 == f3[0] % 4)
                    {
                        if (reservedGameCardsIndexes[i] / 4 > f3.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = reservedGameCardsIndexes[i] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f3.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                    if (reservedGameCardsIndexes[i + 1] % 4 != reservedGameCardsIndexes[i] % 4 && reservedGameCardsIndexes[i + 1] % 4 == f3[0] % 4)
                    {
                        if (reservedGameCardsIndexes[i + 1] / 4 > f3.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = reservedGameCardsIndexes[i + 1] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f3.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f3.Length == 5)
                {
                    if (reservedGameCardsIndexes[i] % 4 == f3[0] % 4 && reservedGameCardsIndexes[i] / 4 > f3.Min() / 4)
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = reservedGameCardsIndexes[i] + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                        _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    if (reservedGameCardsIndexes[i + 1] % 4 == f3[0] % 4 && reservedGameCardsIndexes[i + 1] / 4 > f3.Min() / 4)
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = reservedGameCardsIndexes[i + 1] + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                        _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (reservedGameCardsIndexes[i] / 4 < f3.Min() / 4 && reservedGameCardsIndexes[i + 1] / 4 < f3.Min())
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = f3.Max() + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                        _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f4.Length == 3 || f4.Length == 4)
                {
                    if (reservedGameCardsIndexes[i] % 4 == reservedGameCardsIndexes[i + 1] % 4 && reservedGameCardsIndexes[i] % 4 == f4[0] % 4)
                    {
                        if (reservedGameCardsIndexes[i] / 4 > f4.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = reservedGameCardsIndexes[i] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        if (reservedGameCardsIndexes[i + 1] / 4 > f4.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = reservedGameCardsIndexes[i + 1] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (reservedGameCardsIndexes[i] / 4 < f4.Max() / 4 && reservedGameCardsIndexes[i + 1] / 4 < f4.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f4.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f4.Length == 4)//different cards in hand
                {
                    if (reservedGameCardsIndexes[i] % 4 != reservedGameCardsIndexes[i + 1] % 4 && reservedGameCardsIndexes[i] % 4 == f4[0] % 4)
                    {
                        if (reservedGameCardsIndexes[i] / 4 > f4.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = reservedGameCardsIndexes[i] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f4.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                    if (reservedGameCardsIndexes[i + 1] % 4 != reservedGameCardsIndexes[i] % 4 && reservedGameCardsIndexes[i + 1] % 4 == f4[0] % 4)
                    {
                        if (reservedGameCardsIndexes[i + 1] / 4 > f4.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = reservedGameCardsIndexes[i + 1] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f4.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f4.Length == 5)
                {
                    if (reservedGameCardsIndexes[i] % 4 == f4[0] % 4 && reservedGameCardsIndexes[i] / 4 > f4.Min() / 4)
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = reservedGameCardsIndexes[i] + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                        _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    if (reservedGameCardsIndexes[i + 1] % 4 == f4[0] % 4 && reservedGameCardsIndexes[i + 1] / 4 > f4.Min() / 4)
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = reservedGameCardsIndexes[i + 1] + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                        _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (reservedGameCardsIndexes[i] / 4 < f4.Min() / 4 && reservedGameCardsIndexes[i + 1] / 4 < f4.Min())
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = f4.Max() + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                        _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }
                //ace
                if (f1.Length > 0)
                {
                    if (reservedGameCardsIndexes[i] / 4 == 0 && reservedGameCardsIndexes[i] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        currentPlayer.Type = 5.5;
                        currentPlayer.Power = 13 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5.5 });
                        _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (reservedGameCardsIndexes[i + 1] / 4 == 0 && reservedGameCardsIndexes[i + 1] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        currentPlayer.Type = 5.5;
                        currentPlayer.Power = 13 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5.5 });
                        _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (f2.Length > 0)
                {
                    if (reservedGameCardsIndexes[i] / 4 == 0 && reservedGameCardsIndexes[i] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        currentPlayer.Type = 5.5;
                        currentPlayer.Power = 13 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5.5 });
                        _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (reservedGameCardsIndexes[i + 1] / 4 == 0 && reservedGameCardsIndexes[i + 1] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        currentPlayer.Type = 5.5;
                        currentPlayer.Power = 13 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5.5 });
                        _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (f3.Length > 0)
                {
                    if (reservedGameCardsIndexes[i] / 4 == 0 && reservedGameCardsIndexes[i] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        currentPlayer.Type = 5.5;
                        currentPlayer.Power = 13 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5.5 });
                        _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (reservedGameCardsIndexes[i + 1] / 4 == 0 && reservedGameCardsIndexes[i + 1] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        currentPlayer.Type = 5.5;
                        currentPlayer.Power = 13 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5.5 });
                        _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (f4.Length > 0)
                {
                    if (reservedGameCardsIndexes[i] / 4 == 0 && reservedGameCardsIndexes[i] % 4 == f4[0] % 4 && vf && f4.Length > 0)
                    {
                        currentPlayer.Type = 5.5;
                        currentPlayer.Power = 13 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5.5 });
                        _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (reservedGameCardsIndexes[i + 1] / 4 == 0 && reservedGameCardsIndexes[i + 1] % 4 == f4[0] % 4 && vf)
                    {
                        currentPlayer.Type = 5.5;
                        currentPlayer.Power = 13 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5.5 });
                        _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                            _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        }
                        else
                        {
                            currentPlayer.Type = 4;
                            currentPlayer.Power = op[j + 4] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 4 });
                            _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        }
                    }
                    if (op[j] == 0 && op[j + 1] == 9 && op[j + 2] == 10 && op[j + 3] == 11 && op[j + 4] == 12)
                    {
                        currentPlayer.Type = 4;
                        currentPlayer.Power = 13 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 4 });
                        _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                            _sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                        else
                        {
                            currentPlayer.Type = 3;
                            currentPlayer.Power = fh[0] / 4 + fh[1] / 4 + fh[2] / 4 + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 3 });
                            _sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
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
                    if (reservedGameCardsIndexes[i] / 4 != reservedGameCardsIndexes[i + 1] / 4)
                    {
                        for (int k = 1; k <= max; k++)
                        {
                            if (tc - k < 12)
                            {
                                max--;
                            }
                            if (tc - k >= 12)
                            {
                                if (reservedGameCardsIndexes[i] / 4 == reservedGameCardsIndexes[tc] / 4 && reservedGameCardsIndexes[i + 1] / 4 == reservedGameCardsIndexes[tc - k] / 4 ||
                                    reservedGameCardsIndexes[i + 1] / 4 == reservedGameCardsIndexes[tc] / 4 && reservedGameCardsIndexes[i] / 4 == reservedGameCardsIndexes[tc - k] / 4)
                                {
                                    if (!msgbox)
                                    {
                                        if (reservedGameCardsIndexes[i] / 4 == 0)
                                        {
                                            currentPlayer.Type = 2;
                                            currentPlayer.Power = 13 * 4 + (reservedGameCardsIndexes[i + 1] / 4) * 2 + currentPlayer.Type * 100;
                                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 2 });
                                            _sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (reservedGameCardsIndexes[i + 1] / 4 == 0)
                                        {
                                            currentPlayer.Type = 2;
                                            currentPlayer.Power = 13 * 4 + (reservedGameCardsIndexes[i] / 4) * 2 + currentPlayer.Type * 100;
                                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 2 });
                                            _sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (reservedGameCardsIndexes[i + 1] / 4 != 0 && reservedGameCardsIndexes[i] / 4 != 0)
                                        {
                                            currentPlayer.Type = 2;
                                            currentPlayer.Power = (reservedGameCardsIndexes[i] / 4) * 2 + (reservedGameCardsIndexes[i + 1] / 4) * 2 + currentPlayer.Type * 100;
                                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 2 });
                                            _sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
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
                            if (reservedGameCardsIndexes[tc] / 4 == reservedGameCardsIndexes[tc - k] / 4)
                            {
                                if (reservedGameCardsIndexes[tc] / 4 != reservedGameCardsIndexes[i] / 4 && reservedGameCardsIndexes[tc] / 4 != reservedGameCardsIndexes[i + 1] / 4 && currentPlayer.Type == 1)
                                {
                                    if (!msgbox)
                                    {
                                        if (reservedGameCardsIndexes[i + 1] / 4 == 0)
                                        {
                                            currentPlayer.Type = 2;
                                            currentPlayer.Power = (reservedGameCardsIndexes[i] / 4) * 2 + 13 * 4 + currentPlayer.Type * 100;
                                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 2 });
                                            _sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (reservedGameCardsIndexes[i] / 4 == 0)
                                        {
                                            currentPlayer.Type = 2;
                                            currentPlayer.Power = (reservedGameCardsIndexes[i + 1] / 4) * 2 + 13 * 4 + currentPlayer.Type * 100;
                                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 2 });
                                            _sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (reservedGameCardsIndexes[i + 1] / 4 != 0)
                                        {
                                            currentPlayer.Type = 2;
                                            currentPlayer.Power = (reservedGameCardsIndexes[tc] / 4) * 2 + (reservedGameCardsIndexes[i + 1] / 4) * 2 + currentPlayer.Type * 100;
                                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 2 });
                                            _sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (reservedGameCardsIndexes[i] / 4 != 0)
                                        {
                                            currentPlayer.Type = 2;
                                            currentPlayer.Power = (reservedGameCardsIndexes[tc] / 4) * 2 + (reservedGameCardsIndexes[i] / 4) * 2 + currentPlayer.Type * 100;
                                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 2 });
                                            _sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                    }
                                    msgbox = true;
                                }
                                if (currentPlayer.Type == -1)
                                {
                                    if (!msgbox1)
                                    {
                                        if (reservedGameCardsIndexes[i] / 4 > reservedGameCardsIndexes[i + 1] / 4)
                                        {
                                            if (reservedGameCardsIndexes[tc] / 4 == 0)
                                            {
                                                currentPlayer.Type = 0;
                                                currentPlayer.Power = 13 + reservedGameCardsIndexes[i] / 4 + currentPlayer.Type * 100;
                                                this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 1 });
                                                _sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                            else
                                            {
                                                currentPlayer.Type = 0;
                                                currentPlayer.Power = reservedGameCardsIndexes[tc] / 4 + reservedGameCardsIndexes[i] / 4 + currentPlayer.Type * 100;
                                                this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 1 });
                                                _sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                        }
                                        else
                                        {
                                            if (reservedGameCardsIndexes[tc] / 4 == 0)
                                            {
                                                currentPlayer.Type = 0;
                                                currentPlayer.Power = 13 + reservedGameCardsIndexes[i + 1] + currentPlayer.Type * 100;
                                                this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 1 });
                                                _sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                            else
                                            {
                                                currentPlayer.Type = 0;
                                                currentPlayer.Power = reservedGameCardsIndexes[tc] / 4 + reservedGameCardsIndexes[i + 1] / 4 + currentPlayer.Type * 100;
                                                this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 1 });
                                                _sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
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
                if (reservedGameCardsIndexes[i] / 4 == reservedGameCardsIndexes[i + 1] / 4)
                {
                    if (!msgbox)
                    {
                        if (reservedGameCardsIndexes[i] / 4 == 0)
                        {
                            currentPlayer.Type = 1;
                            currentPlayer.Power = 13 * 4 + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 1 });
                            _sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                        else
                        {
                            currentPlayer.Type = 1;
                            currentPlayer.Power = (reservedGameCardsIndexes[i + 1] / 4) * 4 + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 1 });
                            _sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                    }
                    msgbox = true;
                }
                for (int tc = 16; tc >= 12; tc--)
                {
                    if (reservedGameCardsIndexes[i + 1] / 4 == reservedGameCardsIndexes[tc] / 4)
                    {
                        if (!msgbox)
                        {
                            if (reservedGameCardsIndexes[i + 1] / 4 == 0)
                            {
                                currentPlayer.Type = 1;
                                currentPlayer.Power = 13 * 4 + reservedGameCardsIndexes[i] / 4 + currentPlayer.Type * 100;
                                this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 1 });
                                _sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                            else
                            {
                                currentPlayer.Type = 1;
                                currentPlayer.Power = (reservedGameCardsIndexes[i + 1] / 4) * 4 + reservedGameCardsIndexes[i] / 4 + currentPlayer.Type * 100;
                                this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 1 });
                                _sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                        }
                        msgbox = true;
                    }
                    if (reservedGameCardsIndexes[i] / 4 == reservedGameCardsIndexes[tc] / 4)
                    {
                        if (!msgbox)
                        {
                            if (reservedGameCardsIndexes[i] / 4 == 0)
                            {
                                currentPlayer.Type = 1;
                                currentPlayer.Power = 13 * 4 + reservedGameCardsIndexes[i + 1] / 4 + currentPlayer.Type * 100;
                                this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 1 });
                                _sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                            else
                            {
                                currentPlayer.Type = 1;
                                currentPlayer.Power = (reservedGameCardsIndexes[tc] / 4) * 4 + reservedGameCardsIndexes[i + 1] / 4 + currentPlayer.Type * 100;
                                this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 1 });
                                _sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
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
                if (reservedGameCardsIndexes[i] / 4 > reservedGameCardsIndexes[i + 1] / 4)
                {
                    currentPlayer.Type = -1;
                    currentPlayer.Power = reservedGameCardsIndexes[i] / 4;
                    this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = -1 });
                    _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
                else
                {
                    currentPlayer.Type = -1;
                    currentPlayer.Power = reservedGameCardsIndexes[i + 1] / 4;
                    this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = -1 });
                    _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
                if (reservedGameCardsIndexes[i] / 4 == 0 || reservedGameCardsIndexes[i + 1] / 4 == 0)
                {
                    currentPlayer.Type = -1;
                    currentPlayer.Power = 13;
                    this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = -1 });
                    _sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
            }
        }

        void Winner(ICharacter currentPlayer, string lastly) //this.player.Type, this.player.Power, "Player", this.player.Chips, fixedLast
        {
            if (lastly == " ")
            {
                lastly = "Bot 5";
            }
            for (int j = 0; j <= 16; j++)
            {
                //await Task.Delay(5);
                if (cardsPictureBoxArray[j].Visible)
                    cardsPictureBoxArray[j].Image = gameCardsAsImages[j];
            }
            if (currentPlayer.Type == _sorted.Current)
            {
                if (currentPlayer.Power == _sorted.Power)
                {
                    _winners++;
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
                if (_winners > 1)
                {
                    if (this.gameDatabase.CheckWinners.Contains(this.player.Name))
                    {
                        this.player.Chips += int.Parse(textBoxPot.Text) / _winners;
                        textBoxPlayerChips.Text = this.player.ToString();
                        //pPanel.Visible = true;

                    }
                    if (this.gameDatabase.CheckWinners.Contains(this.bots[0].Name))
                    {
                        this.bots[0].Chips += int.Parse(textBoxPot.Text) / _winners;
                        textBoxBotOneChips.Text = this.bots[0].Chips.ToString();
                        //b1Panel.Visible = true;
                    }
                    if (this.gameDatabase.CheckWinners.Contains(this.bots[1].Name))
                    {
                        this.bots[1].Chips += int.Parse(textBoxPot.Text) / _winners;
                        textBoxBotTwoChips.Text = this.bots[1].Chips.ToString();
                        //b2Panel.Visible = true;
                    }
                    if (this.gameDatabase.CheckWinners.Contains(this.bots[2].Name))
                    {
                        this.bots[2].Chips += int.Parse(textBoxPot.Text) / _winners;
                        textBoxBotThreeChips.Text = this.bots[2].Chips.ToString();
                        //b3Panel.Visible = true;
                    }
                    if (this.gameDatabase.CheckWinners.Contains(this.bots[3].Name))
                    {
                        this.bots[3].Chips += int.Parse(textBoxPot.Text) / _winners;
                        textBoxBotFourChips.Text = this.bots[3].Chips.ToString();
                        //b4Panel.Visible = true;
                    }
                    if (this.gameDatabase.CheckWinners.Contains(this.bots[4].Name))
                    {
                        this.bots[4].Chips += int.Parse(textBoxPot.Text) / _winners;
                        textBoxBotFiveChips.Text = this.bots[4].Chips.ToString();
                        //b5Panel.Visible = true;
                    }
                    //await Finish(1);
                }
                if (_winners == 1)
                {
                    if (this.gameDatabase.CheckWinners.Contains(this.player.Name))
                    {
                        this.player.Chips += int.Parse(textBoxPot.Text);
                        //await Finish(1);
                        //pPanel.Visible = true;
                    }
                    if (this.gameDatabase.CheckWinners.Contains(this.bots[0].Name))
                    {
                        this.bots[0].Chips += int.Parse(textBoxPot.Text);
                        //await Finish(1);
                        //b1Panel.Visible = true;
                    }
                    if (this.gameDatabase.CheckWinners.Contains(this.bots[1].Name))
                    {
                        this.bots[1].Chips += int.Parse(textBoxPot.Text);
                        //await Finish(1);
                        //b2Panel.Visible = true;

                    }
                    if (this.gameDatabase.CheckWinners.Contains(this.bots[2].Name))
                    {
                        this.bots[2].Chips += int.Parse(textBoxPot.Text);
                        //await Finish(1);
                        //b3Panel.Visible = true;
                    }
                    if (this.gameDatabase.CheckWinners.Contains(this.bots[3].Name))
                    {
                        this.bots[3].Chips += int.Parse(textBoxPot.Text);
                        //await Finish(1);
                        //b4Panel.Visible = true;
                    }
                    if (this.gameDatabase.CheckWinners.Contains(this.bots[4].Name))
                    {
                        this.bots[4].Chips += int.Parse(textBoxPot.Text);
                        //await Finish(1);
                        //b5Panel.Visible = true;
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
                _raisedTurn = currentTurn;
                this.changed = true;
            }
            else
            {
                if (this.turnCount >= _maxLeft - 1 || !this.changed && this.turnCount == _maxLeft)
                {
                    if (currentTurn == _raisedTurn - 1 || !this.changed && this.turnCount == _maxLeft || _raisedTurn == 0 && currentTurn == 5)
                    {
                        this.changed = false;
                        this.turnCount = 0;
                        raise = 0;
                        this.neededChipsToCall = 0;
                        _raisedTurn = 123;
                        rounds++;
                        if (!this.player.OutOfChips)
                            playerStatus.Text = "";
                        if (!this.bots[0].OutOfChips)
                            botOneStatus.Text = "";
                        if (!this.bots[1].OutOfChips)
                            botTwoStatus.Text = "";
                        if (!this.bots[2].OutOfChips)
                            botThreeStatus.Text = "";
                        if (!this.bots[3].OutOfChips)
                            botFourStatus.Text = "";
                        if (!this.bots[4].OutOfChips)
                            botFiveStatus.Text = "";
                    }
                }
            }
            if (rounds == _flop)
            {
                for (int j = 12; j <= 14; j++)
                {
                    if (cardsPictureBoxArray[j].Image != gameCardsAsImages[j])
                    {
                        cardsPictureBoxArray[j].Image = gameCardsAsImages[j];
                        this.player.Call = 0; this.player.Raise = 0;
                        this.bots[0].Call = 0; this.bots[0].Raise = 0;
                        this.bots[1].Call = 0; this.bots[1].Raise = 0;
                        this.bots[2].Call = 0; this.bots[2].Raise = 0;
                        this.bots[3].Call = 0; this.bots[3].Raise = 0;
                        this.bots[4].Call = 0; this.bots[4].Raise = 0;
                    }
                }
            }
            if (rounds == _turn)
            {
                for (int j = 14; j <= 15; j++)
                {
                    if (cardsPictureBoxArray[j].Image != gameCardsAsImages[j])
                    {
                        cardsPictureBoxArray[j].Image = gameCardsAsImages[j];
                        this.player.Call = 0; this.player.Raise = 0;
                        this.bots[0].Call = 0; this.bots[0].Raise = 0;
                        this.bots[1].Call = 0; this.bots[1].Raise = 0;
                        this.bots[2].Call = 0; this.bots[2].Raise = 0;
                        this.bots[3].Call = 0; this.bots[3].Raise = 0;
                        this.bots[4].Call = 0; this.bots[4].Raise = 0;
                    }
                }
            }
            if (rounds == _river)
            {
                for (int j = 15; j <= 16; j++)
                {
                    if (cardsPictureBoxArray[j].Image != gameCardsAsImages[j])
                    {
                        cardsPictureBoxArray[j].Image = gameCardsAsImages[j];
                        this.player.Call = 0; this.player.Raise = 0;
                        this.bots[0].Call = 0; this.bots[0].Raise = 0;
                        this.bots[1].Call = 0; this.bots[1].Raise = 0;
                        this.bots[2].Call = 0; this.bots[2].Raise = 0;
                        this.bots[3].Call = 0; this.bots[3].Raise = 0;
                        this.bots[4].Call = 0; this.bots[4].Raise = 0;
                    }
                }
            }
            if (rounds == _end && _maxLeft == 6)
            {
                string fixedLast = "qwerty";
                if (!playerStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Player";
                    Rules(0, 1, this.player);
                }
                if (!botOneStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 1";
                    Rules(2, 3, this.bots[0]);
                }
                if (!botTwoStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 2";
                    Rules(4, 5, this.bots[1]);
                }
                if (!botThreeStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 3";
                    Rules(6, 7, this.bots[2]);
                }
                if (!botFourStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 4";
                    Rules(8, 9, this.bots[3]);
                }
                if (!botFiveStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 5";
                    Rules(10, 11, this.bots[4]);
                }
                this.Winner(this.player, fixedLast); //this.player.Type, this.player.Power, "Player", this.player.Chips, fixedLast
                this.Winner(this.bots[0], fixedLast);
                this.Winner(this.bots[1], fixedLast);
                this.Winner(this.bots[2], fixedLast);
                this.Winner(this.bots[3], fixedLast);
                this.Winner(this.bots[4], fixedLast);
                _restart = true;
                this.player.CanMakeTurn = true;
                this.player.OutOfChips = false;
                this.bots[0].OutOfChips = false;
                this.bots[1].OutOfChips = false;
                this.bots[2].OutOfChips = false;
                this.bots[3].OutOfChips = false;
                this.bots[4].OutOfChips = false;
                if (this.player.Chips <= 0)
                {
                    AddChips chipAdder = new AddChips();
                    chipAdder.ShowDialog();
                    if (chipAdder.A != 0)
                    {
                        this.player.Chips = chipAdder.A;
                        this.bots[0].Chips += chipAdder.A;
                        this.bots[1].Chips += chipAdder.A;
                        this.bots[2].Chips += chipAdder.A;
                        this.bots[3].Chips += chipAdder.A;
                        this.bots[4].Chips += chipAdder.A;
                        this.player.OutOfChips = false;
                        this.player.CanMakeTurn = true;
                        buttonRaise.Enabled = true;
                        buttonFold.Enabled = true;
                        buttonCheck.Enabled = true;
                        buttonRaise.Text = "Raise";
                    }
                }
                this.player.Panel.Visible = false; this.bots[0].Panel.Visible = false; this.bots[1].Panel.Visible = false; this.bots[2].Panel.Visible = false; this.bots[3].Panel.Visible = false; this.bots[4].Panel.Visible = false;
                this.player.Call = 0; this.player.Raise = 0;
                this.bots[0].Call = 0; this.bots[0].Raise = 0;
                this.bots[1].Call = 0; this.bots[1].Raise = 0;
                this.bots[2].Call = 0; this.bots[2].Raise = 0;
                this.bots[3].Call = 0; this.bots[3].Raise = 0;
                this.bots[4].Call = 0; this.bots[4].Raise = 0;
                _last = 0;
                this.neededChipsToCall = this.bigBlindValue;
                raise = 0;
                cardsImageLocation = Directory.GetFiles("..\\..\\Resources\\Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
                this.gameDatabase.PlayersGameStatus.Clear();
                rounds = 0;
                this.player.Power = 0; this.player.Type = -1;
                type = 0; this.bots[0].Power = 0; this.bots[1].Power = 0; this.bots[2].Power = 0; this.bots[3].Power = 0; this.bots[4].Power = 0;
                this.bots[0].Type = -1; this.bots[1].Type = -1; this.bots[2].Type = -1; this.bots[3].Type = -1; this.bots[4].Type = -1;
                this.gameDatabase.Chips.Clear();
                this.gameDatabase.CheckWinners.Clear();
                _winners = 0;
                this.gameDatabase.Win.Clear();
                _sorted.Current = 0;
                _sorted.Power = 0;
                for (int os = 0; os < 17; os++)
                {
                    cardsPictureBoxArray[os].Image = null;
                    cardsPictureBoxArray[os].Invalidate();
                    cardsPictureBoxArray[os].Visible = false;
                }
                textBoxPot.Text = "0";
                playerStatus.Text = "";
                await Shuffle();
                await Turns();
            }
        }
        void FixCall(Label status, ICharacter currentPlayer, int options) //ref int cCall, ref int cRaise
        {
            if (rounds != 4)
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
                    if (currentPlayer.Raise != raise && currentPlayer.Raise <= raise)
                    {
                        this.neededChipsToCall = Convert.ToInt32(raise) - currentPlayer.Raise;
                    }
                    if (currentPlayer.Call != this.neededChipsToCall || currentPlayer.Call <= this.neededChipsToCall)
                    {
                        this.neededChipsToCall = this.neededChipsToCall - currentPlayer.Call;
                    }
                    if (currentPlayer.Raise == raise && raise > 0)
                    {
                        this.neededChipsToCall = 0;
                        buttonCall.Enabled = false;
                        buttonCall.Text = "Callisfuckedup";
                    }
                }
            }
        }
        async Task AllIn()
        {
            #region All in
            if (this.player.Chips <= 0 && !this.chipsAreAdded)
            {
                if (playerStatus.Text.Contains("Raise"))
                {
                    this.gameDatabase.Chips.Add(this.player.Chips);
                    this.chipsAreAdded = true;
                }
                if (playerStatus.Text.Contains("Call"))
                {
                    this.gameDatabase.Chips.Add(this.player.Chips);
                    this.chipsAreAdded = true;
                }
            }
            this.chipsAreAdded = false;
            if (this.bots[0].Chips <= 0 && !this.bots[0].OutOfChips)
            {
                if (!this.chipsAreAdded)
                {
                    this.gameDatabase.Chips.Add(this.bots[0].Chips);
                    this.chipsAreAdded = true;
                }
                this.chipsAreAdded = false;
            }
            if (this.bots[1].Chips <= 0 && !this.bots[1].OutOfChips)
            {
                if (!this.chipsAreAdded)
                {
                    this.gameDatabase.Chips.Add(this.bots[1].Chips);
                    this.chipsAreAdded = true;
                }
                this.chipsAreAdded = false;
            }
            if (this.bots[2].Chips <= 0 && !this.bots[2].OutOfChips)
            {
                if (!this.chipsAreAdded)
                {
                    this.gameDatabase.Chips.Add(this.bots[2].Chips);
                    this.chipsAreAdded = true;
                }
                this.chipsAreAdded = false;
            }
            if (this.bots[3].Chips <= 0 && !this.bots[3].OutOfChips)
            {
                if (!this.chipsAreAdded)
                {
                    this.gameDatabase.Chips.Add(this.bots[3].Chips);
                    this.chipsAreAdded = true;
                }
                this.chipsAreAdded = false;
            }
            if (this.bots[4].Chips <= 0 && !this.bots[4].OutOfChips)
            {
                if (!this.chipsAreAdded)
                {
                    this.gameDatabase.Chips.Add(this.bots[4].Chips);
                    this.chipsAreAdded = true;
                }
            }
            if (this.gameDatabase.Chips.ToArray().Length == _maxLeft)
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
                    this.player.Chips += int.Parse(textBoxPot.Text);
                    textBoxPlayerChips.Text = this.player.Chips.ToString();
                    this.player.Panel.Visible = true;
                    MessageBox.Show("Player Wins");
                }
                if (index == 1)
                {
                    this.bots[0].Chips += int.Parse(textBoxPot.Text);
                    textBoxPlayerChips.Text = this.bots[0].Chips.ToString();
                    this.bots[0].Panel.Visible = true;
                    MessageBox.Show("Bot 1 Wins");
                }
                if (index == 2)
                {
                    this.bots[1].Chips += int.Parse(textBoxPot.Text);
                    textBoxPlayerChips.Text = this.bots[1].Chips.ToString();
                    this.bots[1].Panel.Visible = true;
                    MessageBox.Show("Bot 2 Wins");
                }
                if (index == 3)
                {
                    this.bots[2].Chips += int.Parse(textBoxPot.Text);
                    textBoxPlayerChips.Text = this.bots[2].Chips.ToString();
                    this.bots[2].Panel.Visible = true;
                    MessageBox.Show("Bot 3 Wins");
                }
                if (index == 4)
                {
                    this.bots[3].Chips += int.Parse(textBoxPot.Text);
                    textBoxPlayerChips.Text = this.bots[3].Chips.ToString();
                    this.bots[3].Panel.Visible = true;
                    MessageBox.Show("Bot 4 Wins");
                }
                if (index == 5)
                {
                    this.bots[4].Chips += int.Parse(textBoxPot.Text);
                    textBoxPlayerChips.Text = this.bots[4].Chips.ToString();
                    this.bots[4].Panel.Visible = true;
                    MessageBox.Show("Bot 5 Wins");
                }
                for (int j = 0; j <= 16; j++)
                {
                    cardsPictureBoxArray[j].Visible = false;
                }
                await Finish(1);
            }
            this.chipsAreAdded = false;
            #endregion

            #region FiveOrLessLeft
            if (abc < 6 && abc > 1 && rounds >= _end)
            {
                await Finish(2);
            }
            #endregion


        }
        async Task Finish(int n)
        {
            if (n == 2)
            {
                FixWinners();
            }
            this.player.Panel.Visible = false;
            this.bots[0].Panel.Visible = false;
            this.bots[1].Panel.Visible = false;
            this.bots[2].Panel.Visible = false;
            this.bots[3].Panel.Visible = false;
            this.bots[4].Panel.Visible = false;

            this.neededChipsToCall = bigBlindValue;
            raise = 0;
            foldedPlayers = 5;

            type = 0;
            rounds = 0;
            this.bots[0].Power = 0;
            this.bots[1].Power = 0;
            this.bots[2].Power = 0;
            this.bots[3].Power = 0;
            this.bots[4].Power = 0;
            this.player.Power = 0;

            this.player.Type = -1;
            raise = 0;
            this.bots[0].Type = -1;
            this.bots[1].Type = -1;
            this.bots[2].Type = -1;
            this.bots[3].Type = -1;
            this.bots[4].Type = -1;

            this.player.CanMakeTurn = true;
            this.bots[0].CanMakeTurn = false;
            this.bots[1].CanMakeTurn = false;
            this.bots[2].CanMakeTurn = false;
            this.bots[3].CanMakeTurn = false;
            this.bots[4].CanMakeTurn = false;

            this.player.OutOfChips = false;
            this.bots[0].OutOfChips = false;
            this.bots[1].OutOfChips = false;
            this.bots[2].OutOfChips = false;
            this.bots[3].OutOfChips = false;
            this.bots[4].OutOfChips = false;

            this.player.Folded = false;
            this.bots[0].Folded = false;
            this.bots[1].Folded = false;
            this.bots[2].Folded = false;
            this.bots[3].Folded = false;
            this.bots[4].Folded = false;

            _restart = false;
            this.raising = false;

            this.player.Call = 0;
            this.bots[0].Call = 0;
            this.bots[1].Call = 0;
            this.bots[2].Call = 0;
            this.bots[3].Call = 0;
            this.bots[4].Call = 0;

            this.player.Raise = 0;
            this.bots[0].Raise = 0;
            this.bots[1].Raise = 0;
            this.bots[2].Raise = 0;
            this.bots[3].Raise = 0;
            this.bots[4].Raise = 0;

            _height = 0;
            _width = 0;
            _winners = 0;
            _flop = 1;
            _turn = 2;
            _river = 3;
            _end = 4;
            _maxLeft = 6;
            _last = 123; _raisedTurn = 1;
            this.gameDatabase.PlayersGameStatus.Clear();
            this.gameDatabase.CheckWinners.Clear();
            this.gameDatabase.Chips.Clear();
            this.gameDatabase.Win.Clear();
            _sorted.Current = 0;
            _sorted.Power = 0;
            textBoxPot.Text = "0";
            this.secondsLeft = 60; _up = 10000000; this.turnCount = 0;
            playerStatus.Text = "";
            botOneStatus.Text = "";
            botTwoStatus.Text = "";
            botThreeStatus.Text = "";
            botFourStatus.Text = "";
            botFiveStatus.Text = "";
            if (this.player.Chips <= 0)
            {
                AddChips f2 = new AddChips();
                f2.ShowDialog();
                if (f2.A != 0)
                {
                    this.player.Chips = f2.A;
                    this.bots[0].Chips += f2.A;
                    this.bots[1].Chips += f2.A;
                    this.bots[2].Chips += f2.A;
                    this.bots[3].Chips += f2.A;
                    this.bots[4].Chips += f2.A;
                    this.player.OutOfChips = false;
                    this.player.CanMakeTurn = true;
                    buttonRaise.Enabled = true;
                    buttonFold.Enabled = true;
                    buttonCheck.Enabled = true;
                    buttonRaise.Text = "Raise";
                }
            }
            cardsImageLocation = Directory.GetFiles("..\\..\\Resources\\Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
            for (int os = 0; os < 17; os++)
            {
                cardsPictureBoxArray[os].Image = null;
                cardsPictureBoxArray[os].Invalidate();
                cardsPictureBoxArray[os].Visible = false;
            }
            await Shuffle();
            //await Turns();
        }
        void FixWinners()
        {
            this.gameDatabase.Win.Clear();
            _sorted.Current = 0;
            _sorted.Power = 0;
            string fixedLast = "qwerty";
            if (!playerStatus.Text.Contains("Fold"))
            {
                fixedLast = "Player";
                Rules(0, 1, this.player);
            }
            if (!botOneStatus.Text.Contains("Fold"))
            {
                fixedLast = "Bot 1";
                Rules(2, 3, this.bots[0]);
            }
            if (!botTwoStatus.Text.Contains("Fold"))
            {
                fixedLast = "Bot 2";
                Rules(4, 5, this.bots[1]);
            }
            if (!botThreeStatus.Text.Contains("Fold"))
            {
                fixedLast = "Bot 3";
                Rules(6, 7, this.bots[2]);
            }
            if (!botFourStatus.Text.Contains("Fold"))
            {
                fixedLast = "Bot 4";
                Rules(8, 9, this.bots[3]);
            }
            if (!botFiveStatus.Text.Contains("Fold"))
            {
                fixedLast = "Bot 5";
                Rules(10, 11, this.bots[4]);
            }
            this.Winner(this.player, fixedLast);
            this.Winner(this.bots[0], fixedLast);
            this.Winner(this.bots[1], fixedLast);
            this.Winner(this.bots[2], fixedLast);
            this.Winner(this.bots[3], fixedLast);
            this.Winner(this.bots[4], fixedLast);
        }

        /* staro Ai
        void Ai(int c1, int c2, ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower, double botCurrent)
        {
            if (!sFTurn)
            {
                if (botCurrent == -1)
                {
                    HighCard(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }
                if (botCurrent == 0)
                {
                    PairTable(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }
                if (botCurrent == 1)
                {
                    PairHand(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }
                if (botCurrent == 2)
                {
                    TwoPair(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }
                if (botCurrent == 3)
                {
                    ThreeOfAKind(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
                if (botCurrent == 4)
                {
                    Straight(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
                if (botCurrent == 5 || botCurrent == 5.5)
                {
                    Flush(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
                if (botCurrent == 6)
                {
                    FullHouse(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
                if (botCurrent == 7)
                {
                    FourOfAKind(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
                if (botCurrent == 8 || botCurrent == 9)
                {
                    StraightFlush(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
            }
            if (sFTurn)
            {
                cardsPictureBoxArray[c1].Visible = false;
                cardsPictureBoxArray[c2].Visible = false;
            }
        }
        */
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
            if (progressBarTimer.Value <= 0)
            {
                this.player.OutOfChips = true;
                await Turns();
            }
            if (this.secondsLeft > 0)
            {
                this.secondsLeft--;
                progressBarTimer.Value = (this.secondsLeft / 6) * 100;
            }
        }
        private void UpdateTick(object sender, object e)
        {
            if (this.player.Chips <= 0)
            {
                textBoxPlayerChips.Text = "Chips : 0";
            }
            if (this.bots[0].Chips <= 0)
            {
                textBoxBotOneChips.Text = "Chips : 0";
            }
            if (this.bots[1].Chips <= 0)
            {
                textBoxBotTwoChips.Text = "Chips : 0";
            }
            if (this.bots[2].Chips <= 0)
            {
                textBoxBotThreeChips.Text = "Chips : 0";
            }
            if (this.bots[3].Chips <= 0)
            {
                textBoxBotFourChips.Text = "Chips : 0";
            }
            if (this.bots[4].Chips <= 0)
            {
                textBoxBotFiveChips.Text = "Chips : 0";
            }
            textBoxPlayerChips.Text = "Chips : " + this.player.Chips.ToString();
            textBoxBotOneChips.Text = "Chips : " + this.bots[0].Chips.ToString();
            textBoxBotTwoChips.Text = "Chips : " + this.bots[1].Chips.ToString();
            textBoxBotThreeChips.Text = "Chips : " + this.bots[2].Chips.ToString();
            textBoxBotFourChips.Text = "Chips : " + this.bots[3].Chips.ToString();
            textBoxBotFiveChips.Text = "Chips : " + this.bots[4].Chips.ToString();
            if (this.player.Chips <= 0)
            {
                this.player.CanMakeTurn = false;
                this.player.OutOfChips = true;
                buttonCall.Enabled = false;
                buttonRaise.Enabled = false;
                buttonFold.Enabled = false;
                buttonCheck.Enabled = false;
            }
            if (_up > 0)
            {
                _up--;
            }
            if (this.player.Chips >= this.neededChipsToCall)
            {
                buttonCall.Text = "Call " + this.neededChipsToCall.ToString();
            }
            else
            {
                buttonCall.Text = "All in";
                buttonRaise.Enabled = false;
            }
            if (this.neededChipsToCall > 0)
            {
                buttonCheck.Enabled = false;
            }
            if (this.neededChipsToCall <= 0)
            {
                buttonCheck.Enabled = true;
                buttonCall.Text = "Call";
                buttonCall.Enabled = false;
            }
            if (this.player.Chips <= 0)
            {
                buttonRaise.Enabled = false;
            }
            int parsedValue;

            if (textBoxRaise.Text != "" && int.TryParse(textBoxRaise.Text, out parsedValue))
            {
                if (this.player.Chips <= int.Parse(textBoxRaise.Text))
                {
                    buttonRaise.Text = "All in";
                }
                else
                {
                    buttonRaise.Text = "Raise";
                }
            }
            if (this.player.Chips < this.neededChipsToCall)
            {
                buttonRaise.Enabled = false;
            }
        }

        private async void bFold_Click(object sender, EventArgs e)
        {
            playerStatus.Text = "Fold";
            this.player.CanMakeTurn = false;
            this.player.OutOfChips = true;
            await Turns();
        }

        private async void bCheck_Click(object sender, EventArgs e)
        {
            if (this.neededChipsToCall <= 0)
            {
                this.player.CanMakeTurn = false;
                playerStatus.Text = "Check";
            }
            else
            {
                //playerStatus.Text = "All in " + Chips;

                buttonCheck.Enabled = false;
            }
            await Turns();
        }
        private async void bCall_Click(object sender, EventArgs e)
        {
            Rules(0, 1, this.player);
            if (this.player.Chips >= this.neededChipsToCall)
            {
                this.player.Chips -= this.neededChipsToCall;
                textBoxPlayerChips.Text = "Chips : " + this.player.Chips.ToString();
                if (textBoxPot.Text != "")
                {
                    textBoxPot.Text = (int.Parse(textBoxPot.Text) + this.neededChipsToCall).ToString();
                }
                else
                {
                    textBoxPot.Text = this.neededChipsToCall.ToString();
                }
                this.player.CanMakeTurn = false;
                playerStatus.Text = "Call " + this.neededChipsToCall;
                this.player.Call = this.neededChipsToCall;
            }
            else if (this.player.Chips <= this.neededChipsToCall && this.neededChipsToCall > 0)
            {
                textBoxPot.Text = (int.Parse(textBoxPot.Text) + this.player.Chips).ToString();
                playerStatus.Text = "All in " + this.player.Chips;
                this.player.Chips = 0;
                textBoxPlayerChips.Text = "Chips : " + this.player.Chips.ToString();
                this.player.CanMakeTurn = false;
                buttonFold.Enabled = false;
                this.player.Call = this.player.Chips;
            }
            await Turns();
        }
        private async void bRaise_Click(object sender, EventArgs e)
        {
            Rules(0, 1, this.player);
            int parsedValue;
            if (textBoxRaise.Text != "" && int.TryParse(textBoxRaise.Text, out parsedValue))
            {
                if (this.player.Chips > this.neededChipsToCall)
                {
                    if (raise * 2 > int.Parse(textBoxRaise.Text))
                    {
                        textBoxRaise.Text = (raise * 2).ToString();
                        MessageBox.Show("You must raise atleast twice as the current raise !");
                        return;
                    }
                    else
                    {
                        if (this.player.Chips >= int.Parse(textBoxRaise.Text))
                        {
                            this.neededChipsToCall = int.Parse(textBoxRaise.Text);
                            raise = int.Parse(textBoxRaise.Text);
                            playerStatus.Text = "Raise " + this.neededChipsToCall.ToString();
                            textBoxPot.Text = (int.Parse(textBoxPot.Text) + this.neededChipsToCall).ToString();
                            buttonCall.Text = "Call";
                            this.player.Chips -= int.Parse(textBoxRaise.Text);
                            this.raising = true;
                            _last = 0;
                            this.player.Raise = Convert.ToInt32(raise);
                        }
                        else
                        {
                            this.neededChipsToCall = this.player.Chips;
                            raise = this.player.Chips;
                            textBoxPot.Text = (int.Parse(textBoxPot.Text) + this.player.Chips).ToString();
                            playerStatus.Text = "Raise " + this.neededChipsToCall.ToString();
                            this.player.Chips = 0;
                            this.raising = true;
                            _last = 0;
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
            await Turns();
        }
        private void bAdd_Click(object sender, EventArgs e)
        {
            if (textBoxAdd.Text == "") { }
            else
            {
                this.player.Chips += int.Parse(textBoxAdd.Text);
                this.bots[0].Chips += int.Parse(textBoxAdd.Text);
                this.bots[1].Chips += int.Parse(textBoxAdd.Text);
                this.bots[2].Chips += int.Parse(textBoxAdd.Text);
                this.bots[3].Chips += int.Parse(textBoxAdd.Text);
                this.bots[4].Chips += int.Parse(textBoxAdd.Text);
            }
            textBoxPlayerChips.Text = "Chips : " + this.player.Chips.ToString();
        }
        private void bOptions_Click(object sender, EventArgs e)
        {
            textBoxBigBlind.Text = bigBlindValue.ToString();
            textBoxSmallBlind.Text = smallBlindValue.ToString();
            if (textBoxBigBlind.Visible == false)
            {
                textBoxBigBlind.Visible = true;
                textBoxSmallBlind.Visible = true;
                buttonBigBlind.Visible = true;
                buttonSmallBlind.Visible = true;
            }
            else
            {
                textBoxBigBlind.Visible = false;
                textBoxSmallBlind.Visible = false;
                buttonBigBlind.Visible = false;
                buttonSmallBlind.Visible = false;
            }
        }
        private void bSB_Click(object sender, EventArgs e)
        {
            int parsedValue;
            if (textBoxSmallBlind.Text.Contains(",") || textBoxSmallBlind.Text.Contains("."))
            {
                MessageBox.Show("The Small Blind can be only round number !");
                textBoxSmallBlind.Text = smallBlindValue.ToString();
                return;
            }
            if (!int.TryParse(textBoxSmallBlind.Text, out parsedValue))
            {
                MessageBox.Show("This is a number only field");
                textBoxSmallBlind.Text = smallBlindValue.ToString();
                return;
            }
            if (int.Parse(textBoxSmallBlind.Text) > 100000)
            {
                MessageBox.Show("The maximum of the Small Blind is 100 000 $");
                textBoxSmallBlind.Text = smallBlindValue.ToString();
            }
            if (int.Parse(textBoxSmallBlind.Text) < 250)
            {
                MessageBox.Show("The minimum of the Small Blind is 250 $");
            }
            if (int.Parse(textBoxSmallBlind.Text) >= 250 && int.Parse(textBoxSmallBlind.Text) <= 100000)
            {
                smallBlindValue = int.Parse(textBoxSmallBlind.Text);
                MessageBox.Show("The changes have been saved ! They will become available the next hand you play. ");
            }
        }
        private void bBB_Click(object sender, EventArgs e)
        {
            int parsedValue;
            if (textBoxBigBlind.Text.Contains(",") || textBoxBigBlind.Text.Contains("."))
            {
                MessageBox.Show("The Big Blind can be only round number !");
                textBoxBigBlind.Text = bigBlindValue.ToString();
                return;
            }
            if (!int.TryParse(textBoxSmallBlind.Text, out parsedValue))
            {
                MessageBox.Show("This is a number only field");
                textBoxSmallBlind.Text = bigBlindValue.ToString();
                return;
            }
            if (int.Parse(textBoxBigBlind.Text) > 200000)
            {
                MessageBox.Show("The maximum of the Big Blind is 200 000");
                textBoxBigBlind.Text = bigBlindValue.ToString();
            }
            if (int.Parse(textBoxBigBlind.Text) < 500)
            {
                MessageBox.Show("The minimum of the Big Blind is 500 $");
            }
            if (int.Parse(textBoxBigBlind.Text) >= 500 && int.Parse(textBoxBigBlind.Text) <= 200000)
            {
                bigBlindValue = int.Parse(textBoxBigBlind.Text);
                MessageBox.Show("The changes have been saved ! They will become available the next hand you play. ");
            }
        }
        private void Layout_Change(object sender, LayoutEventArgs e)
        {
            _width = this.Width;
            _height = this.Height;
        }
        #endregion
    }
}
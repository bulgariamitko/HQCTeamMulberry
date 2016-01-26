namespace Poker
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Interfaces;
    using Poker.Data;
    using Poker.Models;
    using Poker.Models.Characters;

    public partial class MainWindow : Form
    {
        #region Variables
        public const int DefaultStartingChips = 10000;
        public const int DefaultSetOfCards = 52;
        public const int DefaultCardsInGame = 17;

        private readonly IPlayer player;
        private readonly IList<IBot> bots;
        private readonly IWinningHandType winningHandType;

        //private int Nm;

        //Players panels
        //private Panel this.player.Panel = new Panel();
        //private Panel this.bots[0].Panel = new Panel();
        //private Panel this.bots[1].Panel = new Panel();
        //private Panel this.bots[2].Panel = new Panel();
        //private Panel this.bots[3].Panel = new Panel();
        //private Panel this.bots[4].Panel = new Panel();

        private ProgressBar progressBar = new ProgressBar();

        private int neededChipsToCall;
        private int foldedPlayers;
        //Chips
        //public int Chips = DefaultStartingChips;
        //public int this.bots[0].Chips = DefaultStartingChips;
        //public int this.bots[1].Chips = DefaultStartingChips;
        //public int this.bots[2].Chips = DefaultStartingChips;
        //public int this.bots[3].Chips = DefaultStartingChips;
        //public int this.bots[4].Chips = DefaultStartingChips;

        private double type;
        private int rounds;
        //private double this.bots[0].Power;
        //private double this.bots[1].Power;
        //private double this.bots[2].Power;
        //private double this.bots[3].Power;
        //private double this.bots[4].Power;
        //private double this.player.Power = 0;

        //private double this.player.Type = -1;
        private int raise;
        //private double this.bots[0].Type = -1;
        //private double this.bots[1].Type = -1;
        //private double this.bots[2].Type = -1;
        //private double this.bots[3].Type = -1;
        //private double this.bots[4].Type = -1;
        //private bool botOneTurn = false;  // otivat v Character
        //private bool botTwoTurn = false;
        //private bool botThreeTurn = false;
        //private bool botFourTurn = false;
        ///private bool botFiveTurn = false;
        //private bool _b1Fturn = false, _b2Fturn = false, _b3Fturn = false, _b4Fturn = false, _b5Fturn = false; //pri igra4ite
        //private bool _pFolded, _b1Folded, _b2Folded, _b3Folded, _b4Folded, _b5Folded  // pri igra4ite

        private bool chipsAreAdded;
        private bool isChanged;

        //private int _pCall = 0, _b1Call = 0, _b2Call = 0, _b3Call = 0, _b4Call = 0, _b5Call = 0, _pRaise = 0, _b1Raise = 0, _b2Raise = 0, _b3Raise = 0, _b4Raise = 0, _b5Raise = 0; // pri igra4ite
        private int height;
        private int width;
        private int winners = 0;
        private int flop = 1;
        private int turn = 2;
        private int river = 3;
        private int end = 4;
        private int maxLeft = 6;
        private int last = 123;
        private int raisedTurn = 1;

        private IDatabase gameDatabase;
        //private List<bool?> playersGameStatus = new List<bool?>(); // database
        //private List<Type> win = new List<Type>(); // database
        //private List<string> _checkWinners = new List<string>(); // database
        //private List<int> ints = new List<int>();  // database

        //private bool _pFturn = false; // pri player
        //private bool _pturn = true; // pri player
        private bool isRestarted = false;

        private bool isRaising = false;
        private Type sorted;
        private string[] cardsImageLocation = Directory.GetFiles("..\\..\\Resources\\Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
        /*string[] cardsImageLocation ={
                   "Assets\\Cards\\33.png","Assets\\Cards\\22.png",
                    "Assets\\Cards\\29.png","Assets\\Cards\\21.png",
                    "Assets\\Cards\\36.png","Assets\\Cards\\17.png",
                    "Assets\\Cards\\40.png","Assets\\Cards\\16.png",
                    "Assets\\Cards\\5.png","Assets\\Cards\\47.png",
                    "Assets\\Cards\\37.png","Assets\\Cards\\13.png",
                    
                    "Assets\\Cards\\12.png",
                    "Assets\\Cards\\8.png","Assets\\Cards\\18.png",
                    "Assets\\Cards\\15.png","Assets\\Cards\\27.png"};*/
        private int[] reservedGameCardsIndexes = new int[17];
        private Image[] gameCardsAsImages = new Image[52];
        private PictureBox[] cardsPictureBoxArray = new PictureBox[52];

        private Timer timer = new Timer();
        private Timer updates = new Timer();

        private int secondsLeft = 60;
        private int i;
        private int bigBlindValue = 500;
        private int smallBlindValue = 250;
        private int up = 10000000;
        private int turnCount;

        #endregion
        public MainWindow()
        {
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
            this.neededChipsToCall = 500;
            this.foldedPlayers = 5;
            this.gameDatabase = new GameDatabase();
            this.turnCount = 0;

            //bools.Add(PFturn); bools.Add(B1Fturn); bools.Add(B2Fturn); bools.Add(B3Fturn); bools.Add(B4Fturn); bools.Add(B5Fturn);
            this.neededChipsToCall = this.bigBlindValue;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.updates.Start();
            InitializeComponent();
            width = this.Width;
            height = this.Height;
            Shuffle();
            textBoxPot.Enabled = false;
            textBoxPlayerChips.Enabled = false;
            textBoxBotOneChips.Enabled = false;
            textBoxBotTwoChips.Enabled = false;
            textBoxBotThreeChips.Enabled = false;
            textBoxBotFourChips.Enabled = false;
            textBoxBotFiveChips.Enabled = false;
            textBoxPlayerChips.Text = "Chips : " + this.player.Chips.ToString();
            textBoxBotOneChips.Text = "Chips : " + this.bots[0].Chips.ToString();
            textBoxBotTwoChips.Text = "Chips : " + this.bots[1].Chips.ToString();
            textBoxBotThreeChips.Text = "Chips : " + this.bots[2].Chips.ToString();
            textBoxBotFourChips.Text = "Chips : " + this.bots[3].Chips.ToString();
            this.textBoxBotFiveChips.Text = "Chips : " + this.bots[4].Chips.ToString();
            this.timer.Interval = 1 * 1 * 1000;
            this.timer.Tick += TimerTick;
            this.updates.Interval = 1 * 1 * 100;
            updates.Tick += UpdateTick;
            textBoxBigBlind.Visible = true;
            textBoxSmallBlind.Visible = true;
            buttonBigBlind.Visible = true;
            buttonSmallBlind.Visible = true;
            textBoxBigBlind.Visible = true;
            textBoxSmallBlind.Visible = true;
            buttonBigBlind.Visible = true;
            buttonSmallBlind.Visible = true;
            textBoxBigBlind.Visible = false;
            textBoxSmallBlind.Visible = false;
            buttonBigBlind.Visible = false;
            buttonSmallBlind.Visible = false;
            textBoxRaise.Text = (this.bigBlindValue * 2).ToString();
        }

        public async Task Shuffle()
        {
            this.gameDatabase.PlayersGameStatus.Add(this.player.OutOfChips);
            this.gameDatabase.PlayersGameStatus.Add(this.bots[0].OutOfChips);
            this.gameDatabase.PlayersGameStatus.Add(this.bots[1].OutOfChips);
            this.gameDatabase.PlayersGameStatus.Add(this.bots[2].OutOfChips);
            this.gameDatabase.PlayersGameStatus.Add(this.bots[3].OutOfChips);
            this.gameDatabase.PlayersGameStatus.Add(this.bots[4].OutOfChips);
            //playersGameStatus.Add(_pFturn); playersGameStatus.Add(_b1Fturn); playersGameStatus.Add(_b2Fturn); playersGameStatus.Add(_b3Fturn); playersGameStatus.Add(_b4Fturn); playersGameStatus.Add(_b5Fturn);
            buttonCall.Enabled = false;
            buttonRaise.Enabled = false;
            buttonFold.Enabled = false;
            buttonCheck.Enabled = false;
            MaximizeBox = false;
            MinimizeBox = false;
            bool check = false;
            Bitmap backImage = new Bitmap("..\\..\\Resources\\Assets\\Back\\Back.png");
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
                gameCardsAsImages[cardNumber] = Image.FromFile(cardsImageLocation[cardNumber]);
                var charsToRemove = new string[] { "..\\..\\Resources\\Assets\\Cards\\", ".png" };
                foreach (var c in charsToRemove)
                {
                    cardsImageLocation[cardNumber] = cardsImageLocation[cardNumber].Replace(c, string.Empty);
                }

                reservedGameCardsIndexes[cardNumber] = int.Parse(cardsImageLocation[cardNumber]) - 1;
                cardsPictureBoxArray[cardNumber] = new PictureBox();
                cardsPictureBoxArray[cardNumber].SizeMode = PictureBoxSizeMode.StretchImage;
                cardsPictureBoxArray[cardNumber].Height = 130;
                cardsPictureBoxArray[cardNumber].Width = 80;
                this.Controls.Add(cardsPictureBoxArray[cardNumber]);
                cardsPictureBoxArray[cardNumber].Name = "pb" + cardNumber.ToString();
                await Task.Delay(200);

                #region Throwing Cards
                if (cardNumber < 2)
                {
                    if (cardsPictureBoxArray[0].Tag != null)
                    {
                        cardsPictureBoxArray[1].Tag = reservedGameCardsIndexes[1];
                    }

                    cardsPictureBoxArray[0].Tag = reservedGameCardsIndexes[0];
                    cardsPictureBoxArray[cardNumber].Image = gameCardsAsImages[cardNumber];
                    cardsPictureBoxArray[cardNumber].Anchor = AnchorStyles.Bottom;
                    //Holder[cardNumber].Dock = DockStyle.Top;
                    cardsPictureBoxArray[cardNumber].Location = new Point(horizontal, vertical);
                    horizontal += cardsPictureBoxArray[cardNumber].Width;
                    this.Controls.Add(this.player.Panel);
                    this.player.InitializePanel(new Point(this.cardsPictureBoxArray[0].Left - 10, this.cardsPictureBoxArray[0].Top - 10));
                }

                if (this.bots[0].Chips > 0)
                {
                    foldedPlayers--;
                    if (cardNumber >= 2 && cardNumber < 4)
                    {
                        if (cardsPictureBoxArray[2].Tag != null)
                        {
                            cardsPictureBoxArray[3].Tag = reservedGameCardsIndexes[3];
                        }

                        cardsPictureBoxArray[2].Tag = reservedGameCardsIndexes[2];
                        if (!check)
                        {
                            horizontal = 15;
                            vertical = 420;
                        }

                        check = true;
                        cardsPictureBoxArray[cardNumber].Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
                        cardsPictureBoxArray[cardNumber].Image = backImage;
                        //Holder[i].Image = Deck[i];
                        cardsPictureBoxArray[cardNumber].Location = new Point(horizontal, vertical);
                        horizontal += cardsPictureBoxArray[cardNumber].Width;
                        cardsPictureBoxArray[cardNumber].Visible = true;
                        this.Controls.Add(this.bots[0].Panel);
                        this.bots[0].Panel.Location = new Point(cardsPictureBoxArray[2].Left - 10, cardsPictureBoxArray[2].Top - 10);
                        this.bots[0].Panel.BackColor = Color.DarkBlue;
                        this.bots[0].Panel.Height = 150;
                        this.bots[0].Panel.Width = 180;
                        this.bots[0].Panel.Visible = false;
                        if (cardNumber == 3)
                        {
                            check = false;
                        }
                    }
                }

                if (this.bots[1].Chips > 0)
                {
                    foldedPlayers--;
                    if (cardNumber >= 4 && cardNumber < 6)
                    {
                        if (cardsPictureBoxArray[4].Tag != null)
                        {
                            cardsPictureBoxArray[5].Tag = reservedGameCardsIndexes[5];
                        }

                        cardsPictureBoxArray[4].Tag = reservedGameCardsIndexes[4];
                        if (!check)
                        {
                            horizontal = 75;
                            vertical = 65;
                        }

                        check = true;
                        cardsPictureBoxArray[cardNumber].Anchor = AnchorStyles.Top | AnchorStyles.Left;
                        cardsPictureBoxArray[cardNumber].Image = backImage;
                        //Holder[i].Image = Deck[i];
                        cardsPictureBoxArray[cardNumber].Location = new Point(horizontal, vertical);
                        horizontal += cardsPictureBoxArray[cardNumber].Width;
                        cardsPictureBoxArray[cardNumber].Visible = true;
                        this.Controls.Add(this.bots[1].Panel);
                        this.bots[1].Panel.Location = new Point(cardsPictureBoxArray[4].Left - 10, cardsPictureBoxArray[4].Top - 10);
                        this.bots[1].Panel.BackColor = Color.DarkBlue;
                        this.bots[1].Panel.Height = 150;
                        this.bots[1].Panel.Width = 180;
                        this.bots[1].Panel.Visible = false;
                        if (cardNumber == 5)
                        {
                            check = false;
                        }
                    }
                }

                if (this.bots[2].Chips > 0)
                {
                    foldedPlayers--;
                    if (cardNumber >= 6 && cardNumber < 8)
                    {
                        if (cardsPictureBoxArray[6].Tag != null)
                        {
                            cardsPictureBoxArray[7].Tag = reservedGameCardsIndexes[7];
                        }

                        cardsPictureBoxArray[6].Tag = reservedGameCardsIndexes[6];
                        if (!check)
                        {
                            horizontal = 590;
                            vertical = 25;
                        }

                        check = true;
                        cardsPictureBoxArray[cardNumber].Anchor = AnchorStyles.Top;
                        cardsPictureBoxArray[cardNumber].Image = backImage;
                        //Holder[i].Image = Deck[i];
                        cardsPictureBoxArray[cardNumber].Location = new Point(horizontal, vertical);
                        horizontal += cardsPictureBoxArray[cardNumber].Width;
                        cardsPictureBoxArray[cardNumber].Visible = true;
                        this.Controls.Add(this.bots[2].Panel);
                        this.bots[2].Panel.Location = new Point(cardsPictureBoxArray[6].Left - 10, cardsPictureBoxArray[6].Top - 10);
                        this.bots[2].Panel.BackColor = Color.DarkBlue;
                        this.bots[2].Panel.Height = 150;
                        this.bots[2].Panel.Width = 180;
                        this.bots[2].Panel.Visible = false;
                        if (cardNumber == 7)
                        {
                            check = false;
                        }
                    }
                }

                if (this.bots[3].Chips > 0)
                {
                    foldedPlayers--;
                    if (cardNumber >= 8 && cardNumber < 10)
                    {
                        if (cardsPictureBoxArray[8].Tag != null)
                        {
                            cardsPictureBoxArray[9].Tag = reservedGameCardsIndexes[9];
                        }

                        cardsPictureBoxArray[8].Tag = reservedGameCardsIndexes[8];
                        if (!check)
                        {
                            horizontal = 1115;
                            vertical = 65;
                        }

                        check = true;
                        cardsPictureBoxArray[cardNumber].Anchor = AnchorStyles.Top | AnchorStyles.Right;
                        cardsPictureBoxArray[cardNumber].Image = backImage;
                        //Holder[i].Image = Deck[i];
                        cardsPictureBoxArray[cardNumber].Location = new Point(horizontal, vertical);
                        horizontal += cardsPictureBoxArray[cardNumber].Width;
                        cardsPictureBoxArray[cardNumber].Visible = true;
                        this.Controls.Add(this.bots[3].Panel);
                        this.bots[3].Panel.Location = new Point(cardsPictureBoxArray[8].Left - 10, cardsPictureBoxArray[8].Top - 10);
                        this.bots[3].Panel.BackColor = Color.DarkBlue;
                        this.bots[3].Panel.Height = 150;
                        this.bots[3].Panel.Width = 180;
                        this.bots[3].Panel.Visible = false;
                        if (cardNumber == 9)
                        {
                            check = false;
                        }
                    }
                }

                if (this.bots[4].Chips > 0)
                {
                    foldedPlayers--;
                    if (cardNumber >= 10 && cardNumber < 12)
                    {
                        if (cardsPictureBoxArray[10].Tag != null)
                        {
                            cardsPictureBoxArray[11].Tag = reservedGameCardsIndexes[11];
                        }

                        cardsPictureBoxArray[10].Tag = reservedGameCardsIndexes[10];
                        if (!check)
                        {
                            horizontal = 1160;
                            vertical = 420;
                        }

                        check = true;
                        cardsPictureBoxArray[cardNumber].Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                        cardsPictureBoxArray[cardNumber].Image = backImage;
                        //Holder[i].Image = Deck[i];
                        cardsPictureBoxArray[cardNumber].Location = new Point(horizontal, vertical);
                        horizontal += cardsPictureBoxArray[cardNumber].Width;
                        cardsPictureBoxArray[cardNumber].Visible = true;
                        this.Controls.Add(this.bots[4].Panel);
                        this.bots[4].Panel.Location = new Point(cardsPictureBoxArray[10].Left - 10, cardsPictureBoxArray[10].Top - 10);
                        this.bots[4].Panel.BackColor = Color.DarkBlue;
                        this.bots[4].Panel.Height = 150;
                        this.bots[4].Panel.Width = 180;
                        this.bots[4].Panel.Visible = false;
                        if (cardNumber == 11)
                        {
                            check = false;
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
                    if (cardsPictureBoxArray[cardNumber] != null)
                    {
                        cardsPictureBoxArray[cardNumber].Anchor = AnchorStyles.None;
                        cardsPictureBoxArray[cardNumber].Image = backImage;
                        //Holder[cardNumber].Image = Deck[cardNumber];
                        cardsPictureBoxArray[cardNumber].Location = new Point(horizontal, vertical);
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
                    if (!isRestarted)
                    {
                        MaximizeBox = true;
                        MinimizeBox = true;
                    }
                    timer.Start();
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
                    up = 10000000;
                    timer.Start();
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
                        maxLeft--;
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
                timer.Stop();
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
                        last = 1;
                        this.bots[0].CanMakeTurn = false;
                        this.bots[1].CanMakeTurn = true;
                    }
                }

                if (this.bots[0].OutOfChips && !this.bots[0].Folded)
                {
                    this.gameDatabase.PlayersGameStatus.RemoveAt(1);
                    this.gameDatabase.PlayersGameStatus.Insert(1, null);
                    maxLeft--;
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
                        last = 2;
                        this.bots[1].CanMakeTurn = false;
                        this.bots[2].CanMakeTurn = true;
                    }
                }

                if (this.bots[1].OutOfChips && !this.bots[1].Folded)
                {
                    this.gameDatabase.PlayersGameStatus.RemoveAt(2);
                    this.gameDatabase.PlayersGameStatus.Insert(2, null);
                    maxLeft--;
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
                        last = 3;
                        this.bots[2].CanMakeTurn = false;
                        this.bots[3].CanMakeTurn = true;
                    }
                }

                if (this.bots[2].OutOfChips && !this.bots[2].Folded)
                {
                    this.gameDatabase.PlayersGameStatus.RemoveAt(3);
                    this.gameDatabase.PlayersGameStatus.Insert(3, null);
                    maxLeft--;
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
                        last = 4;
                        this.bots[3].CanMakeTurn = false;
                        this.bots[4].CanMakeTurn = true;
                    }
                }

                if (this.bots[3].OutOfChips && !this.bots[3].Folded)
                {
                    this.gameDatabase.PlayersGameStatus.RemoveAt(4);
                    this.gameDatabase.PlayersGameStatus.Insert(4, null);
                    maxLeft--;
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
                        last = 5;
                        this.bots[4].CanMakeTurn = false;
                    }
                }

                if (this.bots[4].OutOfChips && !this.bots[4].Folded)
                {
                    this.gameDatabase.PlayersGameStatus.RemoveAt(5);
                    this.gameDatabase.PlayersGameStatus.Insert(5, null);
                    maxLeft--;
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
                        maxLeft--;
                        this.player.Folded = true;
                    }
                }

                #endregion

                await AllIn();
                if (!isRestarted)
                {
                    await Turns();
                }
                isRestarted = false;
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
                        currentPlayer.Power = st1.Max() / 4 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type { Power = currentPlayer.Power, Current = 8 });
                        sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (st1[0] == 0 && st1[1] == 9 && st1[2] == 10 && st1[3] == 11 && st1[0] + 12 == st1[4])
                    {
                        currentPlayer.Type = 9;
                        currentPlayer.Power = st1.Max() / 4 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type { Power = currentPlayer.Power, Current = 9 });
                        sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (st2.Length >= 5)
                {
                    if (st2[0] + 4 == st2[4])
                    {
                        currentPlayer.Type = 8;
                        currentPlayer.Power = st2.Max() / 4 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type { Power = currentPlayer.Power, Current = 8 });
                        sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (st2[0] == 0 && st2[1] == 9 && st2[2] == 10 && st2[3] == 11 && st2[0] + 12 == st2[4])
                    {
                        currentPlayer.Type = 9;
                        currentPlayer.Power = st2.Max() / 4 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type { Power = currentPlayer.Power, Current = 9 });
                        sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (st3.Length >= 5)
                {
                    if (st3[0] + 4 == st3[4])
                    {
                        currentPlayer.Type = 8;
                        currentPlayer.Power = st3.Max() / 4 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type { Power = currentPlayer.Power, Current = 8 });
                        sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (st3[0] == 0 && st3[1] == 9 && st3[2] == 10 && st3[3] == 11 && st3[0] + 12 == st3[4])
                    {
                        currentPlayer.Type = 9;
                        currentPlayer.Power = st3.Max() / 4 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type { Power = currentPlayer.Power, Current = 9 });
                        sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (st4.Length >= 5)
                {
                    if (st4[0] + 4 == st4[4])
                    {
                        currentPlayer.Type = 8;
                        currentPlayer.Power = st4.Max() / 4 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type { Power = currentPlayer.Power, Current = 8 });
                        sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (st4[0] == 0 && st4[1] == 9 && st4[2] == 10 && st4[3] == 11 && st4[0] + 12 == st4[4])
                    {
                        currentPlayer.Type = 9;
                        currentPlayer.Power = st4.Max() / 4 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 9 });
                        sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                        sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (straight[j] / 4 == 0 && straight[j + 1] / 4 == 0 && straight[j + 2] / 4 == 0 && straight[j + 3] / 4 == 0)
                    {
                        currentPlayer.Type = 7;
                        currentPlayer.Power = 13 * 4 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 7 });
                        sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                                sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                                break;
                            }

                            if (fh.Max() / 4 > 0)
                            {
                                currentPlayer.Type = 6;
                                currentPlayer.Power = fh.Max() / 4 * 2 + currentPlayer.Type * 100;
                                this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 6 });
                                sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                            sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (reservedGameCardsIndexes[i + 1] / 4 > f1.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = reservedGameCardsIndexes[i + 1] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (reservedGameCardsIndexes[i] / 4 < f1.Max() / 4 && reservedGameCardsIndexes[i + 1] / 4 < f1.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f1.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f1.Length == 4)
                {
                    if (reservedGameCardsIndexes[i] % 4 != reservedGameCardsIndexes[i + 1] % 4 && reservedGameCardsIndexes[i] % 4 == f1[0] % 4)
                    {
                        if (reservedGameCardsIndexes[i] / 4 > f1.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = reservedGameCardsIndexes[i] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f1.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                            sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f1.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                        sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (reservedGameCardsIndexes[i + 1] % 4 == f1[0] % 4 && reservedGameCardsIndexes[i + 1] / 4 > f1.Min() / 4)
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = reservedGameCardsIndexes[i + 1] + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                        sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (reservedGameCardsIndexes[i] / 4 < f1.Min() / 4 && reservedGameCardsIndexes[i + 1] / 4 < f1.Min())
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = f1.Max() + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                        sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                            sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        if (reservedGameCardsIndexes[i + 1] / 4 > f2.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = reservedGameCardsIndexes[i + 1] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (reservedGameCardsIndexes[i] / 4 < f2.Max() / 4 && reservedGameCardsIndexes[i + 1] / 4 < f2.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f2.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f2.Length == 4)
                {
                    if (reservedGameCardsIndexes[i] % 4 != reservedGameCardsIndexes[i + 1] % 4 && reservedGameCardsIndexes[i] % 4 == f2[0] % 4)
                    {
                        if (reservedGameCardsIndexes[i] / 4 > f2.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = reservedGameCardsIndexes[i] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f2.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                            sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f2.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                        sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    if (reservedGameCardsIndexes[i + 1] % 4 == f2[0] % 4 && reservedGameCardsIndexes[i + 1] / 4 > f2.Min() / 4)
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = reservedGameCardsIndexes[i + 1] + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                        sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (reservedGameCardsIndexes[i] / 4 < f2.Min() / 4 && reservedGameCardsIndexes[i + 1] / 4 < f2.Min())
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = f2.Max() + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                        sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                            sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (reservedGameCardsIndexes[i + 1] / 4 > f3.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = reservedGameCardsIndexes[i + 1] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (reservedGameCardsIndexes[i] / 4 < f3.Max() / 4 && reservedGameCardsIndexes[i + 1] / 4 < f3.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f3.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f3.Length == 4)
                {
                    if (reservedGameCardsIndexes[i] % 4 != reservedGameCardsIndexes[i + 1] % 4 && reservedGameCardsIndexes[i] % 4 == f3[0] % 4)
                    {
                        if (reservedGameCardsIndexes[i] / 4 > f3.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = reservedGameCardsIndexes[i] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f3.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                            sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f3.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                        sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    if (reservedGameCardsIndexes[i + 1] % 4 == f3[0] % 4 && reservedGameCardsIndexes[i + 1] / 4 > f3.Min() / 4)
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = reservedGameCardsIndexes[i + 1] + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                        sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (reservedGameCardsIndexes[i] / 4 < f3.Min() / 4 && reservedGameCardsIndexes[i + 1] / 4 < f3.Min())
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = f3.Max() + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                        sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                            sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (reservedGameCardsIndexes[i + 1] / 4 > f4.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = reservedGameCardsIndexes[i + 1] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (reservedGameCardsIndexes[i] / 4 < f4.Max() / 4 && reservedGameCardsIndexes[i + 1] / 4 < f4.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f4.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f4.Length == 4)
                {
                    if (reservedGameCardsIndexes[i] % 4 != reservedGameCardsIndexes[i + 1] % 4 && reservedGameCardsIndexes[i] % 4 == f4[0] % 4)
                    {
                        if (reservedGameCardsIndexes[i] / 4 > f4.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = reservedGameCardsIndexes[i] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f4.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                            sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f4.Max() + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                            sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                        sorted =
                            this.gameDatabase.Win.OrderByDescending(op1 => op1.Current)
                                .ThenByDescending(op1 => op1.Power)
                                .First();
                        vf = true;
                    }

                    if (reservedGameCardsIndexes[i + 1] % 4 == f4[0] % 4 && reservedGameCardsIndexes[i + 1] / 4 > f4.Min() / 4)
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = reservedGameCardsIndexes[i + 1] + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                        sorted =
                            this.gameDatabase.Win.OrderByDescending(op1 => op1.Current)
                                .ThenByDescending(op1 => op1.Power)
                                .First();
                        vf = true;
                    }
                    else if (reservedGameCardsIndexes[i] / 4 < f4.Min() / 4 && reservedGameCardsIndexes[i + 1] / 4 < f4.Min())
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = f4.Max() + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5 });
                        sorted =
                            this.gameDatabase.Win.OrderByDescending(op1 => op1.Current)
                                .ThenByDescending(op1 => op1.Power)
                                .First();
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
                        sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (reservedGameCardsIndexes[i + 1] / 4 == 0 && reservedGameCardsIndexes[i + 1] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        currentPlayer.Type = 5.5;
                        currentPlayer.Power = 13 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5.5 });
                        sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (f2.Length > 0)
                {
                    if (reservedGameCardsIndexes[i] / 4 == 0 && reservedGameCardsIndexes[i] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        currentPlayer.Type = 5.5;
                        currentPlayer.Power = 13 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5.5 });
                        sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (reservedGameCardsIndexes[i + 1] / 4 == 0 && reservedGameCardsIndexes[i + 1] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        currentPlayer.Type = 5.5;
                        currentPlayer.Power = 13 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5.5 });
                        sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (f3.Length > 0)
                {
                    if (reservedGameCardsIndexes[i] / 4 == 0 && reservedGameCardsIndexes[i] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        currentPlayer.Type = 5.5;
                        currentPlayer.Power = 13 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5.5 });
                        sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (reservedGameCardsIndexes[i + 1] / 4 == 0 && reservedGameCardsIndexes[i + 1] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        currentPlayer.Type = 5.5;
                        currentPlayer.Power = 13 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5.5 });
                        sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (f4.Length > 0)
                {
                    if (reservedGameCardsIndexes[i] / 4 == 0 && reservedGameCardsIndexes[i] % 4 == f4[0] % 4 && vf && f4.Length > 0)
                    {
                        currentPlayer.Type = 5.5;
                        currentPlayer.Power = 13 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5.5 });
                        sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (reservedGameCardsIndexes[i + 1] / 4 == 0 && reservedGameCardsIndexes[i + 1] % 4 == f4[0] % 4 && vf)
                    {
                        currentPlayer.Type = 5.5;
                        currentPlayer.Power = 13 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 5.5 });
                        sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                            sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        }
                        else
                        {
                            currentPlayer.Type = 4;
                            currentPlayer.Power = op[j + 4] + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 4 });
                            sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        }
                    }

                    if (op[j] == 0 && op[j + 1] == 9 && op[j + 2] == 10 && op[j + 3] == 11 && op[j + 4] == 12)
                    {
                        currentPlayer.Type = 4;
                        currentPlayer.Power = 13 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 4 });
                        sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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

                    if (fh.Length != 3) continue;

                    if (fh.Max() / 4 == 0)
                    {
                        currentPlayer.Type = 3;
                        currentPlayer.Power = 13 * 3 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 3 });
                        sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                    }
                    else
                    {
                        currentPlayer.Type = 3;
                        currentPlayer.Power = fh[0] / 4 + fh[1] / 4 + fh[2] / 4 + currentPlayer.Type * 100;
                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 3 });
                        sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                    }
                }
            }
        }

        private void RTwoPair(ICharacter currentPlayer) //ref double current, ref double power
        {
            if (!(currentPlayer.Type >= -1)) return;

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
                                        sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                    }

                                    if (reservedGameCardsIndexes[i + 1] / 4 == 0)
                                    {
                                        currentPlayer.Type = 2;
                                        currentPlayer.Power = 13 * 4 + (reservedGameCardsIndexes[i] / 4) * 2 + currentPlayer.Type * 100;
                                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 2 });
                                        sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                    }

                                    if (reservedGameCardsIndexes[i + 1] / 4 != 0 && reservedGameCardsIndexes[i] / 4 != 0)
                                    {
                                        currentPlayer.Type = 2;
                                        currentPlayer.Power = (reservedGameCardsIndexes[i] / 4) * 2 + (reservedGameCardsIndexes[i + 1] / 4) * 2 + currentPlayer.Type * 100;
                                        this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 2 });
                                        sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                    }
                                }
                                msgbox = true;
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
                                            sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }

                                        if (reservedGameCardsIndexes[i] / 4 == 0)
                                        {
                                            currentPlayer.Type = 2;
                                            currentPlayer.Power = (reservedGameCardsIndexes[i + 1] / 4) * 2 + 13 * 4 + currentPlayer.Type * 100;
                                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 2 });
                                            sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }

                                        if (reservedGameCardsIndexes[i + 1] / 4 != 0)
                                        {
                                            currentPlayer.Type = 2;
                                            currentPlayer.Power = (reservedGameCardsIndexes[tc] / 4) * 2 + (reservedGameCardsIndexes[i + 1] / 4) * 2 + currentPlayer.Type * 100;
                                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 2 });
                                            sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }

                                        if (reservedGameCardsIndexes[i] / 4 != 0)
                                        {
                                            currentPlayer.Type = 2;
                                            currentPlayer.Power = (reservedGameCardsIndexes[tc] / 4) * 2 + (reservedGameCardsIndexes[i] / 4) * 2 + currentPlayer.Type * 100;
                                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 2 });
                                            sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
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
                                                sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                            else
                                            {
                                                currentPlayer.Type = 0;
                                                currentPlayer.Power = reservedGameCardsIndexes[tc] / 4 + reservedGameCardsIndexes[i] / 4 + currentPlayer.Type * 100;
                                                this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 1 });
                                                sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                        }
                                        else
                                        {
                                            if (reservedGameCardsIndexes[tc] / 4 == 0)
                                            {
                                                currentPlayer.Type = 0;
                                                currentPlayer.Power = 13 + reservedGameCardsIndexes[i + 1] + currentPlayer.Type * 100;
                                                this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 1 });
                                                sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                            else
                                            {
                                                currentPlayer.Type = 0;
                                                currentPlayer.Power = reservedGameCardsIndexes[tc] / 4 + reservedGameCardsIndexes[i + 1] / 4 + currentPlayer.Type * 100;
                                                this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 1 });
                                                sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
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
                            sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                        else
                        {
                            currentPlayer.Type = 1;
                            currentPlayer.Power = (reservedGameCardsIndexes[i + 1] / 4) * 4 + currentPlayer.Type * 100;
                            this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 1 });
                            sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
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
                                sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                            else
                            {
                                currentPlayer.Type = 1;
                                currentPlayer.Power = (reservedGameCardsIndexes[i + 1] / 4) * 4 + reservedGameCardsIndexes[i] / 4 + currentPlayer.Type * 100;
                                this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 1 });
                                sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
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
                                sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                            else
                            {
                                currentPlayer.Type = 1;
                                currentPlayer.Power = (reservedGameCardsIndexes[tc] / 4) * 4 + reservedGameCardsIndexes[i + 1] / 4 + currentPlayer.Type * 100;
                                this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = 1 });
                                sorted = this.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
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
                    sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
                else
                {
                    currentPlayer.Type = -1;
                    currentPlayer.Power = reservedGameCardsIndexes[i + 1] / 4;
                    this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = -1 });
                    sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }

                if (reservedGameCardsIndexes[i] / 4 == 0 || reservedGameCardsIndexes[i + 1] / 4 == 0)
                {
                    currentPlayer.Type = -1;
                    currentPlayer.Power = 13;
                    this.gameDatabase.Win.Add(new Type() { Power = currentPlayer.Power, Current = -1 });
                    sorted = this.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
            }
        }

        void Winner(ICharacter currentPlayer, string lastFixed) //this.player.Type, this.player.Power, "Player", this.player.Chips, fixedLast
        {
            if (lastFixed == " ")
            {
                lastFixed = "Bot 5";
            }

            for (int j = 0; j <= 16; j++)
            {
                //await Task.Delay(5);
                if (cardsPictureBoxArray[j].Visible)
                    cardsPictureBoxArray[j].Image = gameCardsAsImages[j];
            }

            if (currentPlayer.Type == sorted.Current)
            {
                if (currentPlayer.Power == sorted.Power)
                {
                    winners++;
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

            if (currentPlayer.Name == lastFixed)
            {
                if (winners > 1)
                {
                    if (this.gameDatabase.CheckWinners.Contains(this.player.Name))
                    {
                        this.player.Chips += int.Parse(textBoxPot.Text) / winners;
                        textBoxPlayerChips.Text = this.player.ToString();
                        //pPanel.Visible = true;

                    }

                    if (this.gameDatabase.CheckWinners.Contains(this.bots[0].Name))
                    {
                        this.bots[0].Chips += int.Parse(textBoxPot.Text) / winners;
                        textBoxBotOneChips.Text = this.bots[0].Chips.ToString();
                        //b1Panel.Visible = true;
                    }

                    if (this.gameDatabase.CheckWinners.Contains(this.bots[1].Name))
                    {
                        this.bots[1].Chips += int.Parse(textBoxPot.Text) / winners;
                        textBoxBotTwoChips.Text = this.bots[1].Chips.ToString();
                        //b2Panel.Visible = true;
                    }

                    if (this.gameDatabase.CheckWinners.Contains(this.bots[2].Name))
                    {
                        this.bots[2].Chips += int.Parse(textBoxPot.Text) / winners;
                        textBoxBotThreeChips.Text = this.bots[2].Chips.ToString();
                        //b3Panel.Visible = true;
                    }

                    if (this.gameDatabase.CheckWinners.Contains(this.bots[3].Name))
                    {
                        this.bots[3].Chips += int.Parse(textBoxPot.Text) / winners;
                        textBoxBotFourChips.Text = this.bots[3].Chips.ToString();
                        //b4Panel.Visible = true;
                    }

                    if (this.gameDatabase.CheckWinners.Contains(this.bots[4].Name))
                    {
                        this.bots[4].Chips += int.Parse(textBoxPot.Text) / winners;
                        textBoxBotFiveChips.Text = this.bots[4].Chips.ToString();
                        //b5Panel.Visible = true;
                    }
                    //await Finish(1);
                }

                if (winners == 1)
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
            if (this.isRaising)
            {
                this.turnCount = 0;
                this.isRaising = false;
                raisedTurn = currentTurn;
                this.isChanged = true;
            }
            else
            {
                if (this.turnCount >= maxLeft - 1 || !this.isChanged && this.turnCount == maxLeft)
                {
                    if (currentTurn == raisedTurn - 1 || !this.isChanged && this.turnCount == maxLeft || raisedTurn == 0 && currentTurn == 5)
                    {
                        this.isChanged = false;
                        this.turnCount = 0;
                        raise = 0;
                        this.neededChipsToCall = 0;
                        raisedTurn = 123;
                        rounds++;
                        if (!this.player.OutOfChips)
                            playerStatus.Text = string.Empty;
                        if (!this.bots[0].OutOfChips)
                            botOneStatus.Text = string.Empty;
                        if (!this.bots[1].OutOfChips)
                            botTwoStatus.Text = string.Empty;
                        if (!this.bots[2].OutOfChips)
                            botThreeStatus.Text = string.Empty;
                        if (!this.bots[3].OutOfChips)
                            botFourStatus.Text = string.Empty;
                        if (!this.bots[4].OutOfChips)
                            botFiveStatus.Text = string.Empty;
                    }
                }
            }
            if (rounds == flop)
            {
                for (int j = 12; j <= 14; j++)
                {
                    if (cardsPictureBoxArray[j].Image != gameCardsAsImages[j])
                    {
                        cardsPictureBoxArray[j].Image = gameCardsAsImages[j];
                        this.player.Call = 0;
                        this.player.Raise = 0;
                        this.bots[0].Call = 0;
                        this.bots[0].Raise = 0;
                        this.bots[1].Call = 0;
                        this.bots[1].Raise = 0;
                        this.bots[2].Call = 0;
                        this.bots[2].Raise = 0;
                        this.bots[3].Call = 0;
                        this.bots[3].Raise = 0;
                        this.bots[4].Call = 0;
                        this.bots[4].Raise = 0;
                    }
                }
            }

            if (rounds == turn)
            {
                for (int j = 14; j <= 15; j++)
                {
                    if (cardsPictureBoxArray[j].Image != gameCardsAsImages[j])
                    {
                        cardsPictureBoxArray[j].Image = gameCardsAsImages[j];
                        this.player.Call = 0;
                        this.player.Raise = 0;
                        this.bots[0].Call = 0;
                        this.bots[0].Raise = 0;
                        this.bots[1].Call = 0;
                        this.bots[1].Raise = 0;
                        this.bots[2].Call = 0;
                        this.bots[2].Raise = 0;
                        this.bots[3].Call = 0;
                        this.bots[3].Raise = 0;
                        this.bots[4].Call = 0;
                        this.bots[4].Raise = 0;
                    }
                }
            }

            if (rounds == river)
            {
                for (int j = 15; j <= 16; j++)
                {
                    if (cardsPictureBoxArray[j].Image != gameCardsAsImages[j])
                    {
                        cardsPictureBoxArray[j].Image = gameCardsAsImages[j];
                        this.player.Call = 0;
                        this.player.Raise = 0;
                        this.bots[0].Call = 0;
                        this.bots[0].Raise = 0;
                        this.bots[1].Call = 0;
                        this.bots[1].Raise = 0;
                        this.bots[2].Call = 0;
                        this.bots[2].Raise = 0;
                        this.bots[3].Call = 0;
                        this.bots[3].Raise = 0;
                        this.bots[4].Call = 0;
                        this.bots[4].Raise = 0;
                    }
                }
            }

            if (rounds == end && maxLeft == 6)
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
                isRestarted = true;
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

                this.player.Panel.Visible = false;
                this.bots[0].Panel.Visible = false;
                this.bots[1].Panel.Visible = false;
                this.bots[2].Panel.Visible = false;
                this.bots[3].Panel.Visible = false;
                this.bots[4].Panel.Visible = false;

                this.player.Call = 0;
                this.player.Raise = 0;
                this.bots[0].Call = 0;
                this.bots[0].Raise = 0;
                this.bots[1].Call = 0;
                this.bots[1].Raise = 0;
                this.bots[2].Call = 0;
                this.bots[2].Raise = 0;
                this.bots[3].Call = 0;
                this.bots[3].Raise = 0;
                this.bots[4].Call = 0;
                this.bots[4].Raise = 0;
                last = 0;
                this.neededChipsToCall = this.bigBlindValue;
                raise = 0;
                cardsImageLocation = Directory.GetFiles("..\\..\\Resources\\Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
                this.gameDatabase.PlayersGameStatus.Clear();
                rounds = 0;
                this.player.Power = 0;
                this.player.Type = -1;
                type = 0;
                this.bots[0].Power = 0;
                this.bots[1].Power = 0;
                this.bots[2].Power = 0;
                this.bots[3].Power = 0;
                this.bots[4].Power = 0;
                this.bots[0].Type = -1;
                this.bots[1].Type = -1;
                this.bots[2].Type = -1;
                this.bots[3].Type = -1;
                this.bots[4].Type = -1;
                this.gameDatabase.Chips.Clear();
                this.gameDatabase.CheckWinners.Clear();
                winners = 0;
                this.gameDatabase.Win.Clear();
                sorted.Current = 0;
                sorted.Power = 0;
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

        public void FixCall(Label status, ICharacter currentPlayer, int options) //ref int cCall, ref int cRaise
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

            if (this.gameDatabase.Chips.ToArray().Length == maxLeft)
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
            if (abc < 6 && abc > 1 && rounds >= end)
            {
                await Finish(2);
            }
            #endregion


        }

        public async Task Finish(int n)
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

            isRestarted = false;
            this.isRaising = false;

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

            height = 0;
            width = 0;
            winners = 0;
            flop = 1;
            turn = 2;
            river = 3;
            end = 4;
            maxLeft = 6;
            last = 123;
            raisedTurn = 1;
            this.gameDatabase.PlayersGameStatus.Clear();
            this.gameDatabase.CheckWinners.Clear();
            this.gameDatabase.Chips.Clear();
            this.gameDatabase.Win.Clear();
            sorted.Current = 0;
            sorted.Power = 0;
            textBoxPot.Text = "0";
            this.secondsLeft = 60;
            up = 10000000;
            this.turnCount = 0;
            playerStatus.Text = string.Empty;
            botOneStatus.Text = string.Empty;
            botTwoStatus.Text = string.Empty;
            botThreeStatus.Text = string.Empty;
            botFourStatus.Text = string.Empty;
            botFiveStatus.Text = string.Empty;
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

        private void FixWinners()
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
                    this.winningHandType.HighCard(player, sStatus, ref this.neededChipsToCall, this.textBoxPot, ref this.raise, ref this.isRaising);
                }

                if (player.Type == 0)
                {
                    this.winningHandType.PairTable(player, sStatus, ref this.neededChipsToCall, this.textBoxPot, ref this.raise, ref this.isRaising);
                }

                if (player.Type == 1)
                {
                    this.winningHandType.PairHand(player, sStatus, ref this.neededChipsToCall, this.textBoxPot, ref this.raise, ref this.isRaising, ref this.rounds);
                }

                if (player.Type == 2)
                {
                    this.winningHandType.TwoPair(player, sStatus, ref this.neededChipsToCall, this.textBoxPot, ref this.raise, ref this.isRaising, ref this.rounds);
                }

                if (player.Type == 3)
                {
                    this.winningHandType.ThreeOfAKind(player, sStatus, name, ref this.neededChipsToCall, this.textBoxPot, ref this.raise, ref this.isRaising, ref this.rounds);
                }

                if (player.Type == 4)
                {
                    this.winningHandType.Straight(player, sStatus, name, ref this.neededChipsToCall, this.textBoxPot, ref this.raise, ref this.isRaising, ref this.rounds);
                }

                if (player.Type == 5 || player.Type == 5.5)
                {
                    this.winningHandType.Flush(player, sStatus, name, ref this.neededChipsToCall, this.textBoxPot, ref this.raise, ref this.isRaising, ref this.rounds);
                }

                if (player.Type == 6)
                {
                    this.winningHandType.FullHouse(player, sStatus, name, ref this.neededChipsToCall, this.textBoxPot, ref this.raise, ref this.isRaising, ref this.rounds);
                }

                if (player.Type == 7)
                {
                    this.winningHandType.FourOfAKind(player, sStatus, name, ref this.neededChipsToCall, this.textBoxPot, ref this.raise, ref this.isRaising, ref this.rounds);
                }

                if (player.Type == 8 || player.Type == 9)
                {
                    this.winningHandType.StraightFlush(player, sStatus, name, ref this.neededChipsToCall, this.textBoxPot, ref this.raise, ref this.isRaising, ref this.rounds);
                }
            }

            if (player.OutOfChips)
            {
                this.cardsPictureBoxArray[card1].Visible = false;
                this.cardsPictureBoxArray[card2].Visible = false;
            }
        }

        /* // otiva v WinningHandType
        private void HighCard(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower)
        {
            Hp(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower, 20, 25);
        }
        private void PairTable(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower)
        {
            Hp(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower, 16, 25);
        }
        private void PairHand(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower)
        {
            Random rPair = new Random();
            int rCall = rPair.Next(10, 16);
            int rRaise = rPair.Next(10, 13);
            if (botPower <= 199 && botPower >= 140)
            {
                Ph(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 6, rRaise);
            }
            if (botPower <= 139 && botPower >= 128)
            {
                Ph(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 7, rRaise);
            }
            if (botPower < 128 && botPower >= 101)
            {
                Ph(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 9, rRaise);
            }
        }
        private void TwoPair(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower)
        {
            Random rPair = new Random();
            int rCall = rPair.Next(6, 11);
            int rRaise = rPair.Next(6, 11);
            if (botPower <= 290 && botPower >= 246)
            {
                Ph(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 3, rRaise);
            }
            if (botPower <= 244 && botPower >= 234)
            {
                Ph(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 4, rRaise);
            }
            if (botPower < 234 && botPower >= 201)
            {
                Ph(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 4, rRaise);
            }
        }
        private void ThreeOfAKind(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower)
        {
            Random tk = new Random();
            int tCall = tk.Next(3, 7);
            int tRaise = tk.Next(4, 8);
            if (botPower <= 390 && botPower >= 330)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, tCall, tRaise);
            }
            if (botPower <= 327 && botPower >= 321)//10  8
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, tCall, tRaise);
            }
            if (botPower < 321 && botPower >= 303)//7 2
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, tCall, tRaise);
            }
        }
        private void Straight(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower)
        {
            Random str = new Random();
            int sCall = str.Next(3, 6);
            int sRaise = str.Next(3, 8);
            if (botPower <= 480 && botPower >= 410)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, sCall, sRaise);
            }
            if (botPower <= 409 && botPower >= 407)//10  8
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, sCall, sRaise);
            }
            if (botPower < 407 && botPower >= 404)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, sCall, sRaise);
            }
        }
        private void Flush(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower)
        {
            Random fsh = new Random();
            int fCall = fsh.Next(2, 6);
            int fRaise = fsh.Next(3, 7);
            Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, fCall, fRaise);
        }
        private void FullHouse(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower)
        {
            Random flh = new Random();
            int fhCall = flh.Next(1, 5);
            int fhRaise = flh.Next(2, 6);
            if (botPower <= 626 && botPower >= 620)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, fhCall, fhRaise);
            }
            if (botPower < 620 && botPower >= 602)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, fhCall, fhRaise);
            }
        }
        private void FourOfAKind(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower)
        {
            Random fk = new Random();
            int fkCall = fk.Next(1, 4);
            int fkRaise = fk.Next(2, 5);
            if (botPower <= 752 && botPower >= 704)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, fkCall, fkRaise);
            }
        }
        private void StraightFlush(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower)
        {
            Random sf = new Random();
            int sfCall = sf.Next(1, 3);
            int sfRaise = sf.Next(1, 3);
            if (botPower <= 913 && botPower >= 804)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, sfCall, sfRaise);
            }
        }
        */

        /* otiva v PlayerMove
        private void Fold(ref bool sTurn, ref bool sFTurn, Label sStatus)
        {
            this.isRaising = false;
            sStatus.Text = "Fold";
            sTurn = false;
            sFTurn = true;
        }
        private void Check(ref bool cTurn, Label cStatus)
        {
            cStatus.Text = "Check";
            cTurn = false;
            this.isRaising = false;
        }
        private void Call(ref int sChips, ref bool sTurn, Label sStatus)
        {
            this.isRaising = false;
            sTurn = false;
            sChips -= this.neededChipsToCall;
            sStatus.Text = "Call " + this.neededChipsToCall;
            textBoxPot.Text = (int.Parse(textBoxPot.Text) + this.neededChipsToCall).ToString();
        }
        private void Raised(ref int sChips, ref bool sTurn, Label sStatus)
        {
            sChips -= Convert.ToInt32(raise);
            sStatus.Text = "Raise " + raise;
            textBoxPot.Text = (int.Parse(textBoxPot.Text) + Convert.ToInt32(raise)).ToString();
            this.neededChipsToCall = Convert.ToInt32(raise);
            this.isRaising = true;
            sTurn = false;
        }
        private static double RoundN(int sChips, int n)
        {
            double a = Math.Round((sChips / n) / 100d, 0) * 100;
            return a;
        }
        */
        /* // otiva v WinningHandType
        private void Hp(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower, int n, int n1)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 4);
            if (this.neededChipsToCall <= 0)
            {
                Check(ref sTurn, sStatus);
            }
            if (this.neededChipsToCall > 0)
            {
                if (rnd == 1)
                {
                    if (this.neededChipsToCall <= RoundN(sChips, n))
                    {
                        Call(ref sChips, ref sTurn, sStatus);
                    }
                    else
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                }
                if (rnd == 2)
                {
                    if (this.neededChipsToCall <= RoundN(sChips, n1))
                    {
                        Call(ref sChips, ref sTurn, sStatus);
                    }
                    else
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                }
            }
            if (rnd == 3)
            {
                if (raise == 0)
                {
                    raise = this.neededChipsToCall * 2;
                    Raised(ref sChips, ref sTurn, sStatus);
                }
                else
                {
                    if (raise <= RoundN(sChips, n))
                    {
                        raise = this.neededChipsToCall * 2;
                        Raised(ref sChips, ref sTurn, sStatus);
                    }
                    else
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                }
            }
            if (sChips <= 0)
            {
                sFTurn = true;
            }
        }
        private void Ph(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int n, int n1, int r)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 3);
            if (rounds < 2)
            {
                if (this.neededChipsToCall <= 0)
                {
                    Check(ref sTurn, sStatus);
                }
                if (this.neededChipsToCall > 0)
                {
                    if (this.neededChipsToCall >= RoundN(sChips, n1))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                    if (raise > RoundN(sChips, n))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                    if (!sFTurn)
                    {
                        if (this.neededChipsToCall >= RoundN(sChips, n) && this.neededChipsToCall <= RoundN(sChips, n1))
                        {
                            Call(ref sChips, ref sTurn, sStatus);
                        }
                        if (raise <= RoundN(sChips, n) && raise >= (RoundN(sChips, n)) / 2)
                        {
                            Call(ref sChips, ref sTurn, sStatus);
                        }
                        if (raise <= (RoundN(sChips, n)) / 2)
                        {
                            if (raise > 0)
                            {
                                raise = (int)RoundN(sChips, n);
                                Raised(ref sChips, ref sTurn, sStatus);
                            }
                            else
                            {
                                raise = this.neededChipsToCall * 2;
                                Raised(ref sChips, ref sTurn, sStatus);
                            }
                        }

                    }
                }
            }
            if (rounds >= 2)
            {
                if (this.neededChipsToCall > 0)
                {
                    if (this.neededChipsToCall >= RoundN(sChips, n1 - rnd))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                    if (raise > RoundN(sChips, n - rnd))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                    if (!sFTurn)
                    {
                        if (this.neededChipsToCall >= RoundN(sChips, n - rnd) && this.neededChipsToCall <= RoundN(sChips, n1 - rnd))
                        {
                            Call(ref sChips, ref sTurn, sStatus);
                        }
                        if (raise <= RoundN(sChips, n - rnd) && raise >= (RoundN(sChips, n - rnd)) / 2)
                        {
                            Call(ref sChips, ref sTurn, sStatus);
                        }
                        if (raise <= (RoundN(sChips, n - rnd)) / 2)
                        {
                            if (raise > 0)
                            {
                                raise = (int)RoundN(sChips, n - rnd);
                                Raised(ref sChips, ref sTurn, sStatus);
                            }
                            else
                            {
                                raise = this.neededChipsToCall * 2;
                                Raised(ref sChips, ref sTurn, sStatus);
                            }
                        }
                    }
                }
                if (this.neededChipsToCall <= 0)
                {
                    raise = (int)RoundN(sChips, r - rnd);
                    Raised(ref sChips, ref sTurn, sStatus);
                }
            }

            if (sChips <= 0)
            {
                sFTurn = true;
            }
        }

        void Smooth(ref int botChips, ref bool botTurn, ref bool botFTurn, Label botStatus, int name, int n, int r)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 3);
            if (this.neededChipsToCall <= 0)
            {
                Check(ref botTurn, botStatus);
            }
            else
            {
                if (this.neededChipsToCall >= RoundN(botChips, n))
                {
                    if (botChips > this.neededChipsToCall)
                    {
                        Call(ref botChips, ref botTurn, botStatus);
                    }
                    else if (botChips <= this.neededChipsToCall)
                    {
                        this.isRaising = false;
                        botTurn = false;
                        botChips = 0;
                        botStatus.Text = "Call " + botChips;
                        textBoxPot.Text = (int.Parse(textBoxPot.Text) + botChips).ToString();
                    }
                }
                else
                {
                    if (raise > 0)
                    {
                        if (botChips >= raise * 2)
                        {
                            raise *= 2;
                            Raised(ref botChips, ref botTurn, botStatus);
                        }
                        else
                        {
                            Call(ref botChips, ref botTurn, botStatus);
                        }
                    }
                    else
                    {
                        raise = this.neededChipsToCall * 2;
                        Raised(ref botChips, ref botTurn, botStatus);
                    }
                }
            }
            if (botChips <= 0)
            {
                botFTurn = true;
            }
        }
        */

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
                this.textBoxPlayerChips.Text = "Chips : 0";
            }

            if (this.bots[0].Chips <= 0)
            {
                this.textBoxBotOneChips.Text = "Chips : 0";
            }

            if (this.bots[1].Chips <= 0)
            {
                this.textBoxBotTwoChips.Text = "Chips : 0";
            }

            if (this.bots[2].Chips <= 0)
            {
                this.textBoxBotThreeChips.Text = "Chips : 0";
            }

            if (this.bots[3].Chips <= 0)
            {
                this.textBoxBotFourChips.Text = "Chips : 0";
            }

            if (this.bots[4].Chips <= 0)
            {
                this.textBoxBotFiveChips.Text = "Chips : 0";
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
                ////playerStatus.Text = "All in " + Chips;

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
                this.textBoxPlayerChips.Text = "Chips : " + this.player.Chips.ToString();
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
                this.textBoxPlayerChips.Text = "Chips : " + this.player.Chips.ToString();
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
                            this.isRaising = true;
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
                            this.isRaising = true;
                            this.last = 0;
                            this.player.Raise = Convert.ToInt32(this.raise);
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
                this.bots[0].Chips += int.Parse(this.textBoxAdd.Text);
                this.bots[1].Chips += int.Parse(this.textBoxAdd.Text);
                this.bots[2].Chips += int.Parse(this.textBoxAdd.Text);
                this.bots[3].Chips += int.Parse(this.textBoxAdd.Text);
                this.bots[4].Chips += int.Parse(this.textBoxAdd.Text);
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
                MessageBox.Show("The Small Blind can be only round number !");
                this.textBoxSmallBlind.Text = this.smallBlindValue.ToString();
                return;
            }

            if (!int.TryParse(this.textBoxSmallBlind.Text, out parsedValue))
            {
                MessageBox.Show("This is a number only field");
                this.textBoxSmallBlind.Text = this.smallBlindValue.ToString();
                return;
            }

            if (int.Parse(this.textBoxSmallBlind.Text) > 100000)
            {
                MessageBox.Show("The maximum of the Small Blind is 100 000 $");
                this.textBoxSmallBlind.Text = this.smallBlindValue.ToString();
            }

            if (int.Parse(this.textBoxSmallBlind.Text) < 250)
            {
                MessageBox.Show("The minimum of the Small Blind is 250 $");
            }

            if (int.Parse(this.textBoxSmallBlind.Text) >= 250 && int.Parse(this.textBoxSmallBlind.Text) <= 100000)
            {
                this.smallBlindValue = int.Parse(this.textBoxSmallBlind.Text);
                MessageBox.Show("The changes have been saved ! They will become available the next hand you play. ");
            }
        }

        private void bBB_Click(object sender, EventArgs e)
        {
            int parsedValue;
            if (this.textBoxBigBlind.Text.Contains(",") || this.textBoxBigBlind.Text.Contains("."))
            {
                MessageBox.Show("The Big Blind can be only round number !");
                this.textBoxBigBlind.Text = this.bigBlindValue.ToString();

                return;
            }

            if (!int.TryParse(this.textBoxSmallBlind.Text, out parsedValue))
            {
                MessageBox.Show("This is a number only field");
                this.textBoxSmallBlind.Text = this.bigBlindValue.ToString();

                return;
            }

            if (int.Parse(this.textBoxBigBlind.Text) > 200000)
            {
                MessageBox.Show("The maximum of the Big Blind is 200 000");
                this.textBoxBigBlind.Text = this.bigBlindValue.ToString();
            }

            if (int.Parse(this.textBoxBigBlind.Text) < 500)
            {
                MessageBox.Show("The minimum of the Big Blind is 500 $");
            }

            if (int.Parse(this.textBoxBigBlind.Text) >= 500 && int.Parse(this.textBoxBigBlind.Text) <= 200000)
            {
                this.bigBlindValue = int.Parse(this.textBoxBigBlind.Text);
                MessageBox.Show("The changes have been saved ! They will become available the next hand you play. ");
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
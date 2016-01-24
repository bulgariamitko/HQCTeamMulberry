namespace Poker
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public partial class MainWindow : Form
    {
        #region Variables
        private ProgressBar progressBar = new ProgressBar();

        private const int DefaultStartingChips = 10000;

        private int Nm;

        //Players panels
        private Panel playerPanel = new Panel();
        private Panel bot1Panel = new Panel();
        private Panel bot2Panel = new Panel();
        private Panel bot3Panel = new Panel();
        private Panel bot4Panel = new Panel();
        private Panel bot5Panel = new Panel();


        int call = 500, foldedPlayers = 5;

        //Chips
        public int Chips = DefaultStartingChips;
        public int Bot1Chips = DefaultStartingChips;
        public int Bot2Chips = DefaultStartingChips;
        public int Bot3Chips = DefaultStartingChips;
        public int Bot4Chips = DefaultStartingChips;
        public int Bot5Chips = DefaultStartingChips;

        private double type;
        private double rounds = 0;
        private double botOnePower;
        private double botTwoPower;
        private double botThreePower;
        private double botFourPower;
        private double botFivePower;
        private double playerPower = 0;

        private double playerType = -1;
        private double raise = 0;
        private double botOneType = -1;
        private double botTwoType = -1;
        private double botThreeType = -1;
        private double botFourType = -1;
        private double botFiveType = -1;

        private bool botOneTurn = false;
        private bool botTwoTurn = false;
        private bool botThreeTurn = false;
        private bool botFourTurn = false;
        private bool botFiveTurn = false;

        private bool _b1Fturn = false, _b2Fturn = false, _b3Fturn = false, _b4Fturn = false, _b5Fturn = false;
        private bool _pFolded, _b1Folded, _b2Folded, _b3Folded, _b4Folded, _b5Folded, intsadded, _changed;
        private int _pCall = 0, _b1Call = 0, _b2Call = 0, _b3Call = 0, _b4Call = 0, _b5Call = 0, _pRaise = 0, _b1Raise = 0, _b2Raise = 0, _b3Raise = 0, _b4Raise = 0, _b5Raise = 0;
        private int _height, _width, _winners = 0, _flop = 1, _turn = 2, _river = 3, _end = 4, _maxLeft = 6;
        private int _last = 123, _raisedTurn = 1;
        private List<bool?> _bools = new List<bool?>();
        private List<Type> _win = new List<Type>();
        private List<string> _checkWinners = new List<string>();
        private List<int> ints = new List<int>();
        private bool _pFturn = false, _pturn = true, _restart = false, _raising = false;
        private Poker.Type _sorted;
        private string[] imgLocation = Directory.GetFiles("..\\..\\Resources\\Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
        /*string[] ImgLocation ={
                   "Assets\\Cards\\33.png","Assets\\Cards\\22.png",
                    "Assets\\Cards\\29.png","Assets\\Cards\\21.png",
                    "Assets\\Cards\\36.png","Assets\\Cards\\17.png",
                    "Assets\\Cards\\40.png","Assets\\Cards\\16.png",
                    "Assets\\Cards\\5.png","Assets\\Cards\\47.png",
                    "Assets\\Cards\\37.png","Assets\\Cards\\13.png",
                    
                    "Assets\\Cards\\12.png",
                    "Assets\\Cards\\8.png","Assets\\Cards\\18.png",
                    "Assets\\Cards\\15.png","Assets\\Cards\\27.png"};*/
        private int[] _reserve = new int[17];
        private Image[] _deck = new Image[52];
        private PictureBox[] _holder = new PictureBox[52];
        private Timer _timer = new Timer();
        private Timer _updates = new Timer();
        private int _t = 60, i, bb = 500, _sb = 250, _up = 10000000, _turnCount = 0;

        #endregion
        public MainWindow()
        {
            //bools.Add(PFturn); bools.Add(B1Fturn); bools.Add(B2Fturn); bools.Add(B3Fturn); bools.Add(B4Fturn); bools.Add(B5Fturn);
            call = bb;
            MaximizeBox = false;
            MinimizeBox = false;
            _updates.Start();
            InitializeComponent();
            _width = this.Width;
            _height = this.Height;
            Shuffle();
            tbPot.Enabled = false;
            tbChips.Enabled = false;
            tbBotChips1.Enabled = false;
            tbBotChips2.Enabled = false;
            tbBotChips3.Enabled = false;
            tbBotChips4.Enabled = false;
            tbBotChips5.Enabled = false;
            tbChips.Text = "Chips : " + Chips.ToString();
            tbBotChips1.Text = "Chips : " + Bot1Chips.ToString();
            tbBotChips2.Text = "Chips : " + Bot2Chips.ToString();
            tbBotChips3.Text = "Chips : " + Bot3Chips.ToString();
            tbBotChips4.Text = "Chips : " + Bot4Chips.ToString();
            tbBotChips5.Text = "Chips : " + Bot5Chips.ToString();
            _timer.Interval = (1 * 1 * 1000);
            _timer.Tick += TimerTick;
            _updates.Interval = (1 * 1 * 100);
            _updates.Tick += UpdateTick;
            tbBB.Visible = true;
            tbSB.Visible = true;
            bBB.Visible = true;
            bSB.Visible = true;
            tbBB.Visible = true;
            tbSB.Visible = true;
            bBB.Visible = true;
            bSB.Visible = true;
            tbBB.Visible = false;
            tbSB.Visible = false;
            bBB.Visible = false;
            bSB.Visible = false;
            tbRaise.Text = (bb * 2).ToString();
        }

        async Task Shuffle()
        {
            _bools.Add(_pFturn); _bools.Add(_b1Fturn); _bools.Add(_b2Fturn); _bools.Add(_b3Fturn); _bools.Add(_b4Fturn); _bools.Add(_b5Fturn);
            bCall.Enabled = false;
            bRaise.Enabled = false;
            bFold.Enabled = false;
            bCheck.Enabled = false;
            MaximizeBox = false;
            MinimizeBox = false;
            bool check = false;
            Bitmap backImage = new Bitmap("..\\..\\Resources\\Assets\\Back\\Back.png");
            int horizontal = 580, vertical = 480;
            Random r = new Random();
            for (i = imgLocation.Length; i > 0; i--)
            {
                int j = r.Next(i);
                var k = imgLocation[j];
                imgLocation[j] = imgLocation[i - 1];
                imgLocation[i - 1] = k;
            }

            for (i = 0; i < 17; i++)
            {
                _deck[i] = Image.FromFile(imgLocation[i]);
                var charsToRemove = new string[] { "..\\..\\Resources\\Assets\\Cards\\", ".png" };
                foreach (var c in charsToRemove)
                {
                    imgLocation[i] = imgLocation[i].Replace(c, string.Empty);
                }

                _reserve[i] = int.Parse(imgLocation[i]) - 1;
                _holder[i] = new PictureBox();
                _holder[i].SizeMode = PictureBoxSizeMode.StretchImage;
                _holder[i].Height = 130;
                _holder[i].Width = 80;
                this.Controls.Add(_holder[i]);
                _holder[i].Name = "pb" + i.ToString();
                await Task.Delay(200);

                #region Throwing Cards
                if (i < 2)
                {
                    if (_holder[0].Tag != null)
                    {
                        _holder[1].Tag = _reserve[1];
                    }

                    _holder[0].Tag = _reserve[0];
                    _holder[i].Image = _deck[i];
                    _holder[i].Anchor = (AnchorStyles.Bottom);
                    //Holder[i].Dock = DockStyle.Top;
                    _holder[i].Location = new Point(horizontal, vertical);
                    horizontal += _holder[i].Width;
                    this.Controls.Add(playerPanel);
                    playerPanel.Location = new Point(_holder[0].Left - 10, _holder[0].Top - 10);
                    playerPanel.BackColor = Color.DarkBlue;
                    playerPanel.Height = 150;
                    playerPanel.Width = 180;
                    playerPanel.Visible = false;
                }

                if (Bot1Chips > 0)
                {
                    foldedPlayers--;
                    if (i >= 2 && i < 4)
                    {
                        if (_holder[2].Tag != null)
                        {
                            _holder[3].Tag = _reserve[3];
                        }

                        _holder[2].Tag = _reserve[2];
                        if (!check)
                        {
                            horizontal = 15;
                            vertical = 420;
                        }

                        check = true;
                        _holder[i].Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
                        _holder[i].Image = backImage;
                        //Holder[i].Image = Deck[i];
                        _holder[i].Location = new Point(horizontal, vertical);
                        horizontal += _holder[i].Width;
                        _holder[i].Visible = true;
                        this.Controls.Add(bot1Panel);
                        bot1Panel.Location = new Point(_holder[2].Left - 10, _holder[2].Top - 10);
                        bot1Panel.BackColor = Color.DarkBlue;
                        bot1Panel.Height = 150;
                        bot1Panel.Width = 180;
                        bot1Panel.Visible = false;
                        if (i == 3)
                        {
                            check = false;
                        }
                    }
                }

                if (Bot2Chips > 0)
                {
                    foldedPlayers--;
                    if (i >= 4 && i < 6)
                    {
                        if (_holder[4].Tag != null)
                        {
                            _holder[5].Tag = _reserve[5];
                        }

                        _holder[4].Tag = _reserve[4];
                        if (!check)
                        {
                            horizontal = 75;
                            vertical = 65;
                        }

                        check = true;
                        _holder[i].Anchor = (AnchorStyles.Top | AnchorStyles.Left);
                        _holder[i].Image = backImage;
                        //Holder[i].Image = Deck[i];
                        _holder[i].Location = new Point(horizontal, vertical);
                        horizontal += _holder[i].Width;
                        _holder[i].Visible = true;
                        this.Controls.Add(bot2Panel);
                        bot2Panel.Location = new Point(_holder[4].Left - 10, _holder[4].Top - 10);
                        bot2Panel.BackColor = Color.DarkBlue;
                        bot2Panel.Height = 150;
                        bot2Panel.Width = 180;
                        bot2Panel.Visible = false;
                        if (i == 5)
                        {
                            check = false;
                        }
                    }
                }

                if (Bot3Chips > 0)
                {
                    foldedPlayers--;
                    if (i >= 6 && i < 8)
                    {
                        if (_holder[6].Tag != null)
                        {
                            _holder[7].Tag = _reserve[7];
                        }

                        _holder[6].Tag = _reserve[6];
                        if (!check)
                        {
                            horizontal = 590;
                            vertical = 25;
                        }

                        check = true;
                        _holder[i].Anchor = (AnchorStyles.Top);
                        _holder[i].Image = backImage;
                        //Holder[i].Image = Deck[i];
                        _holder[i].Location = new Point(horizontal, vertical);
                        horizontal += _holder[i].Width;
                        _holder[i].Visible = true;
                        this.Controls.Add(bot3Panel);
                        bot3Panel.Location = new Point(_holder[6].Left - 10, _holder[6].Top - 10);
                        bot3Panel.BackColor = Color.DarkBlue;
                        bot3Panel.Height = 150;
                        bot3Panel.Width = 180;
                        bot3Panel.Visible = false;
                        if (i == 7)
                        {
                            check = false;
                        }
                    }
                }

                if (Bot4Chips > 0)
                {
                    foldedPlayers--;
                    if (i >= 8 && i < 10)
                    {
                        if (_holder[8].Tag != null)
                        {
                            _holder[9].Tag = _reserve[9];
                        }
                        _holder[8].Tag = _reserve[8];
                        if (!check)
                        {
                            horizontal = 1115;
                            vertical = 65;
                        }
                        check = true;
                        _holder[i].Anchor = (AnchorStyles.Top | AnchorStyles.Right);
                        _holder[i].Image = backImage;
                        //Holder[i].Image = Deck[i];
                        _holder[i].Location = new Point(horizontal, vertical);
                        horizontal += _holder[i].Width;
                        _holder[i].Visible = true;
                        this.Controls.Add(bot4Panel);
                        bot4Panel.Location = new Point(_holder[8].Left - 10, _holder[8].Top - 10);
                        bot4Panel.BackColor = Color.DarkBlue;
                        bot4Panel.Height = 150;
                        bot4Panel.Width = 180;
                        bot4Panel.Visible = false;
                        if (i == 9)
                        {
                            check = false;
                        }
                    }
                }
                if (Bot5Chips > 0)
                {
                    foldedPlayers--;
                    if (i >= 10 && i < 12)
                    {
                        if (_holder[10].Tag != null)
                        {
                            _holder[11].Tag = _reserve[11];
                        }
                        _holder[10].Tag = _reserve[10];
                        if (!check)
                        {
                            horizontal = 1160;
                            vertical = 420;
                        }
                        check = true;
                        _holder[i].Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
                        _holder[i].Image = backImage;
                        //Holder[i].Image = Deck[i];
                        _holder[i].Location = new Point(horizontal, vertical);
                        horizontal += _holder[i].Width;
                        _holder[i].Visible = true;
                        this.Controls.Add(bot5Panel);
                        bot5Panel.Location = new Point(_holder[10].Left - 10, _holder[10].Top - 10);
                        bot5Panel.BackColor = Color.DarkBlue;
                        bot5Panel.Height = 150;
                        bot5Panel.Width = 180;
                        bot5Panel.Visible = false;
                        if (i == 11)
                        {
                            check = false;
                        }
                    }
                }
                if (i >= 12)
                {
                    _holder[12].Tag = _reserve[12];
                    if (i > 12) _holder[13].Tag = _reserve[13];
                    if (i > 13) _holder[14].Tag = _reserve[14];
                    if (i > 14) _holder[15].Tag = _reserve[15];
                    if (i > 15)
                    {
                        _holder[16].Tag = _reserve[16];

                    }
                    if (!check)
                    {
                        horizontal = 410;
                        vertical = 265;
                    }
                    check = true;
                    if (_holder[i] != null)
                    {
                        _holder[i].Anchor = AnchorStyles.None;
                        _holder[i].Image = backImage;
                        //Holder[i].Image = Deck[i];
                        _holder[i].Location = new Point(horizontal, vertical);
                        horizontal += 110;
                    }
                }
                #endregion
                if (Bot1Chips <= 0)
                {
                    _b1Fturn = true;
                    _holder[2].Visible = false;
                    _holder[3].Visible = false;
                }
                else
                {
                    _b1Fturn = false;
                    if (i == 3)
                    {
                        if (_holder[3] != null)
                        {
                            _holder[2].Visible = true;
                            _holder[3].Visible = true;
                        }
                    }
                }
                if (Bot2Chips <= 0)
                {
                    _b2Fturn = true;
                    _holder[4].Visible = false;
                    _holder[5].Visible = false;
                }
                else
                {
                    _b2Fturn = false;
                    if (i == 5)
                    {
                        if (_holder[5] != null)
                        {
                            _holder[4].Visible = true;
                            _holder[5].Visible = true;
                        }
                    }
                }
                if (Bot3Chips <= 0)
                {
                    _b3Fturn = true;
                    _holder[6].Visible = false;
                    _holder[7].Visible = false;
                }
                else
                {
                    _b3Fturn = false;
                    if (i == 7)
                    {
                        if (_holder[7] != null)
                        {
                            _holder[6].Visible = true;
                            _holder[7].Visible = true;
                        }
                    }
                }
                if (Bot4Chips <= 0)
                {
                    _b4Fturn = true;
                    _holder[8].Visible = false;
                    _holder[9].Visible = false;
                }
                else
                {
                    _b4Fturn = false;
                    if (i == 9)
                    {
                        if (_holder[9] != null)
                        {
                            _holder[8].Visible = true;
                            _holder[9].Visible = true;
                        }
                    }
                }
                if (Bot5Chips <= 0)
                {
                    _b5Fturn = true;
                    _holder[10].Visible = false;
                    _holder[11].Visible = false;
                }
                else
                {
                    _b5Fturn = false;
                    if (i == 11)
                    {
                        if (_holder[11] != null)
                        {
                            _holder[10].Visible = true;
                            _holder[11].Visible = true;
                        }
                    }
                }
                if (i == 16)
                {
                    if (!_restart)
                    {
                        MaximizeBox = true;
                        MinimizeBox = true;
                    }
                    _timer.Start();
                }
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
                bRaise.Enabled = true;
                bCall.Enabled = true;
                bRaise.Enabled = true;
                bRaise.Enabled = true;
                bFold.Enabled = true;
            }
        }
        async Task Turns()
        {
            #region Rotating
            if (!_pFturn)
            {
                if (_pturn)
                {
                    FixCall(pStatus, ref _pCall, ref _pRaise, 1);
                    //MessageBox.Show("Player's Turn");
                    pbTimer.Visible = true;
                    pbTimer.Value = 1000;
                    _t = 60;
                    _up = 10000000;
                    _timer.Start();
                    bRaise.Enabled = true;
                    bCall.Enabled = true;
                    bRaise.Enabled = true;
                    bRaise.Enabled = true;
                    bFold.Enabled = true;
                    _turnCount++;
                    FixCall(pStatus, ref _pCall, ref _pRaise, 2);
                }
            }
            if (_pFturn || !_pturn)
            {
                await AllIn();
                if (_pFturn && !_pFolded)
                {
                    if (bCall.Text.Contains("All in") == false || bRaise.Text.Contains("All in") == false)
                    {
                        _bools.RemoveAt(0);
                        _bools.Insert(0, null);
                        _maxLeft--;
                        _pFolded = true;
                    }
                }
                await CheckRaise(0, 0);
                pbTimer.Visible = false;
                bRaise.Enabled = false;
                bCall.Enabled = false;
                bRaise.Enabled = false;
                bRaise.Enabled = false;
                bFold.Enabled = false;
                _timer.Stop();
                botOneTurn = true;
                if (!_b1Fturn)
                {
                    if (botOneTurn)
                    {
                        FixCall(b1Status, ref _b1Call, ref _b1Raise, 1);
                        FixCall(b1Status, ref _b1Call, ref _b1Raise, 2);
                        Rules(2, 3, "Bot 1", ref botOneType, ref botOnePower, _b1Fturn);
                        MessageBox.Show("Bot 1's Turn");
                        Ai(2, 3, ref Bot1Chips, ref botOneTurn, ref _b1Fturn, b1Status, 0, botOnePower, botOneType);
                        _turnCount++;
                        _last = 1;
                        botOneTurn = false;
                        botTwoTurn = true;
                    }
                }
                if (_b1Fturn && !_b1Folded)
                {
                    _bools.RemoveAt(1);
                    _bools.Insert(1, null);
                    _maxLeft--;
                    _b1Folded = true;
                }
                if (_b1Fturn || !botOneTurn)
                {
                    await CheckRaise(1, 1);
                    botTwoTurn = true;
                }
                if (!_b2Fturn)
                {
                    if (botTwoTurn)
                    {
                        FixCall(b2Status, ref _b2Call, ref _b2Raise, 1);
                        FixCall(b2Status, ref _b2Call, ref _b2Raise, 2);
                        Rules(4, 5, "Bot 2", ref botTwoType, ref botTwoPower, _b2Fturn);
                        MessageBox.Show("Bot 2's Turn");
                        Ai(4, 5, ref Bot2Chips, ref botTwoTurn, ref _b2Fturn, b2Status, 1, botTwoPower, botTwoType);
                        _turnCount++;
                        _last = 2;
                        botTwoTurn = false;
                        botThreeTurn = true;
                    }
                }
                if (_b2Fturn && !_b2Folded)
                {
                    _bools.RemoveAt(2);
                    _bools.Insert(2, null);
                    _maxLeft--;
                    _b2Folded = true;
                }
                if (_b2Fturn || !botTwoTurn)
                {
                    await CheckRaise(2, 2);
                    botThreeTurn = true;
                }
                if (!_b3Fturn)
                {
                    if (botThreeTurn)
                    {
                        FixCall(b3Status, ref _b3Call, ref _b3Raise, 1);
                        FixCall(b3Status, ref _b3Call, ref _b3Raise, 2);
                        Rules(6, 7, "Bot 3", ref botThreeType, ref botThreePower, _b3Fturn);
                        MessageBox.Show("Bot 3's Turn");
                        Ai(6, 7, ref Bot3Chips, ref botThreeTurn, ref _b3Fturn, b3Status, 2, botThreePower, botThreeType);
                        _turnCount++;
                        _last = 3;
                        botThreeTurn = false;
                        botFourTurn = true;
                    }
                }
                if (_b3Fturn && !_b3Folded)
                {
                    _bools.RemoveAt(3);
                    _bools.Insert(3, null);
                    _maxLeft--;
                    _b3Folded = true;
                }
                if (_b3Fturn || !botThreeTurn)
                {
                    await CheckRaise(3, 3);
                    botFourTurn = true;
                }
                if (!_b4Fturn)
                {
                    if (botFourTurn)
                    {
                        FixCall(b4Status, ref _b4Call, ref _b4Raise, 1);
                        FixCall(b4Status, ref _b4Call, ref _b4Raise, 2);
                        Rules(8, 9, "Bot 4", ref botFourType, ref botFourPower, _b4Fturn);
                        MessageBox.Show("Bot 4's Turn");
                        Ai(8, 9, ref Bot4Chips, ref botFourTurn, ref _b4Fturn, b4Status, 3, botFourPower, botFourType);
                        _turnCount++;
                        _last = 4;
                        botFourTurn = false;
                        botFiveTurn = true;
                    }
                }
                if (_b4Fturn && !_b4Folded)
                {
                    _bools.RemoveAt(4);
                    _bools.Insert(4, null);
                    _maxLeft--;
                    _b4Folded = true;
                }
                if (_b4Fturn || !botFourTurn)
                {
                    await CheckRaise(4, 4);
                    botFiveTurn = true;
                }
                if (!_b5Fturn)
                {
                    if (botFiveTurn)
                    {
                        FixCall(b5Status, ref _b5Call, ref _b5Raise, 1);
                        FixCall(b5Status, ref _b5Call, ref _b5Raise, 2);
                        Rules(10, 11, "Bot 5", ref botFiveType, ref botFivePower, _b5Fturn);
                        MessageBox.Show("Bot 5's Turn");
                        Ai(10, 11, ref Bot5Chips, ref botFiveTurn, ref _b5Fturn, b5Status, 4, botFivePower, botFiveType);
                        _turnCount++;
                        _last = 5;
                        botFiveTurn = false;
                    }
                }
                if (_b5Fturn && !_b5Folded)
                {
                    _bools.RemoveAt(5);
                    _bools.Insert(5, null);
                    _maxLeft--;
                    _b5Folded = true;
                }
                if (_b5Fturn || !botFiveTurn)
                {
                    await CheckRaise(5, 5);
                    _pturn = true;
                }
                if (_pFturn && !_pFolded)
                {
                    if (bCall.Text.Contains("All in") == false || bRaise.Text.Contains("All in") == false)
                    {
                        _bools.RemoveAt(0);
                        _bools.Insert(0, null);
                        _maxLeft--;
                        _pFolded = true;
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

        void Rules(int c1, int c2, string currentText, ref double current, ref double power, bool foldedTurn)
        {
            if (c1 == 0 && c2 == 1)
            {
            }
            if (!foldedTurn || c1 == 0 && c2 == 1 && pStatus.Text.Contains("Fold") == false)
            {
                #region Variables
                bool done = false, vf = false;
                int[] straight1 = new int[5];
                int[] straight = new int[7];
                straight[0] = _reserve[c1];
                straight[1] = _reserve[c2];
                straight1[0] = straight[2] = _reserve[12];
                straight1[1] = straight[3] = _reserve[13];
                straight1[2] = straight[4] = _reserve[14];
                straight1[3] = straight[5] = _reserve[15];
                straight1[4] = straight[6] = _reserve[16];
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
                    if (_reserve[i] == int.Parse(_holder[c1].Tag.ToString()) && _reserve[i + 1] == int.Parse(_holder[c2].Tag.ToString()))
                    {
                        //Pair from Hand current = 1

                        RPairFromHand(ref current, ref power);

                        #region Pair or Two Pair from Table current = 2 || 0
                        RPairTwoPair(ref current, ref power);
                        #endregion

                        #region Two Pair current = 2
                        RTwoPair(ref current, ref power);
                        #endregion

                        #region Three of a kind current = 3
                        RThreeOfAKind(ref current, ref power, straight);
                        #endregion

                        #region Straight current = 4
                        RStraight(ref current, ref power, straight);
                        #endregion

                        #region Flush current = 5 || 5.5
                        RFlush(ref current, ref power, ref vf, straight1);
                        #endregion

                        #region Full House current = 6
                        RFullHouse(ref current, ref power, ref done, straight);
                        #endregion

                        #region Four of a Kind current = 7
                        RFourOfAKind(ref current, ref power, straight);
                        #endregion

                        #region Straight Flush current = 8 || 9
                        RStraightFlush(ref current, ref power, st1, st2, st3, st4);
                        #endregion

                        #region High Card current = -1
                        RHighCard(ref current, ref power);
                        #endregion
                    }
                }
            }
        }
        private void RStraightFlush(ref double current, ref double power, int[] st1, int[] st2, int[] st3, int[] st4)
        {
            if (current >= -1)
            {
                if (st1.Length >= 5)
                {
                    if (st1[0] + 4 == st1[4])
                    {
                        current = 8;
                        power = (st1.Max()) / 4 + current * 100;
                        _win.Add(new Type() { Power = power, Current = 8 });
                        _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (st1[0] == 0 && st1[1] == 9 && st1[2] == 10 && st1[3] == 11 && st1[0] + 12 == st1[4])
                    {
                        current = 9;
                        power = (st1.Max()) / 4 + current * 100;
                        _win.Add(new Type() { Power = power, Current = 9 });
                        _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (st2.Length >= 5)
                {
                    if (st2[0] + 4 == st2[4])
                    {
                        current = 8;
                        power = (st2.Max()) / 4 + current * 100;
                        _win.Add(new Type() { Power = power, Current = 8 });
                        _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (st2[0] == 0 && st2[1] == 9 && st2[2] == 10 && st2[3] == 11 && st2[0] + 12 == st2[4])
                    {
                        current = 9;
                        power = (st2.Max()) / 4 + current * 100;
                        _win.Add(new Type() { Power = power, Current = 9 });
                        _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (st3.Length >= 5)
                {
                    if (st3[0] + 4 == st3[4])
                    {
                        current = 8;
                        power = (st3.Max()) / 4 + current * 100;
                        _win.Add(new Type() { Power = power, Current = 8 });
                        _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (st3[0] == 0 && st3[1] == 9 && st3[2] == 10 && st3[3] == 11 && st3[0] + 12 == st3[4])
                    {
                        current = 9;
                        power = (st3.Max() / 4) + current * 100;
                        _win.Add(new Type() { Power = power, Current = 9 });
                        _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (st4.Length >= 5)
                {
                    if (st4[0] + 4 == st4[4])
                    {
                        current = 8;
                        power = (st4.Max()) / 4 + current * 100;
                        _win.Add(new Type() { Power = power, Current = 8 });
                        _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (st4[0] == 0 && st4[1] == 9 && st4[2] == 10 && st4[3] == 11 && st4[0] + 12 == st4[4])
                    {
                        current = 9;
                        power = (st4.Max()) / 4 + current * 100;
                        _win.Add(new Type() { Power = power, Current = 9 });
                        _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }
        private void RFourOfAKind(ref double current, ref double power, int[] straight)
        {
            if (current >= -1)
            {
                for (int j = 0; j <= 3; j++)
                {
                    if (straight[j] / 4 == straight[j + 1] / 4 && straight[j] / 4 == straight[j + 2] / 4 &&
                        straight[j] / 4 == straight[j + 3] / 4)
                    {
                        current = 7;
                        power = (straight[j] / 4) * 4 + current * 100;
                        _win.Add(new Type() { Power = power, Current = 7 });
                        _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (straight[j] / 4 == 0 && straight[j + 1] / 4 == 0 && straight[j + 2] / 4 == 0 && straight[j + 3] / 4 == 0)
                    {
                        current = 7;
                        power = 13 * 4 + current * 100;
                        _win.Add(new Type() { Power = power, Current = 7 });
                        _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }
        private void RFullHouse(ref double current, ref double power, ref bool done, int[] straight)
        {
            if (current >= -1)
            {
                type = power;
                for (int j = 0; j <= 12; j++)
                {
                    var fh = straight.Where(o => o / 4 == j).ToArray();
                    if (fh.Length == 3 || done)
                    {
                        if (fh.Length == 2)
                        {
                            if (fh.Max() / 4 == 0)
                            {
                                current = 6;
                                power = 13 * 2 + current * 100;
                                _win.Add(new Type() { Power = power, Current = 6 });
                                _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                                break;
                            }
                            if (fh.Max() / 4 > 0)
                            {
                                current = 6;
                                power = fh.Max() / 4 * 2 + current * 100;
                                _win.Add(new Type() { Power = power, Current = 6 });
                                _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                                break;
                            }
                        }
                        if (!done)
                        {
                            if (fh.Max() / 4 == 0)
                            {
                                power = 13;
                                done = true;
                                j = -1;
                            }
                            else
                            {
                                power = fh.Max() / 4;
                                done = true;
                                j = -1;
                            }
                        }
                    }
                }
                if (current != 6)
                {
                    power = type;
                }
            }
        }
        private void RFlush(ref double current, ref double power, ref bool vf, int[] straight1)
        {
            if (current >= -1)
            {
                var f1 = straight1.Where(o => o % 4 == 0).ToArray();
                var f2 = straight1.Where(o => o % 4 == 1).ToArray();
                var f3 = straight1.Where(o => o % 4 == 2).ToArray();
                var f4 = straight1.Where(o => o % 4 == 3).ToArray();
                if (f1.Length == 3 || f1.Length == 4)
                {
                    if (_reserve[i] % 4 == _reserve[i + 1] % 4 && _reserve[i] % 4 == f1[0] % 4)
                    {
                        if (_reserve[i] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            power = _reserve[i] + current * 100;
                            _win.Add(new Type() { Power = power, Current = 5 });
                            _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        if (_reserve[i + 1] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            power = _reserve[i + 1] + current * 100;
                            _win.Add(new Type() { Power = power, Current = 5 });
                            _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (_reserve[i] / 4 < f1.Max() / 4 && _reserve[i + 1] / 4 < f1.Max() / 4)
                        {
                            current = 5;
                            power = f1.Max() + current * 100;
                            _win.Add(new Type() { Power = power, Current = 5 });
                            _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f1.Length == 4)//different cards in hand
                {
                    if (_reserve[i] % 4 != _reserve[i + 1] % 4 && _reserve[i] % 4 == f1[0] % 4)
                    {
                        if (_reserve[i] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            power = _reserve[i] + current * 100;
                            _win.Add(new Type() { Power = power, Current = 5 });
                            _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f1.Max() + current * 100;
                            _win.Add(new Type() { Power = power, Current = 5 });
                            _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                    if (_reserve[i + 1] % 4 != _reserve[i] % 4 && _reserve[i + 1] % 4 == f1[0] % 4)
                    {
                        if (_reserve[i + 1] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            power = _reserve[i + 1] + current * 100;
                            _win.Add(new Type() { Power = power, Current = 5 });
                            _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f1.Max() + current * 100;
                            _win.Add(new Type() { Power = power, Current = 5 });
                            _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f1.Length == 5)
                {
                    if (_reserve[i] % 4 == f1[0] % 4 && _reserve[i] / 4 > f1.Min() / 4)
                    {
                        current = 5;
                        power = _reserve[i] + current * 100;
                        _win.Add(new Type() { Power = power, Current = 5 });
                        _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    if (_reserve[i + 1] % 4 == f1[0] % 4 && _reserve[i + 1] / 4 > f1.Min() / 4)
                    {
                        current = 5;
                        power = _reserve[i + 1] + current * 100;
                        _win.Add(new Type() { Power = power, Current = 5 });
                        _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (_reserve[i] / 4 < f1.Min() / 4 && _reserve[i + 1] / 4 < f1.Min())
                    {
                        current = 5;
                        power = f1.Max() + current * 100;
                        _win.Add(new Type() { Power = power, Current = 5 });
                        _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f2.Length == 3 || f2.Length == 4)
                {
                    if (_reserve[i] % 4 == _reserve[i + 1] % 4 && _reserve[i] % 4 == f2[0] % 4)
                    {
                        if (_reserve[i] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            power = _reserve[i] + current * 100;
                            _win.Add(new Type() { Power = power, Current = 5 });
                            _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        if (_reserve[i + 1] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            power = _reserve[i + 1] + current * 100;
                            _win.Add(new Type() { Power = power, Current = 5 });
                            _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (_reserve[i] / 4 < f2.Max() / 4 && _reserve[i + 1] / 4 < f2.Max() / 4)
                        {
                            current = 5;
                            power = f2.Max() + current * 100;
                            _win.Add(new Type() { Power = power, Current = 5 });
                            _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f2.Length == 4)//different cards in hand
                {
                    if (_reserve[i] % 4 != _reserve[i + 1] % 4 && _reserve[i] % 4 == f2[0] % 4)
                    {
                        if (_reserve[i] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            power = _reserve[i] + current * 100;
                            _win.Add(new Type() { Power = power, Current = 5 });
                            _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f2.Max() + current * 100;
                            _win.Add(new Type() { Power = power, Current = 5 });
                            _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                    if (_reserve[i + 1] % 4 != _reserve[i] % 4 && _reserve[i + 1] % 4 == f2[0] % 4)
                    {
                        if (_reserve[i + 1] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            power = _reserve[i + 1] + current * 100;
                            _win.Add(new Type() { Power = power, Current = 5 });
                            _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f2.Max() + current * 100;
                            _win.Add(new Type() { Power = power, Current = 5 });
                            _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f2.Length == 5)
                {
                    if (_reserve[i] % 4 == f2[0] % 4 && _reserve[i] / 4 > f2.Min() / 4)
                    {
                        current = 5;
                        power = _reserve[i] + current * 100;
                        _win.Add(new Type() { Power = power, Current = 5 });
                        _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    if (_reserve[i + 1] % 4 == f2[0] % 4 && _reserve[i + 1] / 4 > f2.Min() / 4)
                    {
                        current = 5;
                        power = _reserve[i + 1] + current * 100;
                        _win.Add(new Type() { Power = power, Current = 5 });
                        _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (_reserve[i] / 4 < f2.Min() / 4 && _reserve[i + 1] / 4 < f2.Min())
                    {
                        current = 5;
                        power = f2.Max() + current * 100;
                        _win.Add(new Type() { Power = power, Current = 5 });
                        _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f3.Length == 3 || f3.Length == 4)
                {
                    if (_reserve[i] % 4 == _reserve[i + 1] % 4 && _reserve[i] % 4 == f3[0] % 4)
                    {
                        if (_reserve[i] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            power = _reserve[i] + current * 100;
                            _win.Add(new Type() { Power = power, Current = 5 });
                            _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        if (_reserve[i + 1] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            power = _reserve[i + 1] + current * 100;
                            _win.Add(new Type() { Power = power, Current = 5 });
                            _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (_reserve[i] / 4 < f3.Max() / 4 && _reserve[i + 1] / 4 < f3.Max() / 4)
                        {
                            current = 5;
                            power = f3.Max() + current * 100;
                            _win.Add(new Type() { Power = power, Current = 5 });
                            _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f3.Length == 4)//different cards in hand
                {
                    if (_reserve[i] % 4 != _reserve[i + 1] % 4 && _reserve[i] % 4 == f3[0] % 4)
                    {
                        if (_reserve[i] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            power = _reserve[i] + current * 100;
                            _win.Add(new Type() { Power = power, Current = 5 });
                            _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f3.Max() + current * 100;
                            _win.Add(new Type() { Power = power, Current = 5 });
                            _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                    if (_reserve[i + 1] % 4 != _reserve[i] % 4 && _reserve[i + 1] % 4 == f3[0] % 4)
                    {
                        if (_reserve[i + 1] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            power = _reserve[i + 1] + current * 100;
                            _win.Add(new Type() { Power = power, Current = 5 });
                            _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f3.Max() + current * 100;
                            _win.Add(new Type() { Power = power, Current = 5 });
                            _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f3.Length == 5)
                {
                    if (_reserve[i] % 4 == f3[0] % 4 && _reserve[i] / 4 > f3.Min() / 4)
                    {
                        current = 5;
                        power = _reserve[i] + current * 100;
                        _win.Add(new Type() { Power = power, Current = 5 });
                        _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    if (_reserve[i + 1] % 4 == f3[0] % 4 && _reserve[i + 1] / 4 > f3.Min() / 4)
                    {
                        current = 5;
                        power = _reserve[i + 1] + current * 100;
                        _win.Add(new Type() { Power = power, Current = 5 });
                        _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (_reserve[i] / 4 < f3.Min() / 4 && _reserve[i + 1] / 4 < f3.Min())
                    {
                        current = 5;
                        power = f3.Max() + current * 100;
                        _win.Add(new Type() { Power = power, Current = 5 });
                        _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f4.Length == 3 || f4.Length == 4)
                {
                    if (_reserve[i] % 4 == _reserve[i + 1] % 4 && _reserve[i] % 4 == f4[0] % 4)
                    {
                        if (_reserve[i] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            power = _reserve[i] + current * 100;
                            _win.Add(new Type() { Power = power, Current = 5 });
                            _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        if (_reserve[i + 1] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            power = _reserve[i + 1] + current * 100;
                            _win.Add(new Type() { Power = power, Current = 5 });
                            _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (_reserve[i] / 4 < f4.Max() / 4 && _reserve[i + 1] / 4 < f4.Max() / 4)
                        {
                            current = 5;
                            power = f4.Max() + current * 100;
                            _win.Add(new Type() { Power = power, Current = 5 });
                            _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f4.Length == 4)//different cards in hand
                {
                    if (_reserve[i] % 4 != _reserve[i + 1] % 4 && _reserve[i] % 4 == f4[0] % 4)
                    {
                        if (_reserve[i] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            power = _reserve[i] + current * 100;
                            _win.Add(new Type() { Power = power, Current = 5 });
                            _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f4.Max() + current * 100;
                            _win.Add(new Type() { Power = power, Current = 5 });
                            _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                    if (_reserve[i + 1] % 4 != _reserve[i] % 4 && _reserve[i + 1] % 4 == f4[0] % 4)
                    {
                        if (_reserve[i + 1] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            power = _reserve[i + 1] + current * 100;
                            _win.Add(new Type() { Power = power, Current = 5 });
                            _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f4.Max() + current * 100;
                            _win.Add(new Type() { Power = power, Current = 5 });
                            _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f4.Length == 5)
                {
                    if (_reserve[i] % 4 == f4[0] % 4 && _reserve[i] / 4 > f4.Min() / 4)
                    {
                        current = 5;
                        power = _reserve[i] + current * 100;
                        _win.Add(new Type() { Power = power, Current = 5 });
                        _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    if (_reserve[i + 1] % 4 == f4[0] % 4 && _reserve[i + 1] / 4 > f4.Min() / 4)
                    {
                        current = 5;
                        power = _reserve[i + 1] + current * 100;
                        _win.Add(new Type() { Power = power, Current = 5 });
                        _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (_reserve[i] / 4 < f4.Min() / 4 && _reserve[i + 1] / 4 < f4.Min())
                    {
                        current = 5;
                        power = f4.Max() + current * 100;
                        _win.Add(new Type() { Power = power, Current = 5 });
                        _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }
                //ace
                if (f1.Length > 0)
                {
                    if (_reserve[i] / 4 == 0 && _reserve[i] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        _win.Add(new Type() { Power = power, Current = 5.5 });
                        _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (_reserve[i + 1] / 4 == 0 && _reserve[i + 1] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        _win.Add(new Type() { Power = power, Current = 5.5 });
                        _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (f2.Length > 0)
                {
                    if (_reserve[i] / 4 == 0 && _reserve[i] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        _win.Add(new Type() { Power = power, Current = 5.5 });
                        _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (_reserve[i + 1] / 4 == 0 && _reserve[i + 1] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        _win.Add(new Type() { Power = power, Current = 5.5 });
                        _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (f3.Length > 0)
                {
                    if (_reserve[i] / 4 == 0 && _reserve[i] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        _win.Add(new Type() { Power = power, Current = 5.5 });
                        _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (_reserve[i + 1] / 4 == 0 && _reserve[i + 1] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        _win.Add(new Type() { Power = power, Current = 5.5 });
                        _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (f4.Length > 0)
                {
                    if (_reserve[i] / 4 == 0 && _reserve[i] % 4 == f4[0] % 4 && vf && f4.Length > 0)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        _win.Add(new Type() { Power = power, Current = 5.5 });
                        _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (_reserve[i + 1] / 4 == 0 && _reserve[i + 1] % 4 == f4[0] % 4 && vf)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        _win.Add(new Type() { Power = power, Current = 5.5 });
                        _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }
        private void RStraight(ref double current, ref double power, int[] straight)
        {
            if (current >= -1)
            {
                var op = straight.Select(o => o / 4).Distinct().ToArray();
                for (int j = 0; j < op.Length - 4; j++)
                {
                    if (op[j] + 4 == op[j + 4])
                    {
                        if (op.Max() - 4 == op[j])
                        {
                            current = 4;
                            power = op.Max() + current * 100;
                            _win.Add(new Type() { Power = power, Current = 4 });
                            _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        }
                        else
                        {
                            current = 4;
                            power = op[j + 4] + current * 100;
                            _win.Add(new Type() { Power = power, Current = 4 });
                            _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        }
                    }
                    if (op[j] == 0 && op[j + 1] == 9 && op[j + 2] == 10 && op[j + 3] == 11 && op[j + 4] == 12)
                    {
                        current = 4;
                        power = 13 + current * 100;
                        _win.Add(new Type() { Power = power, Current = 4 });
                        _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }
        private void RThreeOfAKind(ref double current, ref double power, int[] straight)
        {
            if (current >= -1)
            {
                for (int j = 0; j <= 12; j++)
                {
                    var fh = straight.Where(o => o / 4 == j).ToArray();
                    if (fh.Length == 3)
                    {
                        if (fh.Max() / 4 == 0)
                        {
                            current = 3;
                            power = 13 * 3 + current * 100;
                            _win.Add(new Type() { Power = power, Current = 3 });
                            _sorted = _win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                        else
                        {
                            current = 3;
                            power = fh[0] / 4 + fh[1] / 4 + fh[2] / 4 + current * 100;
                            _win.Add(new Type() { Power = power, Current = 3 });
                            _sorted = _win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                    }
                }
            }
        }
        private void RTwoPair(ref double current, ref double power)
        {
            if (current >= -1)
            {
                bool msgbox = false;
                for (int tc = 16; tc >= 12; tc--)
                {
                    int max = tc - 12;
                    if (_reserve[i] / 4 != _reserve[i + 1] / 4)
                    {
                        for (int k = 1; k <= max; k++)
                        {
                            if (tc - k < 12)
                            {
                                max--;
                            }
                            if (tc - k >= 12)
                            {
                                if (_reserve[i] / 4 == _reserve[tc] / 4 && _reserve[i + 1] / 4 == _reserve[tc - k] / 4 ||
                                    _reserve[i + 1] / 4 == _reserve[tc] / 4 && _reserve[i] / 4 == _reserve[tc - k] / 4)
                                {
                                    if (!msgbox)
                                    {
                                        if (_reserve[i] / 4 == 0)
                                        {
                                            current = 2;
                                            power = 13 * 4 + (_reserve[i + 1] / 4) * 2 + current * 100;
                                            _win.Add(new Type() { Power = power, Current = 2 });
                                            _sorted = _win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (_reserve[i + 1] / 4 == 0)
                                        {
                                            current = 2;
                                            power = 13 * 4 + (_reserve[i] / 4) * 2 + current * 100;
                                            _win.Add(new Type() { Power = power, Current = 2 });
                                            _sorted = _win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (_reserve[i + 1] / 4 != 0 && _reserve[i] / 4 != 0)
                                        {
                                            current = 2;
                                            power = (_reserve[i] / 4) * 2 + (_reserve[i + 1] / 4) * 2 + current * 100;
                                            _win.Add(new Type() { Power = power, Current = 2 });
                                            _sorted = _win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
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
        private void RPairTwoPair(ref double current, ref double power)
        {
            if (current >= -1)
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
                            if (_reserve[tc] / 4 == _reserve[tc - k] / 4)
                            {
                                if (_reserve[tc] / 4 != _reserve[i] / 4 && _reserve[tc] / 4 != _reserve[i + 1] / 4 && current == 1)
                                {
                                    if (!msgbox)
                                    {
                                        if (_reserve[i + 1] / 4 == 0)
                                        {
                                            current = 2;
                                            power = (_reserve[i] / 4) * 2 + 13 * 4 + current * 100;
                                            _win.Add(new Type() { Power = power, Current = 2 });
                                            _sorted = _win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (_reserve[i] / 4 == 0)
                                        {
                                            current = 2;
                                            power = (_reserve[i + 1] / 4) * 2 + 13 * 4 + current * 100;
                                            _win.Add(new Type() { Power = power, Current = 2 });
                                            _sorted = _win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (_reserve[i + 1] / 4 != 0)
                                        {
                                            current = 2;
                                            power = (_reserve[tc] / 4) * 2 + (_reserve[i + 1] / 4) * 2 + current * 100;
                                            _win.Add(new Type() { Power = power, Current = 2 });
                                            _sorted = _win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (_reserve[i] / 4 != 0)
                                        {
                                            current = 2;
                                            power = (_reserve[tc] / 4) * 2 + (_reserve[i] / 4) * 2 + current * 100;
                                            _win.Add(new Type() { Power = power, Current = 2 });
                                            _sorted = _win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                    }
                                    msgbox = true;
                                }
                                if (current == -1)
                                {
                                    if (!msgbox1)
                                    {
                                        if (_reserve[i] / 4 > _reserve[i + 1] / 4)
                                        {
                                            if (_reserve[tc] / 4 == 0)
                                            {
                                                current = 0;
                                                power = 13 + _reserve[i] / 4 + current * 100;
                                                _win.Add(new Type() { Power = power, Current = 1 });
                                                _sorted = _win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                            else
                                            {
                                                current = 0;
                                                power = _reserve[tc] / 4 + _reserve[i] / 4 + current * 100;
                                                _win.Add(new Type() { Power = power, Current = 1 });
                                                _sorted = _win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                        }
                                        else
                                        {
                                            if (_reserve[tc] / 4 == 0)
                                            {
                                                current = 0;
                                                power = 13 + _reserve[i + 1] + current * 100;
                                                _win.Add(new Type() { Power = power, Current = 1 });
                                                _sorted = _win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                            else
                                            {
                                                current = 0;
                                                power = _reserve[tc] / 4 + _reserve[i + 1] / 4 + current * 100;
                                                _win.Add(new Type() { Power = power, Current = 1 });
                                                _sorted = _win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
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
        private void RPairFromHand(ref double current, ref double power)
        {
            if (current >= -1)
            {
                bool msgbox = false;
                if (_reserve[i] / 4 == _reserve[i + 1] / 4)
                {
                    if (!msgbox)
                    {
                        if (_reserve[i] / 4 == 0)
                        {
                            current = 1;
                            power = 13 * 4 + current * 100;
                            _win.Add(new Type() { Power = power, Current = 1 });
                            _sorted = _win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                        else
                        {
                            current = 1;
                            power = (_reserve[i + 1] / 4) * 4 + current * 100;
                            _win.Add(new Type() { Power = power, Current = 1 });
                            _sorted = _win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                    }
                    msgbox = true;
                }
                for (int tc = 16; tc >= 12; tc--)
                {
                    if (_reserve[i + 1] / 4 == _reserve[tc] / 4)
                    {
                        if (!msgbox)
                        {
                            if (_reserve[i + 1] / 4 == 0)
                            {
                                current = 1;
                                power = 13 * 4 + _reserve[i] / 4 + current * 100;
                                _win.Add(new Type() { Power = power, Current = 1 });
                                _sorted = _win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                            else
                            {
                                current = 1;
                                power = (_reserve[i + 1] / 4) * 4 + _reserve[i] / 4 + current * 100;
                                _win.Add(new Type() { Power = power, Current = 1 });
                                _sorted = _win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                        }
                        msgbox = true;
                    }
                    if (_reserve[i] / 4 == _reserve[tc] / 4)
                    {
                        if (!msgbox)
                        {
                            if (_reserve[i] / 4 == 0)
                            {
                                current = 1;
                                power = 13 * 4 + _reserve[i + 1] / 4 + current * 100;
                                _win.Add(new Type() { Power = power, Current = 1 });
                                _sorted = _win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                            else
                            {
                                current = 1;
                                power = (_reserve[tc] / 4) * 4 + _reserve[i + 1] / 4 + current * 100;
                                _win.Add(new Type() { Power = power, Current = 1 });
                                _sorted = _win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                        }
                        msgbox = true;
                    }
                }
            }
        }
        private void RHighCard(ref double current, ref double power)
        {
            if (current == -1)
            {
                if (_reserve[i] / 4 > _reserve[i + 1] / 4)
                {
                    current = -1;
                    power = _reserve[i] / 4;
                    _win.Add(new Type() { Power = power, Current = -1 });
                    _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
                else
                {
                    current = -1;
                    power = _reserve[i + 1] / 4;
                    _win.Add(new Type() { Power = power, Current = -1 });
                    _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
                if (_reserve[i] / 4 == 0 || _reserve[i + 1] / 4 == 0)
                {
                    current = -1;
                    power = 13;
                    _win.Add(new Type() { Power = power, Current = -1 });
                    _sorted = _win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
            }
        }

        void Winner(double current, double power, string currentText, int chips, string lastly)
        {
            if (lastly == " ")
            {
                lastly = "Bot 5";
            }
            for (int j = 0; j <= 16; j++)
            {
                //await Task.Delay(5);
                if (_holder[j].Visible)
                    _holder[j].Image = _deck[j];
            }
            if (current == _sorted.Current)
            {
                if (power == _sorted.Power)
                {
                    _winners++;
                    _checkWinners.Add(currentText);
                    if (current == -1)
                    {
                        MessageBox.Show(currentText + " High Card ");
                    }
                    if (current == 1 || current == 0)
                    {
                        MessageBox.Show(currentText + " Pair ");
                    }
                    if (current == 2)
                    {
                        MessageBox.Show(currentText + " Two Pair ");
                    }
                    if (current == 3)
                    {
                        MessageBox.Show(currentText + " Three of a Kind ");
                    }
                    if (current == 4)
                    {
                        MessageBox.Show(currentText + " Straight ");
                    }
                    if (current == 5 || current == 5.5)
                    {
                        MessageBox.Show(currentText + " Flush ");
                    }
                    if (current == 6)
                    {
                        MessageBox.Show(currentText + " Full House ");
                    }
                    if (current == 7)
                    {
                        MessageBox.Show(currentText + " Four of a Kind ");
                    }
                    if (current == 8)
                    {
                        MessageBox.Show(currentText + " Straight Flush ");
                    }
                    if (current == 9)
                    {
                        MessageBox.Show(currentText + " Royal Flush ! ");
                    }
                }
            }
            if (currentText == lastly)//lastfixed
            {
                if (_winners > 1)
                {
                    if (_checkWinners.Contains("Player"))
                    {
                        Chips += int.Parse(tbPot.Text) / _winners;
                        tbChips.Text = Chips.ToString();
                        //pPanel.Visible = true;

                    }
                    if (_checkWinners.Contains("Bot 1"))
                    {
                        Bot1Chips += int.Parse(tbPot.Text) / _winners;
                        tbBotChips1.Text = Bot1Chips.ToString();
                        //b1Panel.Visible = true;
                    }
                    if (_checkWinners.Contains("Bot 2"))
                    {
                        Bot2Chips += int.Parse(tbPot.Text) / _winners;
                        tbBotChips2.Text = Bot2Chips.ToString();
                        //b2Panel.Visible = true;
                    }
                    if (_checkWinners.Contains("Bot 3"))
                    {
                        Bot3Chips += int.Parse(tbPot.Text) / _winners;
                        tbBotChips3.Text = Bot3Chips.ToString();
                        //b3Panel.Visible = true;
                    }
                    if (_checkWinners.Contains("Bot 4"))
                    {
                        Bot4Chips += int.Parse(tbPot.Text) / _winners;
                        tbBotChips4.Text = Bot4Chips.ToString();
                        //b4Panel.Visible = true;
                    }
                    if (_checkWinners.Contains("Bot 5"))
                    {
                        Bot5Chips += int.Parse(tbPot.Text) / _winners;
                        tbBotChips5.Text = Bot5Chips.ToString();
                        //b5Panel.Visible = true;
                    }
                    //await Finish(1);
                }
                if (_winners == 1)
                {
                    if (_checkWinners.Contains("Player"))
                    {
                        Chips += int.Parse(tbPot.Text);
                        //await Finish(1);
                        //pPanel.Visible = true;
                    }
                    if (_checkWinners.Contains("Bot 1"))
                    {
                        Bot1Chips += int.Parse(tbPot.Text);
                        //await Finish(1);
                        //b1Panel.Visible = true;
                    }
                    if (_checkWinners.Contains("Bot 2"))
                    {
                        Bot2Chips += int.Parse(tbPot.Text);
                        //await Finish(1);
                        //b2Panel.Visible = true;

                    }
                    if (_checkWinners.Contains("Bot 3"))
                    {
                        Bot3Chips += int.Parse(tbPot.Text);
                        //await Finish(1);
                        //b3Panel.Visible = true;
                    }
                    if (_checkWinners.Contains("Bot 4"))
                    {
                        Bot4Chips += int.Parse(tbPot.Text);
                        //await Finish(1);
                        //b4Panel.Visible = true;
                    }
                    if (_checkWinners.Contains("Bot 5"))
                    {
                        Bot5Chips += int.Parse(tbPot.Text);
                        //await Finish(1);
                        //b5Panel.Visible = true;
                    }
                }
            }
        }
        async Task CheckRaise(int currentTurn, int raiseTurn)
        {
            if (_raising)
            {
                _turnCount = 0;
                _raising = false;
                _raisedTurn = currentTurn;
                _changed = true;
            }
            else
            {
                if (_turnCount >= _maxLeft - 1 || !_changed && _turnCount == _maxLeft)
                {
                    if (currentTurn == _raisedTurn - 1 || !_changed && _turnCount == _maxLeft || _raisedTurn == 0 && currentTurn == 5)
                    {
                        _changed = false;
                        _turnCount = 0;
                        raise = 0;
                        call = 0;
                        _raisedTurn = 123;
                        rounds++;
                        if (!_pFturn)
                            pStatus.Text = "";
                        if (!_b1Fturn)
                            b1Status.Text = "";
                        if (!_b2Fturn)
                            b2Status.Text = "";
                        if (!_b3Fturn)
                            b3Status.Text = "";
                        if (!_b4Fturn)
                            b4Status.Text = "";
                        if (!_b5Fturn)
                            b5Status.Text = "";
                    }
                }
            }
            if (rounds == _flop)
            {
                for (int j = 12; j <= 14; j++)
                {
                    if (_holder[j].Image != _deck[j])
                    {
                        _holder[j].Image = _deck[j];
                        _pCall = 0; _pRaise = 0;
                        _b1Call = 0; _b1Raise = 0;
                        _b2Call = 0; _b2Raise = 0;
                        _b3Call = 0; _b3Raise = 0;
                        _b4Call = 0; _b4Raise = 0;
                        _b5Call = 0; _b5Raise = 0;
                    }
                }
            }
            if (rounds == _turn)
            {
                for (int j = 14; j <= 15; j++)
                {
                    if (_holder[j].Image != _deck[j])
                    {
                        _holder[j].Image = _deck[j];
                        _pCall = 0; _pRaise = 0;
                        _b1Call = 0; _b1Raise = 0;
                        _b2Call = 0; _b2Raise = 0;
                        _b3Call = 0; _b3Raise = 0;
                        _b4Call = 0; _b4Raise = 0;
                        _b5Call = 0; _b5Raise = 0;
                    }
                }
            }
            if (rounds == _river)
            {
                for (int j = 15; j <= 16; j++)
                {
                    if (_holder[j].Image != _deck[j])
                    {
                        _holder[j].Image = _deck[j];
                        _pCall = 0; _pRaise = 0;
                        _b1Call = 0; _b1Raise = 0;
                        _b2Call = 0; _b2Raise = 0;
                        _b3Call = 0; _b3Raise = 0;
                        _b4Call = 0; _b4Raise = 0;
                        _b5Call = 0; _b5Raise = 0;
                    }
                }
            }
            if (rounds == _end && _maxLeft == 6)
            {
                string fixedLast = "qwerty";
                if (!pStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Player";
                    Rules(0, 1, "Player", ref playerType, ref playerPower, _pFturn);
                }
                if (!b1Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 1";
                    Rules(2, 3, "Bot 1", ref botOneType, ref botOnePower, _b1Fturn);
                }
                if (!b2Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 2";
                    Rules(4, 5, "Bot 2", ref botTwoType, ref botTwoPower, _b2Fturn);
                }
                if (!b3Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 3";
                    Rules(6, 7, "Bot 3", ref botThreeType, ref botThreePower, _b3Fturn);
                }
                if (!b4Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 4";
                    Rules(8, 9, "Bot 4", ref botFourType, ref botFourPower, _b4Fturn);
                }
                if (!b5Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 5";
                    Rules(10, 11, "Bot 5", ref botFiveType, ref botFivePower, _b5Fturn);
                }
                Winner(playerType, playerPower, "Player", Chips, fixedLast);
                Winner(botOneType, botOnePower, "Bot 1", Bot1Chips, fixedLast);
                Winner(botTwoType, botTwoPower, "Bot 2", Bot2Chips, fixedLast);
                Winner(botThreeType, botThreePower, "Bot 3", Bot3Chips, fixedLast);
                Winner(botFourType, botFourPower, "Bot 4", Bot4Chips, fixedLast);
                Winner(botFiveType, botFivePower, "Bot 5", Bot5Chips, fixedLast);
                _restart = true;
                _pturn = true;
                _pFturn = false;
                _b1Fturn = false;
                _b2Fturn = false;
                _b3Fturn = false;
                _b4Fturn = false;
                _b5Fturn = false;
                if (Chips <= 0)
                {
                    AddChips f2 = new AddChips();
                    f2.ShowDialog();
                    if (f2.A != 0)
                    {
                        Chips = f2.A;
                        Bot1Chips += f2.A;
                        Bot2Chips += f2.A;
                        Bot3Chips += f2.A;
                        Bot4Chips += f2.A;
                        Bot5Chips += f2.A;
                        _pFturn = false;
                        _pturn = true;
                        bRaise.Enabled = true;
                        bFold.Enabled = true;
                        bCheck.Enabled = true;
                        bRaise.Text = "Raise";
                    }
                }
                playerPanel.Visible = false; bot1Panel.Visible = false; bot2Panel.Visible = false; bot3Panel.Visible = false; bot4Panel.Visible = false; bot5Panel.Visible = false;
                _pCall = 0; _pRaise = 0;
                _b1Call = 0; _b1Raise = 0;
                _b2Call = 0; _b2Raise = 0;
                _b3Call = 0; _b3Raise = 0;
                _b4Call = 0; _b4Raise = 0;
                _b5Call = 0; _b5Raise = 0;
                _last = 0;
                call = bb;
                raise = 0;
                imgLocation = Directory.GetFiles("..\\..\\Resources\\Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
                _bools.Clear();
                rounds = 0;
                playerPower = 0; playerType = -1;
                type = 0; botOnePower = 0; botTwoPower = 0; botThreePower = 0; botFourPower = 0; botFivePower = 0;
                botOneType = -1; botTwoType = -1; botThreeType = -1; botFourType = -1; botFiveType = -1;
                ints.Clear();
                _checkWinners.Clear();
                _winners = 0;
                _win.Clear();
                _sorted.Current = 0;
                _sorted.Power = 0;
                for (int os = 0; os < 17; os++)
                {
                    _holder[os].Image = null;
                    _holder[os].Invalidate();
                    _holder[os].Visible = false;
                }
                tbPot.Text = "0";
                pStatus.Text = "";
                await Shuffle();
                await Turns();
            }
        }
        void FixCall(Label status, ref int cCall, ref int cRaise, int options)
        {
            if (rounds != 4)
            {
                if (options == 1)
                {
                    if (status.Text.Contains("Raise"))
                    {
                        var changeRaise = status.Text.Substring(6);
                        cRaise = int.Parse(changeRaise);
                    }
                    if (status.Text.Contains("Call"))
                    {
                        var changeCall = status.Text.Substring(5);
                        cCall = int.Parse(changeCall);
                    }
                    if (status.Text.Contains("Check"))
                    {
                        cRaise = 0;
                        cCall = 0;
                    }
                }
                if (options == 2)
                {
                    if (cRaise != raise && cRaise <= raise)
                    {
                        call = Convert.ToInt32(raise) - cRaise;
                    }
                    if (cCall != call || cCall <= call)
                    {
                        call = call - cCall;
                    }
                    if (cRaise == raise && raise > 0)
                    {
                        call = 0;
                        bCall.Enabled = false;
                        bCall.Text = "Callisfuckedup";
                    }
                }
            }
        }
        async Task AllIn()
        {
            #region All in
            if (Chips <= 0 && !intsadded)
            {
                if (pStatus.Text.Contains("Raise"))
                {
                    ints.Add(Chips);
                    intsadded = true;
                }
                if (pStatus.Text.Contains("Call"))
                {
                    ints.Add(Chips);
                    intsadded = true;
                }
            }
            intsadded = false;
            if (Bot1Chips <= 0 && !_b1Fturn)
            {
                if (!intsadded)
                {
                    ints.Add(Bot1Chips);
                    intsadded = true;
                }
                intsadded = false;
            }
            if (Bot2Chips <= 0 && !_b2Fturn)
            {
                if (!intsadded)
                {
                    ints.Add(Bot2Chips);
                    intsadded = true;
                }
                intsadded = false;
            }
            if (Bot3Chips <= 0 && !_b3Fturn)
            {
                if (!intsadded)
                {
                    ints.Add(Bot3Chips);
                    intsadded = true;
                }
                intsadded = false;
            }
            if (Bot4Chips <= 0 && !_b4Fturn)
            {
                if (!intsadded)
                {
                    ints.Add(Bot4Chips);
                    intsadded = true;
                }
                intsadded = false;
            }
            if (Bot5Chips <= 0 && !_b5Fturn)
            {
                if (!intsadded)
                {
                    ints.Add(Bot5Chips);
                    intsadded = true;
                }
            }
            if (ints.ToArray().Length == _maxLeft)
            {
                await Finish(2);
            }
            else
            {
                ints.Clear();
            }
            #endregion

            var abc = _bools.Count(x => x == false);

            #region LastManStanding
            if (abc == 1)
            {
                int index = _bools.IndexOf(false);
                if (index == 0)
                {
                    Chips += int.Parse(tbPot.Text);
                    tbChips.Text = Chips.ToString();
                    playerPanel.Visible = true;
                    MessageBox.Show("Player Wins");
                }
                if (index == 1)
                {
                    Bot1Chips += int.Parse(tbPot.Text);
                    tbChips.Text = Bot1Chips.ToString();
                    bot1Panel.Visible = true;
                    MessageBox.Show("Bot 1 Wins");
                }
                if (index == 2)
                {
                    Bot2Chips += int.Parse(tbPot.Text);
                    tbChips.Text = Bot2Chips.ToString();
                    bot2Panel.Visible = true;
                    MessageBox.Show("Bot 2 Wins");
                }
                if (index == 3)
                {
                    Bot3Chips += int.Parse(tbPot.Text);
                    tbChips.Text = Bot3Chips.ToString();
                    bot3Panel.Visible = true;
                    MessageBox.Show("Bot 3 Wins");
                }
                if (index == 4)
                {
                    Bot4Chips += int.Parse(tbPot.Text);
                    tbChips.Text = Bot4Chips.ToString();
                    bot4Panel.Visible = true;
                    MessageBox.Show("Bot 4 Wins");
                }
                if (index == 5)
                {
                    Bot5Chips += int.Parse(tbPot.Text);
                    tbChips.Text = Bot5Chips.ToString();
                    bot5Panel.Visible = true;
                    MessageBox.Show("Bot 5 Wins");
                }
                for (int j = 0; j <= 16; j++)
                {
                    _holder[j].Visible = false;
                }
                await Finish(1);
            }
            intsadded = false;
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
            playerPanel.Visible = false;
            bot1Panel.Visible = false;
            bot2Panel.Visible = false;
            bot3Panel.Visible = false;
            bot4Panel.Visible = false;
            bot5Panel.Visible = false;

            call = bb;
            raise = 0;
            foldedPlayers = 5;

            type = 0;
            rounds = 0;
            botOnePower = 0;
            botTwoPower = 0;
            botThreePower = 0;
            botFourPower = 0;
            botFivePower = 0;
            playerPower = 0;

            playerType = -1;
            raise = 0;
            botOneType = -1;
            botTwoType = -1;
            botThreeType = -1;
            botFourType = -1;
            botFiveType = -1;

            _pturn = true;
            botOneTurn = false;
            botTwoTurn = false;
            botThreeTurn = false;
            botFourTurn = false;
            botFiveTurn = false;

            _pFturn = false;
            _b1Fturn = false;
            _b2Fturn = false;
            _b3Fturn = false;
            _b4Fturn = false;
            _b5Fturn = false;

            _pFolded = false;
            _b1Folded = false;
            _b2Folded = false;
            _b3Folded = false;
            _b4Folded = false;
            _b5Folded = false;

            _restart = false;
            _raising = false;

            _pCall = 0;
            _b1Call = 0;
            _b2Call = 0;
            _b3Call = 0;
            _b4Call = 0;
            _b5Call = 0;

            _pRaise = 0;
            _b1Raise = 0;
            _b2Raise = 0;
            _b3Raise = 0;
            _b4Raise = 0;
            _b5Raise = 0;

            _height = 0;
            _width = 0;
            _winners = 0;
            _flop = 1;
            _turn = 2;
            _river = 3;
            _end = 4;
            _maxLeft = 6;
            _last = 123; _raisedTurn = 1;
            _bools.Clear();
            _checkWinners.Clear();
            ints.Clear();
            _win.Clear();
            _sorted.Current = 0;
            _sorted.Power = 0;
            tbPot.Text = "0";
            _t = 60; _up = 10000000; _turnCount = 0;
            pStatus.Text = "";
            b1Status.Text = "";
            b2Status.Text = "";
            b3Status.Text = "";
            b4Status.Text = "";
            b5Status.Text = "";
            if (Chips <= 0)
            {
                AddChips f2 = new AddChips();
                f2.ShowDialog();
                if (f2.A != 0)
                {
                    Chips = f2.A;
                    Bot1Chips += f2.A;
                    Bot2Chips += f2.A;
                    Bot3Chips += f2.A;
                    Bot4Chips += f2.A;
                    Bot5Chips += f2.A;
                    _pFturn = false;
                    _pturn = true;
                    bRaise.Enabled = true;
                    bFold.Enabled = true;
                    bCheck.Enabled = true;
                    bRaise.Text = "Raise";
                }
            }
            imgLocation = Directory.GetFiles("..\\..\\Resources\\Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
            for (int os = 0; os < 17; os++)
            {
                _holder[os].Image = null;
                _holder[os].Invalidate();
                _holder[os].Visible = false;
            }
            await Shuffle();
            //await Turns();
        }
        void FixWinners()
        {
            _win.Clear();
            _sorted.Current = 0;
            _sorted.Power = 0;
            string fixedLast = "qwerty";
            if (!pStatus.Text.Contains("Fold"))
            {
                fixedLast = "Player";
                Rules(0, 1, "Player", ref playerType, ref playerPower, _pFturn);
            }
            if (!b1Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 1";
                Rules(2, 3, "Bot 1", ref botOneType, ref botOnePower, _b1Fturn);
            }
            if (!b2Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 2";
                Rules(4, 5, "Bot 2", ref botTwoType, ref botTwoPower, _b2Fturn);
            }
            if (!b3Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 3";
                Rules(6, 7, "Bot 3", ref botThreeType, ref botThreePower, _b3Fturn);
            }
            if (!b4Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 4";
                Rules(8, 9, "Bot 4", ref botFourType, ref botFourPower, _b4Fturn);
            }
            if (!b5Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 5";
                Rules(10, 11, "Bot 5", ref botFiveType, ref botFivePower, _b5Fturn);
            }
            Winner(playerType, playerPower, "Player", Chips, fixedLast);
            Winner(botOneType, botOnePower, "Bot 1", Bot1Chips, fixedLast);
            Winner(botTwoType, botTwoPower, "Bot 2", Bot2Chips, fixedLast);
            Winner(botThreeType, botThreePower, "Bot 3", Bot3Chips, fixedLast);
            Winner(botFourType, botFourPower, "Bot 4", Bot4Chips, fixedLast);
            Winner(botFiveType, botFivePower, "Bot 5", Bot5Chips, fixedLast);
        }
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
                _holder[c1].Visible = false;
                _holder[c2].Visible = false;
            }
        }
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

        private void Fold(ref bool sTurn, ref bool sFTurn, Label sStatus)
        {
            _raising = false;
            sStatus.Text = "Fold";
            sTurn = false;
            sFTurn = true;
        }
        private void Check(ref bool cTurn, Label cStatus)
        {
            cStatus.Text = "Check";
            cTurn = false;
            _raising = false;
        }
        private void Call(ref int sChips, ref bool sTurn, Label sStatus)
        {
            _raising = false;
            sTurn = false;
            sChips -= call;
            sStatus.Text = "Call " + call;
            tbPot.Text = (int.Parse(tbPot.Text) + call).ToString();
        }
        private void Raised(ref int sChips, ref bool sTurn, Label sStatus)
        {
            sChips -= Convert.ToInt32(raise);
            sStatus.Text = "Raise " + raise;
            tbPot.Text = (int.Parse(tbPot.Text) + Convert.ToInt32(raise)).ToString();
            call = Convert.ToInt32(raise);
            _raising = true;
            sTurn = false;
        }
        private static double RoundN(int sChips, int n)
        {
            double a = Math.Round((sChips / n) / 100d, 0) * 100;
            return a;
        }
        private void Hp(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower, int n, int n1)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 4);
            if (call <= 0)
            {
                Check(ref sTurn, sStatus);
            }
            if (call > 0)
            {
                if (rnd == 1)
                {
                    if (call <= RoundN(sChips, n))
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
                    if (call <= RoundN(sChips, n1))
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
                    raise = call * 2;
                    Raised(ref sChips, ref sTurn, sStatus);
                }
                else
                {
                    if (raise <= RoundN(sChips, n))
                    {
                        raise = call * 2;
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
                if (call <= 0)
                {
                    Check(ref sTurn, sStatus);
                }
                if (call > 0)
                {
                    if (call >= RoundN(sChips, n1))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                    if (raise > RoundN(sChips, n))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                    if (!sFTurn)
                    {
                        if (call >= RoundN(sChips, n) && call <= RoundN(sChips, n1))
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
                                raise = RoundN(sChips, n);
                                Raised(ref sChips, ref sTurn, sStatus);
                            }
                            else
                            {
                                raise = call * 2;
                                Raised(ref sChips, ref sTurn, sStatus);
                            }
                        }

                    }
                }
            }
            if (rounds >= 2)
            {
                if (call > 0)
                {
                    if (call >= RoundN(sChips, n1 - rnd))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                    if (raise > RoundN(sChips, n - rnd))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                    if (!sFTurn)
                    {
                        if (call >= RoundN(sChips, n - rnd) && call <= RoundN(sChips, n1 - rnd))
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
                                raise = RoundN(sChips, n - rnd);
                                Raised(ref sChips, ref sTurn, sStatus);
                            }
                            else
                            {
                                raise = call * 2;
                                Raised(ref sChips, ref sTurn, sStatus);
                            }
                        }
                    }
                }
                if (call <= 0)
                {
                    raise = RoundN(sChips, r - rnd);
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
            if (call <= 0)
            {
                Check(ref botTurn, botStatus);
            }
            else
            {
                if (call >= RoundN(botChips, n))
                {
                    if (botChips > call)
                    {
                        Call(ref botChips, ref botTurn, botStatus);
                    }
                    else if (botChips <= call)
                    {
                        _raising = false;
                        botTurn = false;
                        botChips = 0;
                        botStatus.Text = "Call " + botChips;
                        tbPot.Text = (int.Parse(tbPot.Text) + botChips).ToString();
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
                        raise = call * 2;
                        Raised(ref botChips, ref botTurn, botStatus);
                    }
                }
            }
            if (botChips <= 0)
            {
                botFTurn = true;
            }
        }

        #region UI
        private async void TimerTick(object sender, object e)
        {
            if (pbTimer.Value <= 0)
            {
                _pFturn = true;
                await Turns();
            }
            if (_t > 0)
            {
                _t--;
                pbTimer.Value = (_t / 6) * 100;
            }
        }
        private void UpdateTick(object sender, object e)
        {
            if (Chips <= 0)
            {
                tbChips.Text = "Chips : 0";
            }
            if (Bot1Chips <= 0)
            {
                tbBotChips1.Text = "Chips : 0";
            }
            if (Bot2Chips <= 0)
            {
                tbBotChips2.Text = "Chips : 0";
            }
            if (Bot3Chips <= 0)
            {
                tbBotChips3.Text = "Chips : 0";
            }
            if (Bot4Chips <= 0)
            {
                tbBotChips4.Text = "Chips : 0";
            }
            if (Bot5Chips <= 0)
            {
                tbBotChips5.Text = "Chips : 0";
            }
            tbChips.Text = "Chips : " + Chips.ToString();
            tbBotChips1.Text = "Chips : " + Bot1Chips.ToString();
            tbBotChips2.Text = "Chips : " + Bot2Chips.ToString();
            tbBotChips3.Text = "Chips : " + Bot3Chips.ToString();
            tbBotChips4.Text = "Chips : " + Bot4Chips.ToString();
            tbBotChips5.Text = "Chips : " + Bot5Chips.ToString();
            if (Chips <= 0)
            {
                _pturn = false;
                _pFturn = true;
                bCall.Enabled = false;
                bRaise.Enabled = false;
                bFold.Enabled = false;
                bCheck.Enabled = false;
            }
            if (_up > 0)
            {
                _up--;
            }
            if (Chips >= call)
            {
                bCall.Text = "Call " + call.ToString();
            }
            else
            {
                bCall.Text = "All in";
                bRaise.Enabled = false;
            }
            if (call > 0)
            {
                bCheck.Enabled = false;
            }
            if (call <= 0)
            {
                bCheck.Enabled = true;
                bCall.Text = "Call";
                bCall.Enabled = false;
            }
            if (Chips <= 0)
            {
                bRaise.Enabled = false;
            }
            int parsedValue;

            if (tbRaise.Text != "" && int.TryParse(tbRaise.Text, out parsedValue))
            {
                if (Chips <= int.Parse(tbRaise.Text))
                {
                    bRaise.Text = "All in";
                }
                else
                {
                    bRaise.Text = "Raise";
                }
            }
            if (Chips < call)
            {
                bRaise.Enabled = false;
            }
        }

        private async void bFold_Click(object sender, EventArgs e)
        {
            pStatus.Text = "Fold";
            _pturn = false;
            _pFturn = true;
            await Turns();
        }

        private async void bCheck_Click(object sender, EventArgs e)
        {
            if (call <= 0)
            {
                _pturn = false;
                pStatus.Text = "Check";
            }
            else
            {
                //pStatus.Text = "All in " + Chips;

                bCheck.Enabled = false;
            }
            await Turns();
        }
        private async void bCall_Click(object sender, EventArgs e)
        {
            Rules(0, 1, "Player", ref playerType, ref playerPower, _pFturn);
            if (Chips >= call)
            {
                Chips -= call;
                tbChips.Text = "Chips : " + Chips.ToString();
                if (tbPot.Text != "")
                {
                    tbPot.Text = (int.Parse(tbPot.Text) + call).ToString();
                }
                else
                {
                    tbPot.Text = call.ToString();
                }
                _pturn = false;
                pStatus.Text = "Call " + call;
                _pCall = call;
            }
            else if (Chips <= call && call > 0)
            {
                tbPot.Text = (int.Parse(tbPot.Text) + Chips).ToString();
                pStatus.Text = "All in " + Chips;
                Chips = 0;
                tbChips.Text = "Chips : " + Chips.ToString();
                _pturn = false;
                bFold.Enabled = false;
                _pCall = Chips;
            }
            await Turns();
        }
        private async void bRaise_Click(object sender, EventArgs e)
        {
            Rules(0, 1, "Player", ref playerType, ref playerPower, _pFturn);
            int parsedValue;
            if (tbRaise.Text != "" && int.TryParse(tbRaise.Text, out parsedValue))
            {
                if (Chips > call)
                {
                    if (raise * 2 > int.Parse(tbRaise.Text))
                    {
                        tbRaise.Text = (raise * 2).ToString();
                        MessageBox.Show("You must raise atleast twice as the current raise !");
                        return;
                    }
                    else
                    {
                        if (Chips >= int.Parse(tbRaise.Text))
                        {
                            call = int.Parse(tbRaise.Text);
                            raise = int.Parse(tbRaise.Text);
                            pStatus.Text = "Raise " + call.ToString();
                            tbPot.Text = (int.Parse(tbPot.Text) + call).ToString();
                            bCall.Text = "Call";
                            Chips -= int.Parse(tbRaise.Text);
                            _raising = true;
                            _last = 0;
                            _pRaise = Convert.ToInt32(raise);
                        }
                        else
                        {
                            call = Chips;
                            raise = Chips;
                            tbPot.Text = (int.Parse(tbPot.Text) + Chips).ToString();
                            pStatus.Text = "Raise " + call.ToString();
                            Chips = 0;
                            _raising = true;
                            _last = 0;
                            _pRaise = Convert.ToInt32(raise);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("This is a number only field");
                return;
            }
            _pturn = false;
            await Turns();
        }
        private void bAdd_Click(object sender, EventArgs e)
        {
            if (tbAdd.Text == "") { }
            else
            {
                Chips += int.Parse(tbAdd.Text);
                Bot1Chips += int.Parse(tbAdd.Text);
                Bot2Chips += int.Parse(tbAdd.Text);
                Bot3Chips += int.Parse(tbAdd.Text);
                Bot4Chips += int.Parse(tbAdd.Text);
                Bot5Chips += int.Parse(tbAdd.Text);
            }
            tbChips.Text = "Chips : " + Chips.ToString();
        }
        private void bOptions_Click(object sender, EventArgs e)
        {
            tbBB.Text = bb.ToString();
            tbSB.Text = _sb.ToString();
            if (tbBB.Visible == false)
            {
                tbBB.Visible = true;
                tbSB.Visible = true;
                bBB.Visible = true;
                bSB.Visible = true;
            }
            else
            {
                tbBB.Visible = false;
                tbSB.Visible = false;
                bBB.Visible = false;
                bSB.Visible = false;
            }
        }
        private void bSB_Click(object sender, EventArgs e)
        {
            int parsedValue;
            if (tbSB.Text.Contains(",") || tbSB.Text.Contains("."))
            {
                MessageBox.Show("The Small Blind can be only round number !");
                tbSB.Text = _sb.ToString();
                return;
            }
            if (!int.TryParse(tbSB.Text, out parsedValue))
            {
                MessageBox.Show("This is a number only field");
                tbSB.Text = _sb.ToString();
                return;
            }
            if (int.Parse(tbSB.Text) > 100000)
            {
                MessageBox.Show("The maximum of the Small Blind is 100 000 $");
                tbSB.Text = _sb.ToString();
            }
            if (int.Parse(tbSB.Text) < 250)
            {
                MessageBox.Show("The minimum of the Small Blind is 250 $");
            }
            if (int.Parse(tbSB.Text) >= 250 && int.Parse(tbSB.Text) <= 100000)
            {
                _sb = int.Parse(tbSB.Text);
                MessageBox.Show("The changes have been saved ! They will become available the next hand you play. ");
            }
        }
        private void bBB_Click(object sender, EventArgs e)
        {
            int parsedValue;
            if (tbBB.Text.Contains(",") || tbBB.Text.Contains("."))
            {
                MessageBox.Show("The Big Blind can be only round number !");
                tbBB.Text = bb.ToString();
                return;
            }
            if (!int.TryParse(tbSB.Text, out parsedValue))
            {
                MessageBox.Show("This is a number only field");
                tbSB.Text = bb.ToString();
                return;
            }
            if (int.Parse(tbBB.Text) > 200000)
            {
                MessageBox.Show("The maximum of the Big Blind is 200 000");
                tbBB.Text = bb.ToString();
            }
            if (int.Parse(tbBB.Text) < 500)
            {
                MessageBox.Show("The minimum of the Big Blind is 500 $");
            }
            if (int.Parse(tbBB.Text) >= 500 && int.Parse(tbBB.Text) <= 200000)
            {
                bb = int.Parse(tbBB.Text);
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
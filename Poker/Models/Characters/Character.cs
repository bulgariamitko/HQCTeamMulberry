namespace Poker.Models.Characters
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;
    using Interfaces;
    using System;

    public abstract class Character : ICharacter
    {
        protected const int DefaultStartChips = 10000;
        protected const int DefaultPlayerPanelHeight = 150;
        protected const int DefaultPlayerPanelWidth = 180;

        private string name;
        private int chips;
        private Panel panel;
        private double type;
        private double power;
        private int call;
        private int raise;
        private bool canMakeTurn;
        private bool outOfChips;
        private bool folded;
        private IList<ICard> cards;
        private int startCard;

        protected Character(string name)
        {
            this.Name = name;
            this.Chips = DefaultStartChips;
            this.Panel = new Panel();
            this.Type = -1;
            this.Power = 0;
            this.Call = 0;
            this.Raise = 0;
            this.CanMakeTurn = false;
            this.OutOfChips = false;
            this.Folded = false;
            
            this.Cards = new List<ICard>
            {
                new Card(),
                new Card()
            };

        }

        public string Name
        {
            get
            {
                return this.name;
            }

            private set
            {
                this.ValidateForEmptyOrNullString(value, "name");
                this.name = value;
            }
        }

        public int Chips
        {
            get
            {
                return this.chips;
            }

            set
            {
                this.ValidateForNull(value, "Chips");
                if (value < 0)
                {
                    this.chips = 0;
                }
                else
                {
                    this.chips = value;
                }
            }
        }

        public Panel Panel
        {
            get
            {
                return this.panel;
            }

            set
            {
                this.ValidateForNull(value, "Panel");
                this.panel = value;
            }
        }

        public double Type
        {
            get
            {
                return this.type;
            }

            set
            {
                this.ValidateForNull(value, "Type");
                this.type = value;
            }
        }

        public double Power
        {
            get
            {
                return this.power;
            }

            set
            {
                this.ValidateForNull(value, "Power");
                this.power = value;
            }
        }

        public int Call
        {
            get
            {
                return this.call;
            }

            set
            {
                this.ValidateForNull(value, "Call");
                this.call = value;
            }
        }

        public int Raise
        {
            get
            {
                return this.raise;
            }

            set
            {
                this.ValidateForNull(value, "Raise");
                this.raise = value;
            }
            
        }

        public bool CanMakeTurn
        {
            get
            {
                return this.canMakeTurn;
            }

            set
            {
                this.ValidateForNull(value, "CanMakeTurn");
                this.canMakeTurn = value;
            }
        }

        public bool OutOfChips
        {
            get
            {
                return this.outOfChips;
            }

            set
            {
                this.ValidateForNull(value, "OutOfChips");
                this.outOfChips = value;
            }
        }

        public bool Folded
        {
            get
            {
                return this.folded;
            }

            set
            {
                this.ValidateForNull(value, "Folded");
                this.folded = value;
            }
        }

        public IList<ICard> Cards
        {
            get
            {
                return this.cards;
            }

            protected set
            {
                this.ValidateForNull(value, "Cards");
                this.cards = value;
            }
        }

        public int StartCard
        {
            get
            {
                return this.startCard;
            }

            set
            {
                this.ValidateForNull(value, "Start card");
                this.ValidateForNegativeNumber(value, "Start card");
                if (value > MainWindow.DefaultSetOfCards - 1)
                {
                    throw new ArgumentOutOfRangeException("Start card cannot be bigger then DefaultSetOfCards");
                }
                this.startCard = value;
            }
        }
        
        public void InitializePanel(Point location)
        {
            this.Panel.Location = location;
            this.Panel.BackColor = Color.FromArgb(16, 255, 0);
            this.Panel.Height = DefaultPlayerPanelHeight;
            this.Panel.Width = DefaultPlayerPanelWidth;
            this.Panel.Visible = false;
        }

        protected void ValidateForNull<T>(T value, string propertyName)
        {
            if (value == null)
            {
                var msg = string.Format("{0} cannot be null.", propertyName);
                throw new ArgumentNullException(msg);
            }
        }

        protected void ValidateForNegativeNumber(int value, string propertyName)
        {
            if (value < 0)
            {
                var msg = string.Format("{0} cannot be negative.", propertyName);
                throw new ArgumentOutOfRangeException(msg);
            }
        }

        protected void ValidateForEmptyOrNullString(string value, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                var msg = string.Format("{0} cannot be null or white space.", propertyName);
                throw new ArgumentNullException(msg);
            }
        }
    }
}

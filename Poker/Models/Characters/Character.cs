namespace Poker.Models.Characters
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;
    using Poker.Interfaces;
    using Poker.UserInterface;
    using Poker.Validators;

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

        protected Character(string name, int startCard = 0)
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
            this.StartCard = startCard;

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
                PropertyValueValidator.ValidateForEmptyOrNullString(value, "name");
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
                PropertyValueValidator.ValidateForNull(value, "Chips");
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
                PropertyValueValidator.ValidateForNull(value, "Panel");
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
                PropertyValueValidator.ValidateForNull(value, "Type");
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
                PropertyValueValidator.ValidateForNull(value, "Power");
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
                PropertyValueValidator.ValidateForNull(value, "Call");
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
                PropertyValueValidator.ValidateForNull(value, "Raise");
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
                PropertyValueValidator.ValidateForNull(value, "CanMakeTurn");
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
                PropertyValueValidator.ValidateForNull(value, "OutOfChips");
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
                PropertyValueValidator.ValidateForNull(value, "Folded");
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
                PropertyValueValidator.ValidateForNull(value, "Cards");
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
                PropertyValueValidator.ValidateForNull(value, "Start card");
                PropertyValueValidator.ValidateForNegativeNumber(value, "Start card");
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
    }
}

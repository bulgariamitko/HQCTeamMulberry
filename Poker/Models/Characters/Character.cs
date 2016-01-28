// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Character.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the Character type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Poker.Models.Characters
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;
    using Poker.Interfaces;
    using Poker.UserInterface;
    using Poker.Validators;

    /// <summary>
    /// The base class for all players.
    /// </summary>
    public abstract class Character : ICharacter
    {
        /// <summary>
        /// The default start chips.
        /// </summary>
        protected const int DefaultStartChips = 10000;

        /// <summary>
        /// The default player panel height.
        /// </summary>
        protected const int DefaultPlayerPanelHeight = 150;

        /// <summary>
        /// The default player panel width.
        /// </summary>
        protected const int DefaultPlayerPanelWidth = 180;

        /// <summary>
        /// The name.
        /// </summary>
        private string name;

        /// <summary>
        /// The chips.
        /// </summary>
        private int chips;

        /// <summary>
        /// The panel.
        /// </summary>
        private Panel panel;

        /// <summary>
        /// The type.
        /// </summary>
        private double type;

        /// <summary>
        /// The power.
        /// </summary>
        private double power;

        /// <summary>
        /// The call.
        /// </summary>
        private int call;

        /// <summary>
        /// The raise.
        /// </summary>
        private int raise;

        /// <summary>
        /// The can make turn.
        /// </summary>
        private bool canMakeTurn;

        /// <summary>
        /// The out of chips.
        /// </summary>
        private bool outOfChips;

        /// <summary>
        /// The folded.
        /// </summary>
        private bool folded;

        /// <summary>
        /// Player cards.
        /// </summary>
        private IList<ICard> cards;

        /// <summary>
        /// The start card.
        /// </summary>
        private int startCard;

        /// <summary>
        /// Initializes a new instance of the <see cref="Character"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="startCard">
        /// The start card.
        /// </param>
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

        /// <summary>
        /// Gets the name.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the chips.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the panel.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the power.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the call.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the raise.
        /// </summary>
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

        /// <summary>
        /// Gets or sets a value indicating whether can make turn.
        /// </summary>
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

        /// <summary>
        /// Gets or sets a value indicating whether out of chips.
        /// </summary>
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

        /// <summary>
        /// Gets or sets a value indicating whether folded.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the cards.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the start card.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// </exception>
        public int StartCard
        {
            get
            {
                return this.startCard;
            }

            set
            {
                PropertyValueValidator.ValidateForNull(value, "Start card");
                PropertyValueValidator.ValidateForNegativeIntNumber(value, "Start card");
                if (value > MainWindow.DefaultSetOfCards - 1)
                {
                    throw new ArgumentOutOfRangeException("Start card cannot be bigger then DefaultSetOfCards");
                }
                this.startCard = value;
            }
        }

        /// <summary>
        /// The panel initializer.
        /// </summary>
        /// <param name="location">
        /// The location.
        /// </param>
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

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Bot.cs" company="">
//   
// </copyright>
// <summary>
//   The bot.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Poker.Models.Characters
{
    using System.Windows.Forms;
    using Interfaces;
    using Poker.Validators;

    /// <summary>
    /// The bot.
    /// </summary>
    public class Bot : Character, IBot
    {
        /// <summary>
        /// The vertical location coordinate.
        /// </summary>
        private int verticalLocationCoordinate;

        /// <summary>
        /// The horizontal location coordinate.
        /// </summary>
        private int horizontalLocationCoordinate;

        /// <summary>
        /// The vertical location.
        /// </summary>
        private AnchorStyles verticalLocation;

        /// <summary>
        /// The horizontal location.
        /// </summary>
        private AnchorStyles horizontalLocation;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bot"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="startCard">
        /// The start card.
        /// </param>
        /// <param name="verticalLocationCoordinate">
        /// The vertical location coordinate.
        /// </param>
        /// <param name="horizontalLocationCoordinate">
        /// The horizontal location coordinate.
        /// </param>
        /// <param name="verticalLocation">
        /// The vertical location.
        /// </param>
        /// <param name="horizontalLocation">
        /// The horizontal location.
        /// </param>
        public Bot(string name, int startCard, int verticalLocationCoordinate, int horizontalLocationCoordinate, AnchorStyles verticalLocation = 0, AnchorStyles horizontalLocation = 0)
            : base(name, startCard)
        {
            this.VerticalLocationCoordinate = verticalLocationCoordinate;
            this.HorizontalLocationCoordinate = horizontalLocationCoordinate;
            this.VerticalLocation = verticalLocation;
            this.HorizontalLocation = horizontalLocation;
        }

        /// <summary>
        /// Gets or sets the vertical location coordinate.
        /// </summary>
        public int VerticalLocationCoordinate
        {
            get
            {
                return this.verticalLocationCoordinate;
            }

            set
            {
                PropertyValueValidator.ValidateForNegativeIntNumber(value, "Vertical location coordinate");
                this.verticalLocationCoordinate = value;
            }
        }

        /// <summary>
        /// Gets or sets the horizontal location coordinate.
        /// </summary>
        public int HorizontalLocationCoordinate
        {
            get
            {
                return this.horizontalLocationCoordinate;
            }

            set
            {
                PropertyValueValidator.ValidateForNegativeIntNumber(value, "Horizontal location coordinate");
                this.horizontalLocationCoordinate = value;
            }
        }

        /// <summary>
        /// Gets or sets the vertical location.
        /// </summary>
        public AnchorStyles VerticalLocation
        {
            get
            {
                return this.verticalLocation;
            }

            set
            {
                this.verticalLocation = value;
            }
        }

        /// <summary>
        /// Gets or sets the horizontal location.
        /// </summary>
        public AnchorStyles HorizontalLocation
        {
            get
            {
                return this.horizontalLocation;
            }

            set
            {
                this.horizontalLocation = value;
            }
        }
    }
}

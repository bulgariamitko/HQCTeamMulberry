namespace Poker.Models.Characters
{
    using System;
    using System.Windows.Forms;
    using Interfaces;

    public class Bot : Character, IBot
    {
        private int verticalLocationCoordinate;
        private int horizontalLocationCoordinate;
        private AnchorStyles verticalLocation;
        private AnchorStyles horizontalLocation;

        public Bot(string name, int startCard, int verticalLocationCoordinate, int horizontalLocationCoordinate, 
            AnchorStyles verticalLocation = 0, AnchorStyles horizontalLocation = 0)
            : base(name, startCard)
        {
            this.VerticalLocationCoordinate = verticalLocationCoordinate;
            this.HorizontalLocationCoordinate = horizontalLocationCoordinate;
            this.VerticalLocation = verticalLocation;
            this.HorizontalLocation = horizontalLocation;
        }

        public int VerticalLocationCoordinate
        {
            get
            {
                return this.verticalLocationCoordinate;
            }

            set
            {
                this.ValidateForNull(value, "Vertical location coordinate");
                this.ValidateForNegativeNumber(value, "Vertical location coordinate");
                this.verticalLocationCoordinate = value;
            }
        }

        public int HorizontalLocationCoordinate
        {
            get
            {
                return this.horizontalLocationCoordinate;
            }

            set
            {
                this.ValidateForNull(value, "Horizontal location coordinate");
                this.ValidateForNegativeNumber(value, "Horizontal location coordinate");
                this.horizontalLocationCoordinate = value;
            }
        }

        public AnchorStyles VerticalLocation
        {
            get
            {
                return this.verticalLocation;
            }

            set
            {
                this.ValidateForNull(value, "Vertical location");
                this.verticalLocation = value;
            }
        }

        public AnchorStyles HorizontalLocation
        {
            get
            {
                return this.horizontalLocation;
            }

            set
            {
                this.ValidateForNull(value, "Horizontal location");
                this.horizontalLocation = value;
            }
        }

        public AnchorStyles GetAnchorStyles()
        {
            if (this.VerticalLocation == 0)
            {
                return this.HorizontalLocation;
            }
            else if (this.HorizontalLocation == 0)
            {
                return this.VerticalLocation;
            }
            else
            {
                return this.VerticalLocation | this.HorizontalLocation;
            }
        }
    }
}

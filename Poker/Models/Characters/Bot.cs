﻿namespace Poker.Models.Characters
{
    using Interfaces;
    using System.Windows.Forms;

    public class Bot : Character, IBot
    {
        public Bot(string name, int verticalLocationCoordinate, int horizontalLocationCoordinate,
            AnchorStyles verticalLocation = 0, AnchorStyles horizontalLocation = 0)
            : base(name)
        {
            this.VerticalLocationCoordinate = verticalLocationCoordinate;
            this.HorizontalLocationCoordinate = horizontalLocationCoordinate;
            this.VerticalLocation = verticalLocation;
            this.HorizontalLocation = horizontalLocation;
        }

        public int VerticalLocationCoordinate { get; set; }

        public int HorizontalLocationCoordinate { get; set; }

        public AnchorStyles VerticalLocation { get; set; }

        public AnchorStyles HorizontalLocation { get; set; }

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

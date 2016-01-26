namespace Poker.Models.Characters
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    using Interfaces;

    public abstract class Character : ICharacter
    {
        private const int DefaultStartChips = 10000;
        private const int DefaultPlayerPanelHeight = 150;
        private const int DefaultPlayerPanelWidth = 180;

        private int chips;

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
        public string Name { get; set; }

        public double Power { get; set; }

        public int Chips { get; set; }

        public IList<ICard> Cards { get; }

        public int StartCard { get; set; }

        public Panel Panel { get; set; }

        public double Type { get; set; }

        public void InitializePanel(Point location)
        {
            this.Panel.Location = location;
            this.Panel.BackColor = Color.DarkBlue;
            this.Panel.Height = DefaultPlayerPanelHeight;
            this.Panel.Width = DefaultPlayerPanelWidth;
            this.Panel.Visible = false;
        }

        public int Call { get; set; }

        public int Raise { get; set; }

        public bool CanMakeTurn { get; set; }

        public bool OutOfChips { get; set; }

        public bool Folded { get; set; }

        public void Check()
        {
            throw new System.NotImplementedException();
        }

        public void Fold()
        {
            throw new System.NotImplementedException();
        }

        public void AllIn()
        {
            throw new System.NotImplementedException();
        }
    }
}

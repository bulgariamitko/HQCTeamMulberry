namespace Poker.Interfaces
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    public interface ICharacter
    {
        string Name { get; set; }

        double Power { get; set; }

        int Chips { get; set; }


        IList<ICard> Cards { get; }
        int StartCard { get; set; }
        Panel Panel { get; set; }
        double Type { get; set; }
        void InitializePanel(Point location);

        int Call { get; set; }

        int Raise { get; set; }

        bool CanMakeTurn { get; set; }

        bool OutOfChips { get; set; }

        bool Folded { get; set; }


        //void Call();

        //void Raise();

        void Check();

        void Fold();

        void AllIn();
    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICharacter.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ICharacter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Poker.Interfaces
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// The all players interface.
    /// </summary>
    public interface ICharacter
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets or sets the power.
        /// </summary>
        double Power { get; set; }

        /// <summary>
        /// Gets or sets the chips.
        /// </summary>
        int Chips { get; set; }

        /// <summary>
        /// Gets the cards.
        /// </summary>
        IList<ICard> Cards { get; }

        /// <summary>
        /// Gets or sets the start card.
        /// </summary>
        int StartCard { get; set; }

        /// <summary>
        /// Gets or sets the panel.
        /// </summary>
        Panel Panel { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        double Type { get; set; }

        /// <summary>
        /// Gets or sets the call.
        /// </summary>
        int Call { get; set; }

        /// <summary>
        /// Gets or sets the raise.
        /// </summary>
        int Raise { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether can make turn.
        /// </summary>
        bool CanMakeTurn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether out of chips.
        /// </summary>
        bool OutOfChips { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether folded.
        /// </summary>
        bool Folded { get; set; }

        /// <summary>
        /// The panel initializer.
        /// </summary>
        /// <param name="location">
        /// The location.
        /// </param>
        void InitializePanel(Point location);
    }
}

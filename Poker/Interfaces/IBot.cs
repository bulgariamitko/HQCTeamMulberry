// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBot.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the IBot type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Poker.Interfaces
{
    using System.Windows.Forms;

    /// <summary>
    /// The Bot interface.
    /// </summary>
    public interface IBot : ICharacter
    {
        /// <summary>
        /// Gets or sets the vertical location coordinate.
        /// </summary>
        int VerticalLocationCoordinate { get; set; }

        /// <summary>
        /// Gets or sets the horizontal location coordinate.
        /// </summary>
        int HorizontalLocationCoordinate { get; set; }

        /// <summary>
        /// Gets or sets the vertical location.
        /// </summary>
        AnchorStyles VerticalLocation { get; set; }

        /// <summary>
        /// Gets or sets the horizontal location.
        /// </summary>
        AnchorStyles HorizontalLocation { get; set; }
    }
}

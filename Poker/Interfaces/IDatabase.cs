// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDatabase.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the IDatabase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Poker.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
    /// The Database interface.
    /// </summary>
    public interface IDatabase
    {
        /// <summary>
        /// Gets or sets the chips.
        /// </summary>
        IList<int> Chips { get; set; }

        /// <summary>
        /// Gets or sets the check winners.
        /// </summary>
        IList<string> CheckWinners { get; set; }

        /// <summary>
        /// Gets or sets the win.
        /// </summary>
        IList<Poker.Type> Win { get; set; }

        /// <summary>
        /// Gets or sets the players game status.
        /// </summary>
        IList<bool?> PlayersGameStatus { get; set; }
    }
}

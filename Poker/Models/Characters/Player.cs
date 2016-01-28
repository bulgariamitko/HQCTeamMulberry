// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Player.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the Player type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Poker.Models.Characters
{
    using Interfaces;

    /// <summary>
    /// The player.
    /// </summary>
    public class Player : Character, IPlayer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        public Player(string name)
            : base(name)
        {
        }
    }
}
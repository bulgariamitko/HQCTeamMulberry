// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameDatabase.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the GameDatabase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Poker.Data
{
    using System.Collections.Generic;
    using Interfaces;
    using Poker.Validators;

    /// <summary>
    /// The game database.
    /// </summary>
    public class GameDatabase : IDatabase
    {
        /// <summary>
        /// The players game status.
        /// </summary>
        private IList<bool?> playersGameStatus;

        /// <summary>
        /// The win.
        /// </summary>
        private IList<Poker.Type> win;

        /// <summary>
        /// The check winners.
        /// </summary>
        private IList<string> checkWinners;

        /// <summary>
        /// The chips.
        /// </summary>
        private IList<int> chips;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameDatabase"/> class.
        /// </summary>
        public GameDatabase()
        {
            this.PlayersGameStatus = new List<bool?>();
            this.Win = new List<Poker.Type>();
            this.CheckWinners = new List<string>();
            this.Chips = new List<int>();
        }

        /// <summary>
        /// Gets or sets the players game status.
        /// </summary>
        public IList<bool?> PlayersGameStatus
        {
            get
            {
                return this.playersGameStatus;
            }

            set
            {
                PropertyValueValidator.ValidateForNull(value, "PlayersGameStatus");
                this.playersGameStatus = value;
            }
        }

        /// <summary>
        /// Gets or sets the win.
        /// </summary>
        public IList<Poker.Type> Win
        {
            get
            {
                return this.win;
            }

            set
            {
                PropertyValueValidator.ValidateForNull(value, "Win");
                this.win = value;
            }
        }

        /// <summary>
        /// Gets or sets the check winners.
        /// </summary>
        public IList<string> CheckWinners
        {
            get
            {
                return this.checkWinners;
            }

            set
            {
                PropertyValueValidator.ValidateForNull(value, "CheckWinners");
                this.checkWinners = value;
            }
        }

        /// <summary>
        /// Gets or sets the chips.
        /// </summary>
        public IList<int> Chips
        {
            get
            {
                return this.chips;
            }

            set
            {
                PropertyValueValidator.ValidateForNull(value, "Chips");
                this.chips = value;
            }
        }
    }
}

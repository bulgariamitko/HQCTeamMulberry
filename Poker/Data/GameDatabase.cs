namespace Poker.Data
{
    using System.Collections.Generic;
    using Interfaces;
    using System;

    public class GameDatabase : IDatabase
    {
        private IList<bool?> playersGameStatus;
        private IList<Poker.Type> win;
        private IList<string> checkWinners;
        private IList<int> chips;

        public GameDatabase()
        {
            this.PlayersGameStatus = new List<bool?>();
            this.Win = new List<Poker.Type>();
            this.CheckWinners = new List<string>();
            this.Chips = new List<int>();
        }

        public IList<bool?> PlayersGameStatus
        {
            get
            {
                return this.playersGameStatus;
            }

            set
            {
                this.ValidateForNull(value, "PlayersGameStatus");
                this.playersGameStatus = value;
            }
        }

        public IList<Poker.Type> Win
        {
            get
            {
                return this.win;
            }

            set
            {
                this.ValidateForNull(value, "Win");
                this.win = value;
            }
        }

        public IList<string> CheckWinners
        {
            get
            {
                return this.checkWinners;
            }

            set
            {
                this.ValidateForNull(value, "CheckWinners");
                this.checkWinners = value;
            }
        }

        public IList<int> Chips
        {
            get
            {
                return this.chips;
            }

            set
            {
                this.ValidateForNull(value, "Chips");
                this.chips = value;
            }
        }

        protected void ValidateForNull<T>(T value, string propertyName)
        {
            if (value == null)
            {
                var msg = string.Format("{0} cannot be null.", propertyName);
                throw new ArgumentNullException(msg);
            }
        }
    }
}

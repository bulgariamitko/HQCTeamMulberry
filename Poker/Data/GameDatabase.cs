namespace Poker.Data
{
    using System.Collections.Generic;
    using Interfaces;
    using System;
    using Poker.Models;
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

        public IList<bool?> PlayersGameStatus { get; set; }

        public IList<Poker.Type> Win { get; set; }
        
        public IList<string> CheckWinners { get; set; }

        public IList<int> Chips { get; set; }
        
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

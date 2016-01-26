namespace Poker.Data
{
    using System.Collections.Generic;
    using Interfaces;

    public class GameDatabase : IDatabase
    {
        ////TODO: Check if name is proper, previous name - bools

        public GameDatabase()
        {
            this.PlayersGameStatus = new List<bool?>();
            this.Win = new List<Type>();
            this.CheckWinners = new List<string>();
            this.Chips = new List<int>();
        }

        public List<int> Chips { get; set; }

        public List<string> CheckWinners { get; set; }

        public List<Type> Win { get; set; }

        public List<bool?> PlayersGameStatus { get; set; }
    }
}

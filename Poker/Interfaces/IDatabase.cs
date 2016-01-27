namespace Poker.Interfaces
{
    using System.Collections.Generic;

    public interface IDatabase
    {
        IList<int> Chips { get; set; }

        IList<string> CheckWinners { get; set; }

        IList<Poker.Type> Win { get; set; }

        IList<bool?> PlayersGameStatus { get; set; }
    }
}

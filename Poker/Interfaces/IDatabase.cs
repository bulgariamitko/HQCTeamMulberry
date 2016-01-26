﻿namespace Poker.Interfaces
{
    using System.Collections.Generic;

    public interface IDatabase
    {
        List<int> Chips { get; set; }

        List<string> CheckWinners { get; set; }

        List<Type> Win { get; set; }

        List<bool?> PlayersGameStatus { get; set; }
    }
}

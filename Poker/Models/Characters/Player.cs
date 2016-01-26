namespace Poker.Models.Characters
{
    using Poker.Interfaces;

    public class Player : Character , IPlayer
    {
        public Player(string name)
            : base(name)
        {
        }
    }
}

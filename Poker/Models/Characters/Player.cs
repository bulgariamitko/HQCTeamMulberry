namespace Poker.Models.Characters
{
    using Interfaces;

    public class Player : Character, IPlayer
    {
        public Player(string name)
            : base(name)
        {
        }
    }
}
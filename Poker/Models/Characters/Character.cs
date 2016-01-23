namespace Poker.Models.Characters
{
    using Interfaces;

    public abstract class Character : ICharacter
    {
        public string Name { get; set; }

        public int Power { get; set; }

        public int Chips { get; set; }

        public virtual void Call()
        {
            throw new System.NotImplementedException();
        }

        public virtual void Raise()
        {
            throw new System.NotImplementedException();
        }

        public virtual void Check()
        {
            throw new System.NotImplementedException();
        }

        public virtual void Fold()
        {
            throw new System.NotImplementedException();
        }

        public void AllIn()
        {
            throw new System.NotImplementedException();
        }
    }
}

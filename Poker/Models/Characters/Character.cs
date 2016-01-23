namespace Poker.Models.Characters
{
    using Interfaces;

    public abstract class Character : ICharacter
    {
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
    }
}

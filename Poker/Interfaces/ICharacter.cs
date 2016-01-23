namespace Poker.Interfaces
{
    public interface ICharacter
    {
        void Call();

        void Raise();

        void Check();

        void Fold();
    }
}

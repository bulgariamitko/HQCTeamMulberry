namespace Poker.Interfaces
{
    public interface ICharacter
    {
        string Name { get; set; }

        int Power { get; set; }

        int Chips { get; set; }

        void Call();

        void Raise();

        void Check();

        void Fold();

        void AllIn();
    }
}

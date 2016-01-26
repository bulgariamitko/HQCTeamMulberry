namespace Poker.Interfaces
{
    using System.Windows.Forms;

    public interface IWinningHandType
    {
        void HighCard(ICharacter player, Label sStatus, ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising);

        void PairTable(ICharacter player, Label sStatus, ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising);

        void PairHand(ICharacter player, Label sStatus, ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds);

        void TwoPair(ICharacter player, Label sStatus, ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds);

        void ThreeOfAKind(ICharacter player, Label sStatus, int name, ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds);

        void Straight(ICharacter player, Label sStatus, int name, ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds);

        void Flush(ICharacter player, Label sStatus, int name, ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds);

        void FullHouse(ICharacter player, Label sStatus, int name, ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds);

        void FourOfAKind(ICharacter player, Label sStatus, int name, ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds);

        void StraightFlush(ICharacter player, Label sStatus, int name, ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds);
    }
}

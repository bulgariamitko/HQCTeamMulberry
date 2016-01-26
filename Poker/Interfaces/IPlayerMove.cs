namespace Poker.Interfaces
{
    using System.Windows.Forms;

    public interface IPlayerMove
    {
        void Fold(ICharacter pokerPlayer, Label sStatus, ref bool rising);

        void Check(ICharacter pokerPlayer, Label cStatus, ref bool raising);

        void Call(ICharacter pokerPlayer, Label sStatus, ref bool raising, ref int neededChipsToCall, TextBox potStatus);

        void Raised(ICharacter pokerPlayer, Label sStatus, ref bool raising, ref int raise, ref int neededChipsToCall, TextBox potStatus);

        void HP(ICharacter pokerPlayer, Label sStatus, int n, int n1,ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising);

        void PH(ICharacter player, Label sStatus, int n, int n1, int r,ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising,ref int rounds);
    }
}

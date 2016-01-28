// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPlayerMove.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the IPlayerMove type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Poker.Interfaces
{
    using System.Windows.Forms;

    /// <summary>
    /// The PlayerMove interface.
    /// </summary>
    public interface IPlayerMove
    {
        /// <summary>
        /// The fold.
        /// </summary>
        /// <param name="pokerPlayer">
        /// The poker player.
        /// </param>
        /// <param name="sStatus">
        /// The s status.
        /// </param>
        /// <param name="rising">
        /// The rising.
        /// </param>
        void Fold(ICharacter pokerPlayer, Label sStatus, ref bool rising);

        /// <summary>
        /// The check.
        /// </summary>
        /// <param name="pokerPlayer">
        /// The poker player.
        /// </param>
        /// <param name="cStatus">
        /// The c status.
        /// </param>
        /// <param name="raising">
        /// The raising.
        /// </param>
        void Check(ICharacter pokerPlayer, Label cStatus, ref bool raising);

        /// <summary>
        /// The call.
        /// </summary>
        /// <param name="pokerPlayer">
        /// The poker player.
        /// </param>
        /// <param name="sStatus">
        /// The s status.
        /// </param>
        /// <param name="raising">
        /// The raising.
        /// </param>
        /// <param name="neededChipsToCall">
        /// The needed chips to call.
        /// </param>
        /// <param name="potStatus">
        /// The pot status.
        /// </param>
        void Call(ICharacter pokerPlayer, Label sStatus, ref bool raising, ref int neededChipsToCall, TextBox potStatus);

        /// <summary>
        /// The raised.
        /// </summary>
        /// <param name="pokerPlayer">
        /// The poker player.
        /// </param>
        /// <param name="sStatus">
        /// The s status.
        /// </param>
        /// <param name="raising">
        /// The raising.
        /// </param>
        /// <param name="raise">
        /// The raise.
        /// </param>
        /// <param name="neededChipsToCall">
        /// The needed chips to call.
        /// </param>
        /// <param name="potStatus">
        /// The pot status.
        /// </param>
        void Raised(ICharacter pokerPlayer, Label sStatus, ref bool raising, ref int raise, ref int neededChipsToCall, TextBox potStatus);

        /// <summary>
        /// The hp.
        /// </summary>
        /// <param name="pokerPlayer">
        /// The poker player.
        /// </param>
        /// <param name="sStatus">
        /// The s status.
        /// </param>
        /// <param name="n">
        /// The n.
        /// </param>
        /// <param name="n1">
        /// The n 1.
        /// </param>
        /// <param name="neededChipsToCall">
        /// The needed chips to call.
        /// </param>
        /// <param name="potStatus">
        /// The pot status.
        /// </param>
        /// <param name="raise">
        /// The raise.
        /// </param>
        /// <param name="raising">
        /// The raising.
        /// </param>
        void HP(ICharacter pokerPlayer, Label sStatus, int n, int n1, ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, IRandomGenerator randomGenerator);

        /// <summary>
        /// The ph.
        /// </summary>
        /// <param name="player">
        /// The player.
        /// </param>
        /// <param name="sStatus">
        /// The s status.
        /// </param>
        /// <param name="n">
        /// The n.
        /// </param>
        /// <param name="n1">
        /// The n 1.
        /// </param>
        /// <param name="r">
        /// The r.
        /// </param>
        /// <param name="neededChipsToCall">
        /// The needed chips to call.
        /// </param>
        /// <param name="potStatus">
        /// The pot status.
        /// </param>
        /// <param name="raise">
        /// The raise.
        /// </param>
        /// <param name="raising">
        /// The raising.
        /// </param>
        /// <param name="rounds">
        /// The rounds.
        /// </param>
        void PH(ICharacter player, Label sStatus, int n, int n1, int r, ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds, IRandomGenerator randomGenerator);
    }
}

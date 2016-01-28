// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWinningHandType.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the IWinningHandType type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Poker.Interfaces
{
    using System.Windows.Forms;

    /// <summary>
    /// The WinningHandType interface.
    /// </summary>
    public interface IWinningHandType
    {
        /// <summary>
        /// The high card.
        /// </summary>
        /// <param name="player">
        /// The player.
        /// </param>
        /// <param name="sStatus">
        /// The s status.
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
        void HighCard(ICharacter player, Label sStatus, ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising);

        /// <summary>
        /// The pair table.
        /// </summary>
        /// <param name="player">
        /// The player.
        /// </param>
        /// <param name="sStatus">
        /// The s status.
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
        void PairTable(ICharacter player, Label sStatus, ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising);

        /// <summary>
        /// The pair hand.
        /// </summary>
        /// <param name="player">
        /// The player.
        /// </param>
        /// <param name="sStatus">
        /// The s status.
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
        void PairHand(ICharacter player, Label sStatus, ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds);

        /// <summary>
        /// The two pair.
        /// </summary>
        /// <param name="player">
        /// The player.
        /// </param>
        /// <param name="sStatus">
        /// The s status.
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
        void TwoPair(ICharacter player, Label sStatus, ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds);

        /// <summary>
        /// The three of a kind.
        /// </summary>
        /// <param name="player">
        /// The player.
        /// </param>
        /// <param name="sStatus">
        /// The s status.
        /// </param>
        /// <param name="name">
        /// The name.
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
        void ThreeOfAKind(ICharacter player, Label sStatus, int name, ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds);

        /// <summary>
        /// The straight.
        /// </summary>
        /// <param name="player">
        /// The player.
        /// </param>
        /// <param name="sStatus">
        /// The s status.
        /// </param>
        /// <param name="name">
        /// The name.
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
        void Straight(ICharacter player, Label sStatus, int name, ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds);

        /// <summary>
        /// The flush.
        /// </summary>
        /// <param name="player">
        /// The player.
        /// </param>
        /// <param name="sStatus">
        /// The s status.
        /// </param>
        /// <param name="name">
        /// The name.
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
        void Flush(ICharacter player, Label sStatus, int name, ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds);

        /// <summary>
        /// The full house.
        /// </summary>
        /// <param name="player">
        /// The player.
        /// </param>
        /// <param name="sStatus">
        /// The s status.
        /// </param>
        /// <param name="name">
        /// The name.
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
        void FullHouse(ICharacter player, Label sStatus, int name, ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds);

        /// <summary>
        /// The four of a kind.
        /// </summary>
        /// <param name="player">
        /// The player.
        /// </param>
        /// <param name="sStatus">
        /// The s status.
        /// </param>
        /// <param name="name">
        /// The name.
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
        void FourOfAKind(ICharacter player, Label sStatus, int name, ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds);

        /// <summary>
        /// The straight flush.
        /// </summary>
        /// <param name="player">
        /// The player.
        /// </param>
        /// <param name="sStatus">
        /// The s status.
        /// </param>
        /// <param name="name">
        /// The name.
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
        void StraightFlush(ICharacter player, Label sStatus, int name, ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds);
    }
}

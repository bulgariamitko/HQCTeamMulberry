﻿namespace Poker.Models
{
    using System;
    using System.Windows.Forms;
    using Interfaces;

    public class WinningHandType : IWinningHandType
    {
        private readonly IPlayerMove playerMove;
        private readonly Random randomGenerator;

        public WinningHandType()
        {
            this.randomGenerator = new Random();
            this.playerMove = new PlayerMove();
        }

        public void HighCard(ICharacter pokerPlayer, Label sStatus, ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising)
        {
            this.playerMove.HP(pokerPlayer, sStatus, 20, 25,ref neededChipsToCall, potStatus, ref raise, ref raising);
        }

        public void PairTable(ICharacter pokerPlayer, Label sStatus, ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising)
        {
            this.playerMove.HP(pokerPlayer, sStatus, 16, 25,ref neededChipsToCall, potStatus, ref raise, ref raising);
            //Hp(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower, 16, 25);
        }

        public void PairHand(ICharacter player, Label sStatus, ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            int rCall = this.randomGenerator.Next(10, 16);
            int rRaise = this.randomGenerator.Next(10, 13);
            if (player.Power <= 199 && player.Power >= 140)
            {
                this.playerMove.PH(player, sStatus, rCall, 6, rRaise, ref neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }

            if (player.Power <= 139 && player.Power >= 128)
            {
                this.playerMove.PH(player, sStatus, rCall, 7, rRaise, ref neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }

            if (player.Power < 128 && player.Power >= 101)
            {
                this.playerMove.PH(player, sStatus, rCall, 9, rRaise, ref neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }
        }

        public void TwoPair(ICharacter player, Label sStatus, ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            int rCall = this.randomGenerator.Next(6, 11);
            int rRaise = this.randomGenerator.Next(6, 11);
            if (player.Power <= 290 && player.Power >= 246)
            {
                this.playerMove.PH(player, sStatus, rCall, 3, rRaise, ref neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }

            if (player.Power <= 244 && player.Power >= 234)
            {
                this.playerMove.PH(player, sStatus, rCall, 4, rRaise, ref neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }

            if (player.Power < 234 && player.Power >= 201)
            {
                this.playerMove.PH(player, sStatus, rCall, 4, rRaise, ref neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }
        }

        public void ThreeOfAKind(ICharacter player, Label sStatus, int name, ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            int tCall = this.randomGenerator.Next(3, 7);
            int tRaise = this.randomGenerator.Next(4, 8);
            if (player.Power <= 390 && player.Power >= 330)
            {
                this.Smooth(player, sStatus, name, tCall, tRaise, ref neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }

            if (player.Power <= 327 && player.Power >= 321)
            {
                this.Smooth(player, sStatus, name, tCall, tRaise, ref neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }

            if (player.Power < 321 && player.Power >= 303)
            {
                this.Smooth(player, sStatus, name, tCall, tRaise, ref neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }
        }

        public void Straight(ICharacter player, Label sStatus, int name, ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            int sCall = this.randomGenerator.Next(3, 6);
            int sRaise = this.randomGenerator.Next(3, 8);
            if (player.Power <= 480 && player.Power >= 410)
            {
                this.Smooth(player, sStatus, name, sCall, sRaise, ref neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }

            if (player.Power <= 409 && player.Power >= 407)
            {
                this.Smooth(player, sStatus, name, sCall, sRaise, ref neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }

            if (player.Power < 407 && player.Power >= 404)
            {
                this.Smooth(player, sStatus, name, sCall, sRaise, ref neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }
        }

        public void Flush(ICharacter player, Label sStatus, int name, ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            int fCall = this.randomGenerator.Next(2, 6);
            int fRaise = this.randomGenerator.Next(3, 7);
            this.Smooth(player, sStatus, name, fCall, fRaise, ref neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
        }

        public void FullHouse(ICharacter player, Label sStatus, int name, ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            int fhCall = this.randomGenerator.Next(1, 5);
            int fhRaise = this.randomGenerator.Next(2, 6);
            if (player.Power <= 626 && player.Power >= 620)
            {
                this.Smooth(player, sStatus, name, fhCall, fhRaise, ref neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }

            if (player.Power < 620 && player.Power >= 602)
            {
                this.Smooth(player, sStatus, name, fhCall, fhRaise, ref neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }
        }

        public void FourOfAKind(ICharacter player, Label sStatus, int name, ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            int fkCall = this.randomGenerator.Next(1, 4);
            int fkRaise = this.randomGenerator.Next(2, 5);
            if (player.Power <= 752 && player.Power >= 704)
            {
                this.Smooth(player, sStatus, name, fkCall, fkRaise, ref neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }
        }

        public void StraightFlush(ICharacter player, Label sStatus, int name, ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            int sfCall = this.randomGenerator.Next(1, 3);
            int sfRaise = this.randomGenerator.Next(1, 3);
            if (player.Power <= 913 && player.Power >= 804)
            {
                this.Smooth(player, sStatus, name, sfCall, sfRaise, ref neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }
        }

        private void Smooth(ICharacter player, Label botStatus, int name, int n, int r, ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            int rnd = this.randomGenerator.Next(1, 3);
            if (neededChipsToCall <= 0)
            {
                this.playerMove.Check(player, botStatus, ref raising);
            }
            else
            {
                if (neededChipsToCall >= PlayerMove.RoundN(player.Chips, n))
                {
                    if (player.Chips > neededChipsToCall)
                    {
                        this.playerMove.Call(player, botStatus, ref raising, ref neededChipsToCall, potStatus);
                    }
                    else if (player.Chips <= neededChipsToCall)
                    {
                        raising = false;
                        player.CanMakeTurn = false;
                        player.Chips = 0;
                        botStatus.Text = "Call " + player.Chips;
                        potStatus.Text = (int.Parse(potStatus.Text) + player.Chips).ToString();
                    }
                }
                else
                {
                    if (raise > 0)
                    {
                        if (player.Chips >= raise * 2)
                        {
                            raise *= 2;
                            this.playerMove.Raised(player, botStatus, ref raising, ref raise, ref neededChipsToCall, potStatus);
                        }
                        else
                        {
                            this.playerMove.Call(player, botStatus, ref raising, ref neededChipsToCall, potStatus);
                        }
                    }
                    else
                    {
                        raise = neededChipsToCall * 2;
                        this.playerMove.Raised(player, botStatus, ref raising, ref raise, ref neededChipsToCall, potStatus);
                    }
                }
            }

            if (player.Chips <= 0)
            {
                player.OutOfChips = true;
            }
        }
    }
}

﻿namespace Poker.Models
{
    using System;
    using System.Windows.Forms;
    using Interfaces;

    public class PlayerMove : IPlayerMove
    {
        public static double RoundN(int sChips, int n)
        {
            double a = Math.Round((sChips / n) / 100d, 0) * 100;
            return a;
        }

        public void Fold(ICharacter player, Label sStatus, ref bool rising)
        {
            rising = false;
            sStatus.Text = "Fold";
            player.CanMakeTurn = false;
            player.OutOfChips = true;
        }

        public void Check(ICharacter player, Label cStatus, ref bool raising)
        {
            cStatus.Text = "Check";
            player.CanMakeTurn = false;
            raising = false;
        }

        public void Call(ICharacter player, Label sStatus, ref bool raising, ref int neededChipsToCall, TextBox potStatus)
        {
            raising = false;
            player.CanMakeTurn = false;
            player.Chips -= neededChipsToCall;
            sStatus.Text = "Call " + neededChipsToCall;
            potStatus.Text = (int.Parse(potStatus.Text) + neededChipsToCall).ToString();
        }

        public void Raised(ICharacter player, Label sStatus, ref bool raising, ref int raise, ref int neededChipsToCall, TextBox potStatus)
        {
            player.Chips -= Convert.ToInt32(raise);
            sStatus.Text = "Raise " + raise;
            potStatus.Text = (int.Parse(potStatus.Text) + Convert.ToInt32(raise)).ToString();
            neededChipsToCall = Convert.ToInt32(raise);
            raising = true;
            player.CanMakeTurn = false;
        }

        public void HP(ICharacter player, Label sStatus, int n, int n1, ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 4);
            if (neededChipsToCall <= 0)
            {
                this.Check(player, sStatus, ref raising);
            }

            if (neededChipsToCall > 0)
            {
                if (rnd == 1)
                {
                    if (neededChipsToCall <= RoundN(player.Chips, n))
                    {
                        this.Call(player, sStatus, ref raising, ref neededChipsToCall, potStatus);
                    }
                    else
                    {
                        this.Fold(player, sStatus, ref raising);
                    }
                }

                if (rnd == 2)
                {
                    if (neededChipsToCall <= RoundN(player.Chips, n1))
                    {
                        this.Call(player, sStatus, ref raising, ref neededChipsToCall, potStatus);
                    }
                    else
                    {
                        this.Fold(player, sStatus, ref raising);
                    }
                }
            }

            if (rnd == 3)
            {
                if (raise == 0)
                {
                    raise = neededChipsToCall * 2;
                    this.Raised(player, sStatus, ref raising, ref raise, ref neededChipsToCall, potStatus);
                }
                else
                {
                    if (raise <= RoundN(player.Chips, n))
                    {
                        raise = neededChipsToCall * 2;
                        this.Raised(player, sStatus, ref raising, ref raise, ref neededChipsToCall, potStatus);
                    }
                    else
                    {
                        this.Fold(player, sStatus, ref raising);
                    }
                }
            }

            if (player.Chips <= 0)
            {
                player.OutOfChips = true;
            }
        }

        public void PH(ICharacter player, Label sStatus, int n, int n1, int r, ref int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 3);
            if (rounds < 2)
            {
                if (neededChipsToCall <= 0)
                {
                    this.Check(player, sStatus, ref raising);
                }

                if (neededChipsToCall > 0)
                {
                    if (neededChipsToCall >= RoundN(player.Chips, n1))
                    {
                        this.Fold(player, sStatus, ref raising);
                    }

                    if (raise > RoundN(player.Chips, n))
                    {
                        this.Fold(player, sStatus, ref raising);
                    }

                    if (!player.OutOfChips)
                    {
                        if (neededChipsToCall >= RoundN(player.Chips, n) && neededChipsToCall <= RoundN(player.Chips, n1))
                        {
                            this.Call(player, sStatus, ref raising, ref neededChipsToCall, potStatus);
                        }

                        if (raise <= RoundN(player.Chips, n) && raise >= RoundN(player.Chips, n) / 2)
                        {
                            this.Call(player, sStatus, ref raising, ref neededChipsToCall, potStatus);
                        }

                        if (raise <= RoundN(player.Chips, n) / 2)
                        {
                            if (raise > 0)
                            {
                                raise = (int)RoundN(player.Chips, n);
                                this.Raised(player, sStatus, ref raising, ref raise, ref neededChipsToCall, potStatus);
                            }
                            else
                            {
                                raise = neededChipsToCall * 2;
                                this.Raised(player, sStatus, ref raising, ref raise, ref neededChipsToCall, potStatus);
                            }
                        }
                    }
                }
            }

            if (rounds >= 2)
            {
                if (neededChipsToCall > 0)
                {
                    if (neededChipsToCall >= RoundN(player.Chips, n1 - rnd))
                    {
                        this.Fold(player, sStatus, ref raising);
                    }

                    if (raise > RoundN(player.Chips, n - rnd))
                    {
                        this.Fold(player, sStatus, ref raising);
                    }

                    if (!player.OutOfChips)
                    {
                        if (neededChipsToCall >= RoundN(player.Chips, n - rnd) && neededChipsToCall <= RoundN(player.Chips, n1 - rnd))
                        {
                            this.Call(player, sStatus, ref raising, ref neededChipsToCall, potStatus);
                        }

                        if (raise <= RoundN(player.Chips, n - rnd) && raise >= RoundN(player.Chips, n - rnd) / 2)
                        {
                            this.Call(player, sStatus, ref raising, ref neededChipsToCall, potStatus);
                        }

                        if (raise <= RoundN(player.Chips, n - rnd) / 2)
                        {
                            if (raise > 0)
                            {
                                raise = (int)RoundN(player.Chips, n - rnd);
                                this.Raised(player, sStatus, ref raising, ref raise, ref neededChipsToCall, potStatus);
                            }
                            else
                            {
                                raise = neededChipsToCall * 2;
                                this.Raised(player, sStatus, ref raising, ref raise, ref neededChipsToCall, potStatus);
                            }
                        }
                    }
                }

                if (neededChipsToCall <= 0)
                {
                    raise = (int)RoundN(player.Chips, r - rnd);
                    this.Raised(player, sStatus, ref raising, ref raise, ref neededChipsToCall, potStatus);
                }
            }

            if (player.Chips <= 0)
            {
                player.OutOfChips = true;
            }
        }
    }
}

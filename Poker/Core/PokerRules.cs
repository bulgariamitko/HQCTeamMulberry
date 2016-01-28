namespace Poker.Core
{
    using System;
    using System.Linq;

    using Poker.Interfaces;
    using Poker.UserInterface;

    public class PokerRules
    {
        public const int DefaultCartsOnBoard = 5;

        private MainWindow mainWindow;

        public PokerRules(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        public void Rules(int card1, int card2, ICharacter currentPlayer)
        {
            if (!currentPlayer.OutOfChips || card1 == 0 && card2 == 1 && this.mainWindow.playerStatus.Text.Contains("Fold") == false)
            {
                #region Variables
                bool done = false;
                bool vf = false;
                int[] cardsOnBoard = new int[DefaultCartsOnBoard];
                int[] straight = new int[7];
                straight[0] = this.mainWindow.reservedGameCardsIndexes[card1];
                straight[1] = this.mainWindow.reservedGameCardsIndexes[card2];
                cardsOnBoard[0] = straight[2] = this.mainWindow.reservedGameCardsIndexes[12];
                cardsOnBoard[1] = straight[3] = this.mainWindow.reservedGameCardsIndexes[13];
                cardsOnBoard[2] = straight[4] = this.mainWindow.reservedGameCardsIndexes[14];
                cardsOnBoard[3] = straight[5] = this.mainWindow.reservedGameCardsIndexes[15];
                cardsOnBoard[4] = straight[6] = this.mainWindow.reservedGameCardsIndexes[16];
                var a = straight.Where(o => o % 4 == 0).ToArray();
                var b = straight.Where(o => o % 4 == 1).ToArray();
                var c = straight.Where(o => o % 4 == 2).ToArray();
                var d = straight.Where(o => o % 4 == 3).ToArray();
                var st1 = a.Select(o => o / 4).Distinct().ToArray();
                var st2 = b.Select(o => o / 4).Distinct().ToArray();
                var st3 = c.Select(o => o / 4).Distinct().ToArray();
                var st4 = d.Select(o => o / 4).Distinct().ToArray();
                Array.Sort(straight);
                Array.Sort(st1);
                Array.Sort(st2);
                Array.Sort(st3);
                Array.Sort(st4);
                #endregion

                for (this.mainWindow.i = 0; this.mainWindow.i < 16; this.mainWindow.i++)
                {
                    if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] == int.Parse(this.mainWindow.cardsPictureBoxArray[card1].Tag.ToString()) && this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] == int.Parse(this.mainWindow.cardsPictureBoxArray[card2].Tag.ToString()))
                    {
                        this.RPairFromHand(currentPlayer);

                        #region Pair or Two Pair from Table current = 2 || 0
                        this.RPairTwoPair(currentPlayer);
                        #endregion

                        #region Two Pair current = 2
                        this.RTwoPair(currentPlayer);
                        #endregion

                        #region Three of a kind current = 3
                        this.RThreeOfAKind(currentPlayer, straight);
                        #endregion

                        #region Straight current = 4
                        this.RStraight(currentPlayer, straight);
                        #endregion

                        #region Flush current = 5 || 5.5
                        this.RFlush(currentPlayer, ref vf, cardsOnBoard);
                        #endregion

                        #region Full House current = 6
                        this.RFullHouse(currentPlayer, ref done, straight);
                        #endregion

                        #region Four of a Kind current = 7
                        this.RFourOfAKind(currentPlayer, straight);
                        #endregion

                        #region Straight Flush current = 8 || 9
                        this.RStraightFlush(currentPlayer, st1, st2, st3, st4);
                        #endregion

                        #region High Card current = -1
                        this.RHighCard(currentPlayer);
                        #endregion
                    }
                }
            }
        }

        public void RStraightFlush(ICharacter currentPlayer, int[] st1, int[] st2, int[] st3, int[] st4)
        {
            if (currentPlayer.Type >= -1)
            {
                if (st1.Length >= 5)
                {
                    if (st1[0] + 4 == st1[4])
                    {
                        currentPlayer.Type = 8;
                        currentPlayer.Power = (st1.Max()) / 4 + currentPlayer.Type * 100;
                        this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 8 });
                        this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (st1[0] == 0 && st1[1] == 9 && st1[2] == 10 && st1[3] == 11 && st1[0] + 12 == st1[4])
                    {
                        currentPlayer.Type = 9;
                        currentPlayer.Power = (st1.Max()) / 4 + currentPlayer.Type * 100;
                        this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 9 });
                        this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (st2.Length >= 5)
                {
                    if (st2[0] + 4 == st2[4])
                    {
                        currentPlayer.Type = 8;
                        currentPlayer.Power = (st2.Max()) / 4 + currentPlayer.Type * 100;
                        this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 8 });
                        this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (st2[0] == 0 && st2[1] == 9 && st2[2] == 10 && st2[3] == 11 && st2[0] + 12 == st2[4])
                    {
                        currentPlayer.Type = 9;
                        currentPlayer.Power = (st2.Max()) / 4 + currentPlayer.Type * 100;
                        this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 9 });
                        this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (st3.Length >= 5)
                {
                    if (st3[0] + 4 == st3[4])
                    {
                        currentPlayer.Type = 8;
                        currentPlayer.Power = (st3.Max()) / 4 + currentPlayer.Type * 100;
                        this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 8 });
                        this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (st3[0] == 0 && st3[1] == 9 && st3[2] == 10 && st3[3] == 11 && st3[0] + 12 == st3[4])
                    {
                        currentPlayer.Type = 9;
                        currentPlayer.Power = (st3.Max() / 4) + currentPlayer.Type * 100;
                        this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 9 });
                        this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (st4.Length >= 5)
                {
                    if (st4[0] + 4 == st4[4])
                    {
                        currentPlayer.Type = 8;
                        currentPlayer.Power = (st4.Max()) / 4 + currentPlayer.Type * 100;
                        this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 8 });
                        this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (st4[0] == 0 && st4[1] == 9 && st4[2] == 10 && st4[3] == 11 && st4[0] + 12 == st4[4])
                    {
                        currentPlayer.Type = 9;
                        currentPlayer.Power = (st4.Max()) / 4 + currentPlayer.Type * 100;
                        this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 9 });
                        this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        public void RFourOfAKind(ICharacter currentPlayer, int[] straight)
        {
            if (currentPlayer.Type >= -1)
            {
                for (int j = 0; j <= 3; j++)
                {
                    if (straight[j] / 4 == straight[j + 1] / 4 && straight[j] / 4 == straight[j + 2] / 4 &&
                        straight[j] / 4 == straight[j + 3] / 4)
                    {
                        currentPlayer.Type = 7;
                        currentPlayer.Power = (straight[j] / 4) * 4 + currentPlayer.Type * 100;
                        this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 7 });
                        this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (straight[j] / 4 == 0 && straight[j + 1] / 4 == 0 && straight[j + 2] / 4 == 0 && straight[j + 3] / 4 == 0)
                    {
                        currentPlayer.Type = 7;
                        currentPlayer.Power = 13 * 4 + currentPlayer.Type * 100;
                        this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 7 });
                        this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        public void RFullHouse(ICharacter currentPlayer, ref bool done, int[] straight)
        {
            if (currentPlayer.Type >= -1)
            {
                this.mainWindow.type = currentPlayer.Power;
                for (int j = 0; j <= 12; j++)
                {
                    var fh = straight.Where(o => o / 4 == j).ToArray();
                    if (fh.Length == 3 || done)
                    {
                        if (fh.Length == 2)
                        {
                            if (fh.Max() / 4 == 0)
                            {
                                currentPlayer.Type = 6;
                                currentPlayer.Power = 13 * 2 + currentPlayer.Type * 100;
                                this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 6 });
                                this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                                break;
                            }

                            if (fh.Max() / 4 > 0)
                            {
                                currentPlayer.Type = 6;
                                currentPlayer.Power = fh.Max() / 4 * 2 + currentPlayer.Type * 100;
                                this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 6 });
                                this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                                break;
                            }
                        }

                        if (!done)
                        {
                            if (fh.Max() / 4 == 0)
                            {
                                currentPlayer.Power = 13;
                                done = true;
                                j = -1;
                            }
                            else
                            {
                                currentPlayer.Power = fh.Max() / 4;
                                done = true;
                                j = -1;
                            }
                        }
                    }
                }

                if (currentPlayer.Type != 6)
                {
                    currentPlayer.Power = this.mainWindow.type;
                }
            }
        }

        public void RFlush(ICharacter currentPlayer, ref bool vf, int[] cardsOnBoard)
        {
            if (currentPlayer.Type >= -1)
            {
                var f1 = cardsOnBoard.Where(o => o % 4 == 0).ToArray();
                var f2 = cardsOnBoard.Where(o => o % 4 == 1).ToArray();
                var f3 = cardsOnBoard.Where(o => o % 4 == 2).ToArray();
                var f4 = cardsOnBoard.Where(o => o % 4 == 3).ToArray();
                if (f1.Length == 3 || f1.Length == 4)
                {
                    if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] % 4 == this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] % 4 && 
                        this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] % 4 == f1[0] % 4)
                    {
                        if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 > f1.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] + currentPlayer.Type * 100;
                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 > f1.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] + currentPlayer.Type * 100;
                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 < f1.Max() / 4 && this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 < f1.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f1.Max() + currentPlayer.Type * 100;
                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f1.Length == 4)//different cards in hand
                {
                    if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] % 4 != this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] % 4 && 
                        this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] % 4 == f1[0] % 4)
                    {
                        if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 > f1.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] + currentPlayer.Type * 100;
                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f1.Max() + currentPlayer.Type * 100;
                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }

                    if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] % 4 != this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] % 4 &&
                        this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] % 4 == f1[0] % 4)
                    {
                        if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 > f1.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] + currentPlayer.Type * 100;
                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f1.Max() + currentPlayer.Type * 100;
                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f1.Length == 5)
                {
                    if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] % 4 == f1[0] % 4 && this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 > f1.Min() / 4)
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] + currentPlayer.Type * 100;
                        this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                        this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] % 4 == f1[0] % 4 && this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 > f1.Min() / 4)
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] + currentPlayer.Type * 100;
                        this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                        this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 < f1.Min() / 4 && this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 < f1.Min())
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = f1.Max() + currentPlayer.Type * 100;
                        this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                        this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f2.Length == 3 || f2.Length == 4)
                {
                    if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] % 4 == this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] % 4 && 
                        this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] % 4 == f2[0] % 4)
                    {
                        if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 > f2.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] + currentPlayer.Type * 100;
                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 > f2.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] + currentPlayer.Type * 100;
                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 < f2.Max() / 4 && this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 < f2.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f2.Max() + currentPlayer.Type * 100;
                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f2.Length == 4)//different cards in hand
                {
                    if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] % 4 != this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] % 4 &&
                        this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] % 4 == f2[0] % 4)
                    {
                        if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 > f2.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] + currentPlayer.Type * 100;
                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f2.Max() + currentPlayer.Type * 100;
                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }

                    if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] % 4 != this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] % 4 && 
                        this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] % 4 == f2[0] % 4)
                    {
                        if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 > f2.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] + currentPlayer.Type * 100;
                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f2.Max() + currentPlayer.Type * 100;
                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f2.Length == 5)
                {
                    if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] % 4 == f2[0] % 4 && this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 > f2.Min() / 4)
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] + currentPlayer.Type * 100;
                        this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                        this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] % 4 == f2[0] % 4 && this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 > f2.Min() / 4)
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] + currentPlayer.Type * 100;
                        this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                        this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 < f2.Min() / 4 && this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 < f2.Min())
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = f2.Max() + currentPlayer.Type * 100;
                        this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                        this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f3.Length == 3 || f3.Length == 4)
                {
                    if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] % 4 == this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] % 4 &&
                        this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] % 4 == f3[0] % 4)
                    {
                        if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 > f3.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] + currentPlayer.Type * 100;
                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 > f3.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] + currentPlayer.Type * 100;
                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 < f3.Max() / 4 && this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 < f3.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f3.Max() + currentPlayer.Type * 100;
                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f3.Length == 4)//different cards in hand
                {
                    if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] % 4 != this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] % 4 &&
                        this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] % 4 == f3[0] % 4)
                    {
                        if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 > f3.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] + currentPlayer.Type * 100;
                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f3.Max() + currentPlayer.Type * 100;
                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }

                    if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] % 4 != this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] % 4 && 
                        this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] % 4 == f3[0] % 4)
                    {
                        if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 > f3.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] + currentPlayer.Type * 100;
                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f3.Max() + currentPlayer.Type * 100;
                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f3.Length == 5)
                {
                    if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] % 4 == f3[0] % 4 && this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 > f3.Min() / 4)
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] + currentPlayer.Type * 100;
                        this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                        this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] % 4 == f3[0] % 4 && this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 > f3.Min() / 4)
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] + currentPlayer.Type * 100;
                        this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                        this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 < f3.Min() / 4 && this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 < f3.Min())
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = f3.Max() + currentPlayer.Type * 100;
                        this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                        this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f4.Length == 3 || f4.Length == 4)
                {
                    if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] % 4 == this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] % 4 &&
                        this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] % 4 == f4[0] % 4)
                    {
                        if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 > f4.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] + currentPlayer.Type * 100;
                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 > f4.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] + currentPlayer.Type * 100;
                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 < f4.Max() / 4 && this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 < f4.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f4.Max() + currentPlayer.Type * 100;
                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f4.Length == 4)//different cards in hand
                {
                    if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] % 4 != this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] % 4 &&
                        this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] % 4 == f4[0] % 4)
                    {
                        if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 > f4.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] + currentPlayer.Type * 100;
                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f4.Max() + currentPlayer.Type * 100;
                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }

                    if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] % 4 != this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] % 4 && 
                        this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] % 4 == f4[0] % 4)
                    {
                        if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 > f4.Max() / 4)
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] + currentPlayer.Type * 100;
                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentPlayer.Type = 5;
                            currentPlayer.Power = f4.Max() + currentPlayer.Type * 100;
                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f4.Length == 5)
                {
                    if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] % 4 == f4[0] % 4 && this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 > f4.Min() / 4)
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] + currentPlayer.Type * 100;
                        this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                        this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] % 4 == f4[0] % 4 && this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 > f4.Min() / 4)
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] + currentPlayer.Type * 100;
                        this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                        this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 < f4.Min() / 4 && this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 < f4.Min())
                    {
                        currentPlayer.Type = 5;
                        currentPlayer.Power = f4.Max() + currentPlayer.Type * 100;
                        this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5 });
                        this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }
                
                if (f1.Length > 0)//ace
                {
                    if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 == 0 && this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        currentPlayer.Type = 5.5;
                        currentPlayer.Power = 13 + currentPlayer.Type * 100;
                        this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5.5 });
                        this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 == 0 && this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        currentPlayer.Type = 5.5;
                        currentPlayer.Power = 13 + currentPlayer.Type * 100;
                        this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5.5 });
                        this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (f2.Length > 0)
                {
                    if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 == 0 && this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        currentPlayer.Type = 5.5;
                        currentPlayer.Power = 13 + currentPlayer.Type * 100;
                        this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5.5 });
                        this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 == 0 && this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        currentPlayer.Type = 5.5;
                        currentPlayer.Power = 13 + currentPlayer.Type * 100;
                        this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5.5 });
                        this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (f3.Length > 0)
                {
                    if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 == 0 && this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        currentPlayer.Type = 5.5;
                        currentPlayer.Power = 13 + currentPlayer.Type * 100;
                        this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5.5 });
                        this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 == 0 && this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        currentPlayer.Type = 5.5;
                        currentPlayer.Power = 13 + currentPlayer.Type * 100;
                        this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5.5 });
                        this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (f4.Length > 0)
                {
                    if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 == 0 && this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] % 4 == f4[0] % 4 && vf && f4.Length > 0)
                    {
                        currentPlayer.Type = 5.5;
                        currentPlayer.Power = 13 + currentPlayer.Type * 100;
                        this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5.5 });
                        this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 == 0 && this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] % 4 == f4[0] % 4 && vf)
                    {
                        currentPlayer.Type = 5.5;
                        currentPlayer.Power = 13 + currentPlayer.Type * 100;
                        this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 5.5 });
                        this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        public void RStraight(ICharacter currentPlayer, int[] straight)
        {
            if (currentPlayer.Type >= -1)
            {
                var op = straight.Select(o => o / 4).Distinct().ToArray();
                for (int j = 0; j < op.Length - 4; j++)
                {
                    if (op[j] + 4 == op[j + 4])
                    {
                        if (op.Max() - 4 == op[j])
                        {
                            currentPlayer.Type = 4;
                            currentPlayer.Power = op.Max() + currentPlayer.Type * 100;
                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 4 });
                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        }
                        else
                        {
                            currentPlayer.Type = 4;
                            currentPlayer.Power = op[j + 4] + currentPlayer.Type * 100;
                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 4 });
                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        }
                    }

                    if (op[j] == 0 && op[j + 1] == 9 && op[j + 2] == 10 && op[j + 3] == 11 && op[j + 4] == 12)
                    {
                        currentPlayer.Type = 4;
                        currentPlayer.Power = 13 + currentPlayer.Type * 100;
                        this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 4 });
                        this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        public void RThreeOfAKind(ICharacter currentPlayer, int[] straight)
        {
            if (currentPlayer.Type >= -1)
            {
                for (int j = 0; j <= 12; j++)
                {
                    var fh = straight.Where(o => o / 4 == j).ToArray();
                    if (fh.Length == 3)
                    {
                        if (fh.Max() / 4 == 0)
                        {
                            currentPlayer.Type = 3;
                            currentPlayer.Power = 13 * 3 + currentPlayer.Type * 100;
                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 3 });
                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                        else
                        {
                            currentPlayer.Type = 3;
                            currentPlayer.Power = fh[0] / 4 + fh[1] / 4 + fh[2] / 4 + currentPlayer.Type * 100;
                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 3 });
                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                    }
                }
            }
        }

        public void RTwoPair(ICharacter currentPlayer) //ref double current, ref double power
        {
            if (currentPlayer.Type >= -1)
            {
                bool msgbox = false;
                for (int tc = 16; tc >= 12; tc--)
                {
                    int max = tc - 12;
                    if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 != this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4)
                    {
                        for (int k = 1; k <= max; k++)
                        {
                            if (tc - k < 12)
                            {
                                max--;
                            }

                            if (tc - k >= 12)
                            {
                                if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 == this.mainWindow.reservedGameCardsIndexes[tc] / 4 && 
                                    this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 == this.mainWindow.reservedGameCardsIndexes[tc - k] / 4 || 
                                    this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 == this.mainWindow.reservedGameCardsIndexes[tc] / 4 && 
                                    this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 == this.mainWindow.reservedGameCardsIndexes[tc - k] / 4)
                                {
                                    if (!msgbox)
                                    {
                                        if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 == 0)
                                        {
                                            currentPlayer.Type = 2;
                                            currentPlayer.Power = 13 * 4 + (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4) * 2 + currentPlayer.Type * 100;
                                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 2 });
                                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op => op.Current)
                                                .ThenByDescending(op => op.Power).First();
                                        }

                                        if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 == 0)
                                        {
                                            currentPlayer.Type = 2;
                                            currentPlayer.Power = 13 * 4 + (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4) * 2 + currentPlayer.Type * 100;
                                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 2 });
                                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op => op.Current)
                                                .ThenByDescending(op => op.Power).First();
                                        }

                                        if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 != 0 && this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 != 0)
                                        {
                                            currentPlayer.Type = 2;
                                            currentPlayer.Power = (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4) * 2 + 
                                                                  (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4) * 2 + currentPlayer.Type * 100;
                                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 2 });
                                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op => op.Current)
                                                .ThenByDescending(op => op.Power).First();
                                        }
                                    }

                                    msgbox = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void RPairTwoPair(ICharacter currentPlayer) //ref double current, ref double power
        {
            if (currentPlayer.Type >= -1)
            {
                bool msgbox = false;
                bool msgbox1 = false;
                for (int tc = 16; tc >= 12; tc--)
                {
                    int max = tc - 12;
                    for (int k = 1; k <= max; k++)
                    {
                        if (tc - k < 12)
                        {
                            max--;
                        }

                        if (tc - k >= 12)
                        {
                            if (this.mainWindow.reservedGameCardsIndexes[tc] / 4 == this.mainWindow.reservedGameCardsIndexes[tc - k] / 4)
                            {
                                if (this.mainWindow.reservedGameCardsIndexes[tc] / 4 != this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 &&
                                    this.mainWindow.reservedGameCardsIndexes[tc] / 4 != this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 &&
                                    currentPlayer.Type == 1)
                                {
                                    if (!msgbox)
                                    {
                                        if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 == 0)
                                        {
                                            currentPlayer.Type = 2;
                                            currentPlayer.Power = (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4) * 2 + 13 * 4 + currentPlayer.Type * 100;
                                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 2 });
                                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op => op.Current)
                                                .ThenByDescending(op => op.Power).First();
                                        }

                                        if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 == 0)
                                        {
                                            currentPlayer.Type = 2;
                                            currentPlayer.Power = (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4) * 2 + 13 * 4 + currentPlayer.Type * 100;
                                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 2 });
                                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op => op.Current)
                                                .ThenByDescending(op => op.Power).First();
                                        }

                                        if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 != 0)
                                        {
                                            currentPlayer.Type = 2;
                                            currentPlayer.Power = (this.mainWindow.reservedGameCardsIndexes[tc] / 4) * 2 +
                                                                  (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4) * 2 + currentPlayer.Type * 100;
                                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 2 });
                                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op => op.Current)
                                                .ThenByDescending(op => op.Power).First();
                                        }

                                        if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 != 0)
                                        {
                                            currentPlayer.Type = 2;
                                            currentPlayer.Power = (this.mainWindow.reservedGameCardsIndexes[tc] / 4) * 2 + 
                                                                  (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4) * 2 + currentPlayer.Type * 100;
                                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 2 });
                                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op => op.Current)
                                                .ThenByDescending(op => op.Power).First();
                                        }
                                    }

                                    msgbox = true;
                                }

                                if (currentPlayer.Type == -1)
                                {
                                    if (!msgbox1)
                                    {
                                        if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 > this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4)
                                        {
                                            if (this.mainWindow.reservedGameCardsIndexes[tc] / 4 == 0)
                                            {
                                                currentPlayer.Type = 0;
                                                currentPlayer.Power = 13 + this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 + currentPlayer.Type * 100;
                                                this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 1 });
                                                this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op => op.Current)
                                                    .ThenByDescending(op => op.Power).First();
                                            }
                                            else
                                            {
                                                currentPlayer.Type = 0;
                                                currentPlayer.Power = this.mainWindow.reservedGameCardsIndexes[tc] / 4 + 
                                                                      this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 + currentPlayer.Type * 100;
                                                this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 1 });
                                                this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op => op.Current)
                                                    .ThenByDescending(op => op.Power).First();
                                            }
                                        }
                                        else
                                        {
                                            if (this.mainWindow.reservedGameCardsIndexes[tc] / 4 == 0)
                                            {
                                                currentPlayer.Type = 0;
                                                currentPlayer.Power = 13 + this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] + currentPlayer.Type * 100;
                                                this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 1 });
                                                this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op => op.Current)
                                                    .ThenByDescending(op => op.Power).First();
                                            }
                                            else
                                            {
                                                currentPlayer.Type = 0;
                                                currentPlayer.Power = this.mainWindow.reservedGameCardsIndexes[tc] / 4 +
                                                                      this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 + currentPlayer.Type * 100;
                                                this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 1 });
                                                this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op => op.Current)
                                                    .ThenByDescending(op => op.Power).First();
                                            }
                                        }
                                    }

                                    msgbox1 = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void RPairFromHand(ICharacter currentPlayer) //ref double current, ref double power
        {
            if (currentPlayer.Type >= -1)
            {
                bool msgbox = false;
                if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 == this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4)
                {
                    if (!msgbox)
                    {
                        if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 == 0)
                        {
                            currentPlayer.Type = 1;
                            currentPlayer.Power = 13 * 4 + currentPlayer.Type * 100;
                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 1 });
                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                        else
                        {
                            currentPlayer.Type = 1;
                            currentPlayer.Power = (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4) * 4 + currentPlayer.Type * 100;
                            this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 1 });
                            this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                    }

                    msgbox = true;
                }

                for (int tc = 16; tc >= 12; tc--)
                {
                    if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 == this.mainWindow.reservedGameCardsIndexes[tc] / 4)
                    {
                        if (!msgbox)
                        {
                            if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 == 0)
                            {
                                currentPlayer.Type = 1;
                                currentPlayer.Power = 13 * 4 + this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 + currentPlayer.Type * 100;
                                this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 1 });
                                this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                            else
                            {
                                currentPlayer.Type = 1;
                                currentPlayer.Power = (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4) * 4 + 
                                                      this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 + currentPlayer.Type * 100;
                                this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 1 });
                                this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                        }

                        msgbox = true;
                    }

                    if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 == this.mainWindow.reservedGameCardsIndexes[tc] / 4)
                    {
                        if (!msgbox)
                        {
                            if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 == 0)
                            {
                                currentPlayer.Type = 1;
                                currentPlayer.Power = 13 * 4 + this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 + currentPlayer.Type * 100;
                                this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 1 });
                                this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                            else
                            {
                                currentPlayer.Type = 1;
                                currentPlayer.Power = (this.mainWindow.reservedGameCardsIndexes[tc] / 4) * 4 +
                                                      this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 + currentPlayer.Type * 100;
                                this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = 1 });
                                this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                        }

                        msgbox = true;
                    }
                }
            }
        }

        public void RHighCard(ICharacter currentPlayer)
        {
            if (currentPlayer.Type == -1)
            {
                if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 > this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4)
                {
                    currentPlayer.Type = -1;
                    currentPlayer.Power = this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4;
                    this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = -1 });
                    this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
                else
                {
                    currentPlayer.Type = -1;
                    currentPlayer.Power = this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4;
                    this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = -1 });
                    this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }

                if (this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i] / 4 == 0 || this.mainWindow.reservedGameCardsIndexes[this.mainWindow.i + 1] / 4 == 0)
                {
                    currentPlayer.Type = -1;
                    currentPlayer.Power = 13;
                    this.mainWindow.gameDatabase.Win.Add(new Poker.Type() { Power = currentPlayer.Power, Current = -1 });
                    this.mainWindow.sorted = this.mainWindow.gameDatabase.Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
            }
        }
    }
}
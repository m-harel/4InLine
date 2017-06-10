using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace _Inline
{
    class Player
    {
        public static int[,] scoresTable = { { -1, 100, 100000, 10000000 }, { -1, 100, 10000, 10000000 } };
        private Color playerColor;
        public bool ComputerPlayer = false;
        public string name;
        private int Num;
        private int depth = 6;

        public int num
        {
            get
            {
                return this.Num;
            }
        }
        public Color PlayerColor
        {
            get
            {
                return playerColor;
            }
        }
        private int[,] Board;
        int BestMove = 0;
        public Player(Color c, int Number)
        {
            this.Num = Number;
            this.playerColor = c;
            this.name = "the " +c.ToString() + " player";
        }
        public override string ToString()
        {
            return this.name;
        }
        public void UpDateBoard(int[,] b)
        {
            if (ComputerPlayer)
                this.Board = b;
            else
                throw new Exception("this is not a computer player");
        }
        public int TheBestMove()
        {
            double i;
            checkMove(1, true);
            return BestMove;
        }
        private double checkMove(int DC, bool that)
        {
            int i;
            if (DC < depth)
            {
                double b = LowCheck();
                if (b == 0)
                {
                    double[] all = new double[GamePanel.widthNum];
                    for (i = GamePanel.widthNum -1; i >=0; i--)
                    {
                        if (LegalMove(i))
                        {
                            add(i, that);
                            all[i] = this.checkMove(DC + 1, !that);
                            remove(i);
                            if (all[i] < -Math.Pow(10,15) && !that && DC<3)
                                break;
                            else if (all[i] > Math.Pow(10,14) && that && DC<3)
                                break;
                        }
                        else if (that)
                            all[i] = double.MinValue;
                        else
                            all[i] = double.MaxValue;
                    }
                    
                    if (that)
                    {
                        BestMove = max(all);
                        return all[max(all)] + HighCheck() / 10;
                    }
                    else
                    {
                        BestMove = min(all);
                        return all[min(all)] + HighCheck() / 10;
                    }
                }
                else
                {
                    return b;
                }
            }
            else
            {
                double b = LowCheck();
                if (b == 0)
                    return HighCheck(); 
                else
                    return b;
            }
        }
        private bool LegalMove(int i)
        {
            if (i < 0 || i >= GamePanel.widthNum)
                return false;
            else if (Board[i, 0] != 0)
                return false;
            
             return true;
        }
        private int max(double[] d)
        {
            double b = double.MinValue;
            int t = 0;
            int i;
            for (i = 0; i < d.Length; i++)
            {
                if (d[i] > b)
                {
                    b = d[i];
                    t=i;
                }
            }
            return t;
        }
        private int min(double[] d)
        {
            double b = double.MaxValue;
            int t = 0;
            int i;
            for (i = 0; i < d.Length; i++)
            {
                if (d[i] < b)
                {
                    b = d[i];
                    t = i;
                }
            }
            return t;
        }
        private void add(int l,bool that)
        {
            if (l < 0 || l > GamePanel.widthNum - 1)
                throw new Exception("illegal number");
            else if (Board[l, 0] != 0)
                throw new Exception("illegal column");
            int j;
            for (j = GamePanel.heightNum - 1; j >= 0; j--)
            {
                if (Board[l, j] == 0)
                {
                    if (that)
                        this.Board[l, j] = this.Num;
                    else
                        this.Board[l, j] = this.Num == 1 ? 2 : 1;
                    break;
                }
            }
        }
        private void remove(int l)
        {
            int j;
            for (j = 0; j < GamePanel.heightNum; j++)
            {
                if (Board[l, j] != 0)
                {
                    Board[l, j] = 0;
                    return;
                }
            }
            throw new Exception("try to remove from a empty column");
        }
        private double LowCheck()
        {
            double score = 0;
            int i, j, t, k, x;
            bool series = false;
            for (i = 0; i < GamePanel.widthNum; i++)
            {
                for (j = 0; j < GamePanel.heightNum; j++)
                {
                    if (this.Board[i,j] != 0)
                    {
                        for (t = -1; t < 2; t++)
                        {
                            for (k = -1; k < 2; k++)
                            {
                                series = true;
                                for (x = 1; x < 4; x++)
                                {
                                    if (existing(i + x * t, j + x * k) != this.Board[i, j])
                                    {
                                        series = false;
                                        break;
                                    }
                                }
                                if (series && (t != 0 || k != 0))
                                {
                                    if (this.Board[i, j] == this.Num)
                                        return Math.Pow(10,15);
                                    else
                                        return -Math.Pow(10, 16);

                                }
                            }
                        }
                    }
                }
            }
            return score;
        }
        private int existing(int i, int j)
        {
            if (i >= 0 && i < GamePanel.widthNum && j >= 0 && j < GamePanel.heightNum)
                return this.Board[i, j];
            return -1;
        }
        private double HighCheck()
        {
            double score = 0;
            int i, j, t, k,x;
            int u, q;
            bool series;
            for (i = 0; i < GamePanel.widthNum; i++)
            {
                for (j = 0; j < GamePanel.heightNum; j++)
                {
                    for (t = -1; t < 2; t++)
                    {
                        for (k = -1; k < 2; k++)
                        {
                            u = q =0;
                            for (x = 0; x < 4; x++)
                            {
                                int p =existing(i + x * t, j + x * k);
                                if (p == this.Num)
                                    u++;
                                else if (p > 0)
                                    q++;
                                else if (p == -1)
                                {
                                    u = q = 0;
                                    break;
                                }
                            }
                            if (u != 0 && q == 0)
                                score += Player.scoresTable[this.Num-1, u - 1]; //Math.Pow(10,u);
                            else if (q != 0 && u == 0)
                                score -= Math.Pow(10, q);
                                /*else if(q!=0 && u!=0 && u<q)
                                    score += Math.Pow(10, 1);
                                else if (q != 0 && u != 0 && q < u)
                                    score -= Math.Pow(10, 1);*/

                        }
                    }
                    
                }
            }
            return score;
        }
    }
}

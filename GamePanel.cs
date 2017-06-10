using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace _Inline
{
    class GamePanel : System.Windows.Forms.Panel
    {
        public  const int widthNum = 7;
        public  const int heightNum = 6;
        private square[,] AllSquare;
        private Player[] AllPlayers;
        private Player CurrentPlayer;
        private bool OnGame = false;
        public GamePanel(System.Drawing.Size PanelSize)
        {
            this.ResetBoard(PanelSize);
            this.AllPlayers = new Player[2];
        }
        public void ResetBoard(System.Drawing.Size PanelSize)
        {
            this.Controls.Clear();
            this.AllSquare = new square[GamePanel.widthNum, GamePanel.heightNum];
            int i, j;
            for (i = 0; i < GamePanel.widthNum; i++)
            {
                for (j = 0; j < GamePanel.heightNum; j++)
                {
                    this.AllSquare[i, j] = new square(i, j);
                    this.AllSquare[i, j].Location = new System.Drawing.Point((int)(i * (PanelSize.Width / GamePanel.widthNum)) + 1, (int)(j * (PanelSize.Height / GamePanel.heightNum)) + 1);
                    this.AllSquare[i, j].Size = new System.Drawing.Size((int)(PanelSize.Width / GamePanel.widthNum) - 2, (int)(PanelSize.Height / GamePanel.heightNum) - 2);
                    this.AllSquare[i, j].BackColor = Color.White;
                    this.AllSquare[i, j].Click += new System.EventHandler(this.panel_Click);
                    this.Controls.Add(this.AllSquare[i, j]);

                }
            }
        }
        private void panel_Click(object sender, EventArgs e)
        {
            square s = (square)(sender);
           // MessageBox.Show(s.j.ToString());
            if (OnGame)
            {

                if (LegalSquare(s))
                {
                    s.Relevant = this.CurrentPlayer;
                    this.CurrentPlayer = NextPlayer(CurrentPlayer);
                    
                }
           
                if (CompleteGame())
                {
                    this.OnGame = false;
                    MessageBox.Show("you win!!!");
                }
                else if (this.CurrentPlayer.ComputerPlayer)
                {
                    this.SendBoard(CurrentPlayer);
                    AddToColumn(CurrentPlayer.TheBestMove(), CurrentPlayer);
                    if (CompleteGame())
                    {
                        this.OnGame = false;
                        MessageBox.Show("loser!!!");
                    }
                    this.CurrentPlayer = NextPlayer(CurrentPlayer);
                    

                }
            }
           
        }
        public void StartGame(Player[] p)
        {
            this.OnGame = true;
            this.AllPlayers = p;
            this.CurrentPlayer = p[0];
        abc:
            this.Refresh();
            if (CompleteGame())
            {
                this.OnGame = false;
                MessageBox.Show("you win!!!");
            }
            else if (this.CurrentPlayer.ComputerPlayer)
            {
                 this.SendBoard(CurrentPlayer);
                    AddToColumn(CurrentPlayer.TheBestMove(), CurrentPlayer);
                    if (CompleteGame())
                    {
                        this.OnGame = false;
                        MessageBox.Show("loser!!!");
                    
                    }
                    this.CurrentPlayer = NextPlayer(CurrentPlayer);
                    if (this.CurrentPlayer.ComputerPlayer)
                        goto abc;
                }
        }
        private Player NextPlayer(Player p)
        {
            if (p == AllPlayers[0])
                return AllPlayers[1];
            return AllPlayers[0];
        }
        private bool LegalSquare(square s)
        {
            if (s.Relevant != null) //האם המשבצת ריקה
                return false;
            else if (s.j == GamePanel.heightNum-1) //אם המשבצת ריקה והיא הכי נמוכה תחזיר חיובי
                return true;
            else if (this.AllSquare[s.i, s.j + 1].Relevant == null) //אם זאת לא המשבצת הכי נמוכה ומתחתיה יש משבצת שהיא ריקה תחזיר שלילי
                return false;
            
            return true;
        }
        private bool CompleteGame()
        {
            int i,j,t,k,x;
            bool series = false;
            for (i = 0; i < GamePanel.widthNum; i++)
            {
                for (j = 0; j < GamePanel.heightNum; j++)
                {
                    if (this.AllSquare[i, j].Relevant != null)
                    {
                        for (t = -1; t < 2; t++)
                        {
                            for (k = -1; k < 2; k++)
                            {
                                series = true;
                                for (x = 1; x < 4; x++)
                                {
                                    if (existing(i + x * t, j + x * k) != this.AllSquare[i, j].Relevant.num)
                                    {
                                        series = false;
                                        break;
                                    }
                                }
                                if (series && (t!=0 || k!=0))
                                {
                               //     MessageBox.Show(this.AllSquare[i, j].Relevant.ToString() + " won!!!");
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
        private int existing(int i, int j)
        {
            if (i >= 0 && i < GamePanel.widthNum && j >= 0 && j < GamePanel.heightNum)
                if(this.AllSquare[i,j].Relevant != null)
                    return this.AllSquare[i, j].Relevant.num;
            return 0;
        }
        private void SendBoard(Player p)
        {
            int i,j;
            int[,] b = new int[widthNum, heightNum];
            for (i = 0; i < widthNum; i++)
            {
                for (j = 0; j < heightNum; j++)
                {
                    if (AllSquare[i, j].Relevant != null)
                        b[i, j] = AllSquare[i, j].Relevant.num;
                    else
                        b[i, j] = 0;
                }
            }
            p.UpDateBoard(b);
        }
        private void AddToColumn(int l, Player p)
        {
            if (l < 0 || l > widthNum - 1)
                throw new Exception("illegal number");
            else if (AllSquare[l, 0].Relevant != null)
                throw new Exception("illegal column");
            int j;
            for (j = heightNum - 1; j >= 0; j--)
            {
                if (AllSquare[l, j].Relevant == null)
                {
                    this.AllSquare[l, j].Relevant = p;
                    break;
                }
            }
        }
    }
}

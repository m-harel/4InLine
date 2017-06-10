using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace _Inline
{
    public partial class Form1 : Form
    {
        private GamePanel Board;
        private Player[] Players;
        public Form1()
        {
            InitializeComponent();
            this.Board = new GamePanel(this.panel1.Size);
            this.Board.Size = this.panel1.Size;

            this.panel1.Controls.Add(this.Board);
            this.Players = new Player[2];
            this.Players[0] = new Player(Color.Red,1);
            this.Players[0].name = "player 1";
            this.Players[1] = new Player(Color.Blue,2);
            this.Players[1].name = "player 2";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Players[0].ComputerPlayer = false;
            this.Players[1].ComputerPlayer = false;
            Board.ResetBoard(Board.Size);
            Board.StartGame(this.Players);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Players[0].ComputerPlayer = false;
            this.Players[1].ComputerPlayer = true;
            Board.ResetBoard(Board.Size);
            Board.StartGame(this.Players);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Players[0].ComputerPlayer = true;
            this.Players[1].ComputerPlayer = false;
            Board.ResetBoard(Board.Size);
            Board.StartGame(this.Players);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Players[0].ComputerPlayer = true;
            this.Players[1].ComputerPlayer = true;
            Board.ResetBoard(Board.Size);
            Board.StartGame(this.Players);
        }
    }
}
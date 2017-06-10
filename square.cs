using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace _Inline
{
    class square : System.Windows.Forms.Panel
    {
        private Player relevant;
        public Player Relevant
        {
            get
            {
                return this.relevant;
            }
            set
            {
                if (this.relevant != null)
                    throw new Exception("this square is all ready clicked");
                else
                {
                    this.relevant = value;
                    this.BackColor = this.relevant.PlayerColor;
                }
            }
        }
        public int i, j;
        public square(int i, int j)
        {
            this.BackColor = Color.Red;
            this.i = i;
            this.j = j;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Logic
{
    public class Block
    {
        public int WH { get; set; }

        public Brush Brush { get; set; }

        public Block(int wh,Brush brush)
        {
            WH = wh;
            Brush = brush;
        }

    }
}

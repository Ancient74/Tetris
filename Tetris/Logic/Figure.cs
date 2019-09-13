using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Logic
{
    public class Figure
    {
        public Block[][] Blocks { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Xindex { get; set; }
        public int Yindex { get; set; }
        public int WH { get; }
        public Figure(Block[][]figure, int wh)
        {
            Blocks = figure;
            WH = wh;
        }
        public void Draw(Graphics graphics)
        {
            for (int i = 0; i < Blocks.Length; i++)
            {
                for (int j = 0; j < Blocks[i].Length; j++)
                {
                    if (Blocks[i][j] != null)
                    {
                        int xoff = X + i * WH;
                        int yoff = Y + j * WH;
                        graphics.FillRectangle(Blocks[i][j].Brush, xoff, yoff, WH, WH);
                    }
                }
            }
        }

        public bool Move(Block[][]grid,int index)
        {
            ClearGrid(grid);

            Xindex += index;

            bool res = FillGrid(grid,Blocks);
            if (!res)
                Xindex -= index;
            return res;
        }

        private void ClearGrid(Block[][] grid)
        {
            for (int i = 0; i < Blocks.Length; i++)
            {
                for (int j = 0; j < Blocks[i].Length; j++)
                {
                    if (Blocks[i][j] != null)
                    {
                        if(Xindex + i < grid.Length && Xindex+i>=0 
                           &&  Yindex + j < grid[Xindex + i].Length && Yindex + j >=0)
                        {
                            grid[Xindex + i][Yindex + j] = null;
                        }
                    }
                }
            }
        }
        private bool FillGrid(Block[][] grid, Block[][]blocks)
        {
            for (int i = 0; i < blocks.Length; i++)
            {
                int xoff = Xindex + i;
                for (int j = 0; j < blocks[i].Length; j++)
                {
                    int yoff = Yindex + j;
                    if (blocks[i][j] != null)
                    {
                        if (xoff >= grid.Length || xoff < 0) 
                        {
                            return false;
                        }
                        if(yoff < 0)
                        {
                            continue;
                        }
                        if (yoff >= grid[xoff].Length)
                        {
                            return false;
                        }
                        if (grid[xoff][yoff] != null)
                        {
                            return false;
                        }
                        grid[xoff][yoff] = blocks[i][j];
                    }
                }
            }
            return true;
        }

        public bool GoDown(Block[][] grid)
        {
            ClearGrid(grid);
            Yindex++;
            bool res = FillGrid(grid,Blocks);
            if (!res)
                Yindex--;
            return res;
        }
        public bool Rotate(Block[][] grid)
        {
            ClearGrid(grid);


            int n = Blocks.Length;
            int m = Blocks[0].Length;
            Block[][] res = new Block[m][];
            for (int i = 0; i < m; i++)
            {
                res[i] = new Block[n];
                for (int j = n-1,k = 0; j >= 0; j--,k++)
                {
                    res[i][k] = Blocks[j][i];
                }
            }

            bool flag = FillGrid(grid,res);
            if(flag)
                Blocks = res;
            return flag;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Logic
{
    public class FigureFactory
    {
        private int Cols { get; }
        private int Rows { get; }
        private int WH { get; }

        private readonly string[] figures = { "I", "J", "L", "O", "S", "T", "Z" };
        private Random r = new Random();
        private string nextFigure;
        private string currentFigure;

        public FigureFactory(int cols,int rows,int wh)
        {
            Cols = cols;
            Rows = rows;
            WH = wh;
            NextFigure();
        }
        public string PeekNextFigure()
        {
            return nextFigure;
        }
        public string NextFigure()
        {
            currentFigure = nextFigure;
            nextFigure = figures[r.Next(0, figures.Length)];
            return currentFigure;
        }

        private Figure CreateI(Brush brush)
        {
            Block[][] res = new Block[1][];
            res[0] = new Block[4];
            for (int i = 0; i < 4; i++)
            {
                res[0][i] = new Block(WH, brush);
            }
            return new Figure(res, WH);
        }
        private Figure CreateJ(Brush brush)
        {
            Block[][] res = new Block[2][];
            for (int i = 0; i < 2; i++)
            {
                res[i] = new Block[3];
            }
            res[0][2] = new Block(WH, brush);
            for (int i = 0; i < 3; i++)
            {
                res[1][i] = new Block(WH, brush);
            }
            return new Figure(res, WH);
        }
        private Figure CreateL(Brush brush)
        {
            Block[][] res = new Block[2][];
            for (int i = 0; i < 2; i++)
            {
                res[i] = new Block[3];
            }
            res[1][2] = new Block(WH, brush);
            for (int i = 0; i < 3; i++)
            {
                res[0][i] = new Block(WH, brush);
            }
            return new Figure(res, WH);
        }
        private Figure CreateO(Brush brush)
        {
            Block[][] res = new Block[2][];
            res = new Block[2][];
            for (int i = 0; i < 2; i++)
            {
                res[i] = new Block[2];
                for (int j = 0; j < 2; j++)
                {
                    res[i][j] = new Block(WH, brush);
                }
            }
            return new Figure(res, WH);
        }
        private Figure CreateS(Brush brush)
        {
            Block[][] res = new Block[3][];
            for (int i = 0; i < 3; i++)
            {
                res[i] = new Block[2];
            }
            res[0][1] = new Block(WH, brush);
            res[1][0] = new Block(WH, brush);
            res[1][1] = new Block(WH, brush);
            res[2][0] = new Block(WH, brush);
            return new Figure(res, WH);
        }
        private Figure CreateT(Brush brush)
        {
            Block[][] res = new Block[3][];
            for (int i = 0; i < 3; i++)
            {
                res[i] = new Block[2];
            }
            for (int i = 0; i < 3; i++)
            {
                res[i][1] = new Block(WH, brush);
            }
            res[1][0] = new Block(WH, brush);
            return new Figure(res, WH);
        }
        private Figure CreateZ(Brush brush)
        {
            Block[][] res = new Block[3][];
            for (int i = 0; i < 3; i++)
            {
                res[i] = new Block[2];
            }
            res[0][0] = new Block(WH, brush);
            res[1][0] = new Block(WH, brush);
            res[1][1] = new Block(WH, brush);
            res[2][1] = new Block(WH, brush);
            return new Figure(res, WH);
        }

        public Figure CreateFigure(string figure)
        {
            switch (figure)
            {
                case "I":
                    {
                        return CreateI(Brushes.Cyan);
                    }
                case "J":
                    {
                        return CreateJ(Brushes.Blue);
                    }
                case "L":
                    {
                        return CreateL(Brushes.Orange);
                    }
                case "O":
                    {
                        return CreateO(Brushes.Yellow);
                    }
                case "S":
                    {
                        return CreateS(Brushes.Green);
                    }
                case "T":
                    {
                        return CreateT(Brushes.Purple);
                    }
                case "Z":
                    {
                        return CreateZ(Brushes.Red);
                    }
                default:
                    {
                        return null;
                    }
            }
        }
    }
}

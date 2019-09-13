using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tetris.Logic;
using Tetris.UIElements;

namespace Tetris.Components
{
    public class TetrisGame : IComponent
    {
        private GameOptions GameOptions { get; set; }
        private Game Game { get; }

        public FigureFactory FigureFactory { get; }

        public int Cols => 10;
        public int Rows => 20;
        public int BlockWH => Game.CanvasWidth / (2 * (Cols+1));

        private UITextElement NextFigure { get; }
        private UITextElement ScoreUI { get; }

        public SoundPlayer SoundPlayer { get; set; }

        public Block[][]Grid { get; private set; }

        public int Scores
        {
            get
            {
                return _scores;
            }
            set
            {
                _scores = value;
                ScoreUI.Text = ScoreText;
            }
        }
        private int _scores = 0;
        private string ScoreText => "Scores : " + Scores;

        private readonly Pen BorderPen = Pens.Gray;
        private readonly int X = 20;
        private int Y => -BlockWH*3;

        private int RightX => Game.CanvasWidth / 2 + 40;

        private event Action FigureChange;
        private Figure currentFigure;
        private Figure nextFigure;

        private bool GoFast { get; set; } = false;

        public TetrisGame(Game game, GameOptions gameOptions)
        {
            GameOptions = gameOptions;
            Game = game;
            Grid = new Block[Cols][];
            for (int i = 0; i < Cols; i++)
            {
                Grid[i] = new Block[Rows];
            }
            Font font = new Font("Microsoft Sans Serif", 14, FontStyle.Bold, GraphicsUnit.Pixel);
            NextFigure = new UITextElement("Next figure: ", font, Brushes.White,RightX , BlockWH*2);
            ScoreUI = new UITextElement(ScoreText, font, Brushes.White,RightX, BlockWH*10);

            FigureFactory = new FigureFactory(Cols, Rows, BlockWH);
            FigureChange += OnFigureChange;
            FigureChange += OnRowDestroy;
            FigureChange();
        }
        private void OnFigureChange()
        {
            currentFigure = FigureFactory.CreateFigure(FigureFactory.NextFigure());
            currentFigure.Xindex = Cols / 2 - 1;
            currentFigure.Yindex = -2;
            nextFigure = FigureFactory.CreateFigure(FigureFactory.PeekNextFigure());
        }
        private void OnRowDestroy()
        {
            Game.CurrentState = States.RowDestroyBegin;
            
        }
        public void Start()
        {
            if(SoundPlayer==null)
                SoundPlayer = new SoundPlayer(Properties.Resources.GameSong);
            SoundPlayer.PlayLooping();
        }
        private double elapsed = 0;
        public void Update()
        {
            int denumerator = GoFast ? 20 : GameOptions.Difficulty;
            if(elapsed > 1000 / denumerator)
            {
                elapsed = 0;
                Block[][] clone = CloneGrid();
                if (currentFigure.GoDown(clone))
                {
                    Grid = clone;
                }
                else
                {
                    FigureChange();
                    for (int i = 0; i < currentFigure.Blocks.Length; i++)
                    {
                        for (int j = 0; j < currentFigure.Blocks[i].Length; j++)
                        {
                            int xoff = currentFigure.Xindex + i;
                            int yoff = currentFigure.Yindex + j;
                            if(xoff>=0 && xoff <Grid.Length && yoff>=0 && yoff < Grid[xoff].Length)
                            {
                                if (Grid[xoff][yoff] != null)
                                {
                                    Game.CurrentState = States.GameOverInit;
                                }
                            }
                           
                        }
                    }
                }
            }
            elapsed += 1000 / 60;
        }

        public void Draw()
        {
            Graphics g = Game.Graphics;

            NextFigure.Draw(g);
            ScoreUI.Draw(g);

            int rowoff = -Y;
            g.DrawLine(BorderPen, X, Y + rowoff, X, Y + Rows * BlockWH + rowoff);
            g.DrawLine(BorderPen, X, Y+Rows*BlockWH + rowoff, X+Cols*BlockWH, Y + Rows * BlockWH + rowoff);
            g.DrawLine(BorderPen, X + Cols * BlockWH, Y + Rows * BlockWH + rowoff, X + Cols * BlockWH, Y + rowoff);//border

            int nextFigureBorderOffsetY = 20;
            int y = nextFigureBorderOffsetY + BlockWH*2;
            g.DrawRectangle(BorderPen, RightX, y, BlockWH * 5,BlockWH * 6);//next figure border
            nextFigure.X = RightX + BlockWH;
            nextFigure.Y = y + BlockWH;
            nextFigure.Draw(g);

            for (int i = 0; i < Cols; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    if (Grid[i][j] != null)
                    {
                        int posx = i * BlockWH + X;
                        int posy = j * BlockWH;
                        g.FillRectangle(Grid[i][j].Brush, posx, posy, BlockWH, BlockWH);
                    }
                }
            }

        }

        private Block[][] CloneGrid()
        {
            return Grid.Select(x => x.Select(y => y).ToArray()).ToArray();
        }

       

        public void Dispose()
        {
            FigureChange -= OnFigureChange;
            FigureChange -= OnRowDestroy;
            GoFast = false;
            ScoreUI.Dispose();
            NextFigure.Dispose();
            SoundPlayer.Dispose();
        }
        public void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Down)
            {
                GoFast = false;
            }
            if (Game.CurrentState == States.FigureFalling)
            {
                switch (e.KeyData)
                {
                    case Keys.Up:
                        {
                            Block[][] clone = CloneGrid();
                            if (currentFigure.Rotate(clone))
                            {
                                Grid = clone;
                            }
                            break;
                        }
                }
            }
            
        }
        public void OnKeyDown(object sender,KeyEventArgs e)
        {
            if (Game.CurrentState == States.FigureFalling)
            {
                Block[][] clone = CloneGrid();
                switch (e.KeyData)
                {
                    case Keys.Left:
                        {
                            if (currentFigure.Move(clone, -1))
                            {
                                Grid = clone;
                            }
                            break;
                        }
                    case Keys.Right:
                        {
                            if (currentFigure.Move(clone, 1))
                            {
                                Grid = clone;
                            }
                            break;
                        }
                    
                }
            }
            if(e.KeyData == Keys.Down)
            {
                GoFast = true;
            }
        }

        public void Stop()
        {
            SoundPlayer?.Stop();
        }
    }
}

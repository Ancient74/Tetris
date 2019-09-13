using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tetris.Components;
using Tetris.Util;

namespace Tetris
{
    public partial class Game : Form
    {
        public Graphics Graphics { get; }
        public Bitmap Bitmap { get; }
        public States CurrentState { get; set; }
        public int CanvasWidth => pictureBox1.Width;
        public int CanvasHeight => pictureBox1.Height;

        private Intro Intro { get; set; }
        private MainMenu MainMenu { get; set; }
        private TetrisGame TetrisGame { get; set; }
        private GameOverScreen GameOver { get; set; }

        private List<int> _indexes;
        private List<List<Brush>> _rowBrushes;
        private double elapsed;
        private int couner = 0;

        public Game()
        {
            InitializeComponent();

            DoubleBuffered = true;

            Bitmap = new Bitmap(CanvasWidth,CanvasHeight);
            Graphics = Graphics.FromImage(Bitmap);
            Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            CurrentState = States.PreIntro;

            timer1.Interval = 1000 / 60;
            timer1.Tick += Update;
            timer1.Start();

            Intro = new Intro(this);
            MainMenu = new MainMenu(this);
        }
       
        public void Update(object sender, EventArgs e)
        {
            Graphics.Clear(Color.Black);
            WindowInfo.CursorPosition = PointToClient(Cursor.Position);

            switch (CurrentState)
            {
                case States.PreIntro:
                    {
                        CurrentState = States.Intro;
                        Intro.Start();
                        break;
                    }
                case States.Intro:
                    {
                        Intro.Update();
                        Intro.Draw();
                        break;
                    }
                case States.MainMenuInit:
                    {
                        Intro.Dispose();
                        pictureBox1.Click += MainMenu.OnClick;
                        if (GameOver!=null)
                        {
                            pictureBox1.Click -= GameOver.OnClick;
                            GameOver.Dispose();
                        }
                        if (TetrisGame != null)
                        {
                            TetrisGame.Stop();
                        }
                        MainMenu.Start();
                        CurrentState = States.MainMenu;
                        break;
                    }
                case States.MainMenu:
                    {
                        MainMenu.Update();
                        MainMenu.Draw();
                        break;
                    }
                case States.GamePreload:
                    {
                        TetrisGame = new TetrisGame(this, MainMenu.GameOptions);
                        KeyUp += TetrisGame.OnKeyUp;
                        KeyDown += TetrisGame.OnKeyDown;
                        pictureBox1.Click -= MainMenu.OnClick;
                        MainMenu.Stop();
                        TetrisGame.Start();
                        CurrentState = States.FigureFalling;
                        break;
                    }
                case States.FigureFalling:
                    {
                        TetrisGame.Update();
                        TetrisGame.Draw();
                        break;
                    }
                case States.GameOverInit:
                    {
                        GameOver = new GameOverScreen(this, TetrisGame.Scores);
                        KeyUp -= TetrisGame.OnKeyUp;
                        KeyDown -= TetrisGame.OnKeyDown;
                        TetrisGame.Dispose();
                        CurrentState = States.GameOver;
                        pictureBox1.Click += GameOver.OnClick;
                        break;
                    }
                case States.GameOver:
                    {
                        GameOver.Update();
                        GameOver.Draw();
                        break;
                    }
                case States.RowDestroyBegin:
                    {
                        TetrisGame.Draw();
                        _indexes = new List<int>();
                        for (int i = 0; i < TetrisGame.Rows; i++)
                        {
                            bool flag = true;
                            for (int j = 0; j < TetrisGame.Cols; j++)
                            {
                                if (TetrisGame.Grid[j][i] == null)
                                {
                                    flag = false;
                                    break;
                                }
                            }
                            if (flag)
                            {
                                _indexes.Add(i);
                            }
                        }
                        if (_indexes.Count == 0)
                            CurrentState = States.FigureFalling;
                        else
                        {
                            _rowBrushes = new List<List<Brush>>();
                            for (int i = 0; i < _indexes.Count; i++)
                            {
                                _rowBrushes.Add(new List<Brush>());
                                for (int j = 0; j < TetrisGame.Cols; j++)
                                {
                                    _rowBrushes[i].Add(TetrisGame.Grid[j][_indexes[i]].Brush);
                                }
                            }
                            CurrentState = States.RowDestroying;
                        }
                        break;
                    }
                case States.RowDestroying:
                    {
                        elapsed += 1000 / 60;
                        TetrisGame.Draw();
                        if(elapsed > 1000 / 4)
                        {
                            elapsed = 0;
                            couner++;
                            if (couner % 2 == 0)
                            {
                                for (int i = 0; i < _indexes.Count; i++)
                                {
                                    for (int j = 0; j < TetrisGame.Cols; j++)
                                    {
                                        TetrisGame.Grid[j][_indexes[i]].Brush = Brushes.Black;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < _indexes.Count; i++)
                                {
                                    for (int j = 0; j < TetrisGame.Cols; j++)
                                    {
                                        TetrisGame.Grid[j][_indexes[i]].Brush = _rowBrushes[i][j];
                                    }
                                }
                            }
                            if(couner >= 5)
                            {
                                for (int i = 0; i < _indexes.Count; i++)
                                {
                                    for (int j = 0; j < TetrisGame.Cols; j++)
                                    {
                                        TetrisGame.Grid[j][_indexes[i]] = null;
                                    }
                                }
                                _indexes.Sort();
                                _indexes.Reverse();

                                for (int i = 0; i < _indexes.Count; i++)
                                {
                                    for (int j = _indexes[i]+i; j >0 ; j--)
                                    {
                                        for (int c = 0; c < TetrisGame.Cols; c++)
                                        {
                                            TetrisGame.Grid[c][j] = TetrisGame.Grid[c][j - 1];
                                        }
                                    }
                                }
                                TetrisGame.Scores += 100 * _indexes.Count * _indexes.Count;

                                CurrentState = States.FigureFalling;
                                couner = 0;
                            }
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            pictureBox1.Image = Bitmap;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.Components;
using Tetris.UIElements;

namespace Tetris
{
    public class Intro : IComponent
    {
        private Game Game { get; set; }
        public TimeSpan Delay { get; private set; }
        public TimeSpan Appearance { get; private set; }

        private DateTime StartTime { get; set; }
        private Font Font { get; }
        private StringFormat StringFormat { get; }

        public UITextElement Title { get; }
        public UITextElement SubTitle { get; }


        private TimeSpan diff;

        public Intro(Game game)
        {
            Game = game;
            Delay = TimeSpan.FromSeconds(3);
            Appearance = TimeSpan.FromSeconds(1);
            Font = new Font("Microsoft Sans Serif", 24, FontStyle.Bold, GraphicsUnit.Pixel);
            StringFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            Title = new UITextElement("Tetris", Font, null, 0, 0,StringFormat);
            SubTitle = new UITextElement("By Yura Ruban", Font, null, 0, 0, StringFormat);
        }
        public void Start()
        {
            StartTime = DateTime.Now;
        }
        public void Update()
        {
            diff = DateTime.Now - StartTime;
            if (diff > Delay)
            {
                Game.CurrentState = States.MainMenuInit;
            }
        }
        public void Draw()
        {
            Brush brush;
            if (diff < Appearance)
            {
                double alpha = MathUtil.Map(diff.Milliseconds, 0, Appearance.TotalMilliseconds, 0, 255);
                brush = new SolidBrush(Color.FromArgb((int)alpha, 255, 255, 255));
            }
            else
            {
                brush = Brushes.White;
            }
            PointF titlePoint = new PointF(Game.CanvasWidth / 2, Game.CanvasHeight / 2 - 16);
            PointF subtitlePoint = new PointF(Game.CanvasWidth / 2, Game.CanvasHeight / 2 + 16);

            Title.X = (int)titlePoint.X;
            Title.Y = (int)titlePoint.Y;
            Title.Brush = brush;
            SubTitle.X = (int)subtitlePoint.X;
            SubTitle.Y = (int)subtitlePoint.Y;
            SubTitle.Brush = brush;

            Title.Draw(Game.Graphics);
            SubTitle.Draw(Game.Graphics);

        }

        public void Dispose()
        {
            Title.Dispose();
            SubTitle.Dispose();
            Font.Dispose();
            StringFormat.Dispose();
        }

        public void Stop()
        {
        }
    }
}

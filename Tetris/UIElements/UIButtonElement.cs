using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tetris.Util;

namespace Tetris.UIElements
{
    public class UIButtonElement : IDisposable
    {
        public string Text { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
       
        public Brush ActiveBkgBrush
        {
            get
            {
                return _activeBkgBrush;
            }
            set
            {
                _activeBkgBrush = value;
                if (Active)
                {
                    CurrentBkgBrush = _activeBkgBrush;
                }
            }
        }
        public Brush UnactiveBkgBrush
        {
            get
            {
                return _unactiveBkgBrush;
            }
            set
            {
                _unactiveBkgBrush = value;
                if (!Active)
                {
                    CurrentBkgBrush = _unactiveBkgBrush;
                }

            }
        }
        public Brush ActiveFontBrush
        {
            get
            {
                return _activeFontBrush;
            }
            set
            {
                _activeFontBrush = value;
                if (Active)
                {
                    CurrentBkgBrush = _activeFontBrush;
                }
            }
        }
        public Brush UnactiveFontBrush
        {
            get
            {
                return _unactiveFontBrush;
            }
            set
            {
                _unactiveFontBrush = value;
                if (!Active)
                {
                    CurrentBkgBrush = _unactiveFontBrush;
                }

            }
        }

        public Font Font { get; set; }
        public Brush CurrentBkgBrush { get; private set; }
        public Brush CurrentFontBrush { get; private set; }

        public bool Active
        {
            get
            {
                return _active;
            }
            set
            {
                if(value)
                {
                    CurrentBkgBrush = ActiveBkgBrush;
                    CurrentFontBrush = ActiveFontBrush;
                }
                else
                {
                    CurrentBkgBrush = UnactiveBkgBrush;
                    CurrentFontBrush = UnactiveFontBrush;
                }
                _active = value;
            }
        }

        private StringFormat format;

        private bool _active;

        private Brush _activeBkgBrush;
        private Brush _unactiveBkgBrush;
        private Brush _activeFontBrush;
        private Brush _unactiveFontBrush;

        public UIButtonElement(string text, int x, int y, int w, int h, Brush activeBkgStateBrush, Brush unactiveBkgStateBrush, Brush activeFontStateBrush, Brush unactiveFontStateBrush , Font font)
        {
            Text = text;
            X = x;
            Y = y;
            Width = w;
            Height = h;
            format = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            Font = font;
            _activeBkgBrush = activeBkgStateBrush;
            _activeFontBrush = activeFontStateBrush;
            _unactiveBkgBrush = unactiveBkgStateBrush;
            _unactiveFontBrush = unactiveFontStateBrush;
        }

        public void Draw(Graphics graphics)
        {
            graphics.FillRectangle(CurrentBkgBrush, X, Y, Width, Height);
            graphics.DrawString(Text,Font, CurrentFontBrush, X + Width / 2, Y + Height / 2, format);

        }
        public void Update()
        {
            Point pos = WindowInfo.CursorPosition;
            if (pos.X > X && pos.X < X + Width
                && pos.Y > Y && pos.Y < Y + Height)
                Active = true;
            else
                Active = false;
        }

        public void Dispose()
        {
            format.Dispose();
        }


    }
}

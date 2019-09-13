using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.UIElements
{
    public class UITextElement : IDisposable
    {
        public string Text { get; set; }
        public Font Font { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Brush Brush { get; set; }
        public StringFormat StringFormat { get; set; }

        public UITextElement(string text,Font font, Brush brush, int x, int y,StringFormat format = null )
        {
            Text = text;
            Font = font;
            X = x;
            Y = y;
            Brush = brush;
            StringFormat = format;
        }
        public virtual void Draw(Graphics graphics)
        {
            graphics.DrawString(Text, Font, Brush, X, Y, StringFormat);
        }

        public void Dispose()
        {
            StringFormat?.Dispose();
        }
    }
}

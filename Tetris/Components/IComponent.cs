using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Components
{
    public interface IComponent : IDisposable
    {
        void Start();
        void Update();
        void Draw();
        void Stop();
    }
}

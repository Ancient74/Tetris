using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public static class MathUtil
    {
        public static double Map(double x, double minX,double maxX,double newMinX, double newMaxX)
        {
            return (((x - minX) / (maxX - minX)) * (newMaxX - newMinX)) + newMinX;
        }
    }
}

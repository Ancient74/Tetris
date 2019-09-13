using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Logic
{
    public class GameOptions
    {
        private int _difficulty = 1;


        public int Difficulty
        {
            get
            {
                return _difficulty;
            }
            set
            {
                value = value % 10;
                if (value < 0)
                    value = 10 + value;
                if (value == 0)
                    value++;
                _difficulty = value;
            }
        }
    }
}

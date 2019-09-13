using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public enum States
    {
        PreIntro,
        Intro,
        MainMenuInit,
        MainMenu,
        GamePreload,
        FigureFalling,
        RowDestroyBegin,
        RowDestroying,
        GameOverInit,
        GameOver
    }
}

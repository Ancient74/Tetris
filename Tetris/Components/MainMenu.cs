using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using Tetris.Components;
using Tetris.Logic;
using Tetris.UIElements;

namespace Tetris
{
    public class MainMenu : IComponent
    {
        private Game Game { get; }
        private Font Font { get; }
        
        private UIButtonElement StartButton { get; }
        private UIButtonElement DifficultyButton { get; }

        public SoundPlayer SoundPlayer { get; private set; }

        public List<UIButtonElement> Buttons { get; }

        public GameOptions GameOptions { get; set; }

        private string DifficultyText => "Difficulty : " + GameOptions.Difficulty;

        public MainMenu(Game game)
        {
            Game = game;
            GameOptions = new GameOptions
            {
                Difficulty = 5
            };
            Font = new Font("Microsoft Sans Serif", 14, FontStyle.Bold, GraphicsUnit.Pixel);
            StartButton = new UIButtonElement("Start Game", (int)(Game.CanvasWidth * 0.5) - 100, (int)(Game.CanvasHeight * 0.5), 200, 40, Brushes.LightGray, Brushes.Gray, Brushes.Black, Brushes.White, Font);
            DifficultyButton = new UIButtonElement(DifficultyText, (int)(Game.CanvasWidth * 0.5) - 100, (int)(Game.CanvasHeight * 0.6), 200, 40, Brushes.LightGray, Brushes.Gray, Brushes.Black, Brushes.White, Font);
            Buttons = new List<UIButtonElement> { StartButton, DifficultyButton };
        }
        public void OnClick(object sender, EventArgs e)
        {
            UIButtonElement active = Buttons.Find(x => x.Active);
            if(active == StartButton)
            {
                Game.CurrentState = States.GamePreload;
            }
            else if(active == DifficultyButton)
            {
                GameOptions.Difficulty++;
                DifficultyButton.Text = DifficultyText;
            }
        }
        public void Update()
        {
            Buttons.ForEach(x => x.Update());
        }
        public void Draw()
        {
            Game.Graphics.DrawImage(Properties.Resources.Logo, new Point((int)(Game.CanvasWidth/2 - Properties.Resources.Logo.Width/1.5), 40));
            Buttons.ForEach(x => x.Draw(Game.Graphics));
        }

        public void Dispose()
        {
            Buttons.ForEach(x => x.Dispose());
            SoundPlayer.Dispose();
        }

        public void Start()
        {
            if(SoundPlayer==null)
                SoundPlayer = new SoundPlayer(Properties.Resources.MainMenuSong);
            SoundPlayer.PlayLooping();
        }
        public void Stop()
        {
            SoundPlayer?.Stop();
        }
    }
}

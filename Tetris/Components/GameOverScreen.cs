using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.UIElements;

namespace Tetris.Components
{
    public class GameOverScreen : IComponent
    {

        private Game Game { get; }
        private int Scores { get; set; }
        private UITextElement GameOverText;
        private UITextElement ScoresText;
        private UIButtonElement BackToMenuButton { get; set; }

        public GameOverScreen(Game game, int scores)
        {
            Game = game;
            Scores = scores;
            Font font = new Font("Microsoft Sans Serif", 28, FontStyle.Bold, GraphicsUnit.Pixel);
            Font fontBtn = new Font("Microsoft Sans Serif", 14, FontStyle.Bold, GraphicsUnit.Pixel);
            StringFormat format = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            GameOverText = new UITextElement("GAME OVER", font, Brushes.White, game.CanvasWidth / 2, game.CanvasHeight / 2 - 16, format);
            ScoresText = new UITextElement("Yours score: " + Scores, font, Brushes.White, game.CanvasWidth / 2, game.CanvasHeight / 2 + 16, format);
            BackToMenuButton = new UIButtonElement("BACK TO MENU", game.CanvasWidth / 2 - 100, (int)(game.CanvasHeight * 0.7), 200, 30, Brushes.LightGray, Brushes.Gray, Brushes.Black, Brushes.White, fontBtn);
        }

        public void OnClick(object sender,EventArgs e)
        {
            if (BackToMenuButton.Active)
                Game.CurrentState = States.MainMenuInit;
        }

        public void Dispose()
        {
            GameOverText.Dispose();
            ScoresText.Dispose();
            BackToMenuButton.Dispose();
        }

        public void Draw()
        {
            GameOverText.Draw(Game.Graphics);
            ScoresText.Draw(Game.Graphics);
            BackToMenuButton.Draw(Game.Graphics);

        }

        public void Start()
        {
        }

        public void Update()
        {
            BackToMenuButton.Update();
        }

        public void Stop()
        {
        }
    }
}

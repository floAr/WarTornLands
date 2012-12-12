using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WarTornLands.Infrastructure.Systems.GameState.States
{
    public class MainMenueState : BaseGameState
    {
        public override void Initialize()
        {

        }

        public override void LoadContent()
        {

        }

        public override void Pause()
        {

        }

        public override void Resume()
        {

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Game1.Instance.Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                Game1.Instance.PushState(new RunningGameState());
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Game1.Instance.SpriteBatch.Begin();
            Game1.Instance.SpriteBatch.DrawString(Game1.Instance.Content.Load<SpriteFont>("font/DialogFont"), "MAINMENUE\n\nPress {Enter} to start new Game or {ESC} to exit", new Vector2(50, 150), Color.CadetBlue);
            Game1.Instance.SpriteBatch.End();
        }
    }
}

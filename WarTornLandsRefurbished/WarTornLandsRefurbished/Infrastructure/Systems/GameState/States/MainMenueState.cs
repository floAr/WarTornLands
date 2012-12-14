using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarTornLands.Infrastructure.Systems.InputSystem;

namespace WarTornLands.Infrastructure.Systems.GameState.States
{
    public class MainMenueState : BaseGameState
    {
        public override void Initialize()
        {
            this._inputSheet.RegisterKey("Exit", Keys.Escape);
            this._inputSheet.RegisterKey("New", Keys.Enter);
        }

        public override void LoadContent()
        {
        
            base.LoadContent();
        }

     

        public override void Pause()
        {

        }

        public override void Resume()
        {

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            (_inputSheet["Exit"] as Key).Pressed += new EventHandler(ExitGame);
            (_inputSheet["New"] as Key).FreshPressed += new EventHandler(NewGame);
        }

        bool  debugFirstPush=true;
        void NewGame(object sender, EventArgs e)
        {
            if (debugFirstPush)
            {
                Game1.Instance.PushState(new RunningGameState());
                debugFirstPush = false;
            }

        }

        void ExitGame(object sender, EventArgs e)
        {
            Game1.Instance.Exit();
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Game1.Instance.SpriteBatch.Begin();
            Game1.Instance.SpriteBatch.DrawString(Game1.Instance.Content.Load<SpriteFont>("font/DialogFont"), "MAINMENUE\n\nPress {Enter} to start new Game or {ESC} to exit", new Vector2(50, 150), Color.CadetBlue);
            Game1.Instance.SpriteBatch.End();
        }
    }
}

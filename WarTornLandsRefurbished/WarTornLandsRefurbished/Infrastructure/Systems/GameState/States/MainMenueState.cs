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
        private static MainMenueState _instance = new MainMenueState();
        public static MainMenueState Instance { get { return _instance; } }

        public override void Initialize()
        {
            this._inputSheet.RegisterKey("Exit", Keys.Escape);
            this._inputSheet.RegisterKey("New", Keys.Enter);
            base.Initialize();
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
            base.Resume();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            (InputManager.Instance["Exit"] as Key).Pressed += new EventHandler(ExitGame);
            (InputManager.Instance["New"] as Key).FreshPressed += new EventHandler(NewGame);
        }

        void NewGame(object sender, EventArgs e)
        {
            {
                Game1.Instance.PushState(RunningGameState.Instance);
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

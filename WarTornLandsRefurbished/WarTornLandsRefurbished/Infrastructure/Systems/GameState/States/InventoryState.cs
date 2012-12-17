using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using WarTornLands.Infrastructure.Systems.InputSystem;

namespace WarTornLands.Infrastructure.Systems.GameState.States
{
    public class InventoryState:BaseOverlayGameState
    {
        public InventoryState(RenderTarget2D background) : base(background) { }

        public override void Initialize()
        {
            this._inputSheet.RegisterKey("Exit", Keys.I);

            base.Initialize();
          
        }

        public override void LoadContent()
        {
            //own logic here
            (InputManager.Instance["Exit"] as Key).Pressed += new EventHandler(CloseInventory);

            //bind input
            base.LoadContent();
        }

        void CloseInventory(object sender, EventArgs e)
        {
            Game1.Instance.PopState();
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
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            /*base.Draw(gameTime);
            //Own Draw logic here
            Game1.Instance.SpriteBatch.Begin();
              Game1.Instance.SpriteBatch.Draw(_background, Vector2.Zero, Color.Tomato);
            Game1.Instance.SpriteBatch.End();/**/
            _drawManager.Draw(
                _drawManager.B(
                    _drawManager.B(Game1.Instance.Player.Inventory),
                    _drawManager.B( _background,Game1.Instance.Content.Load<Effect>("effect/test")))
                , gameTime);
        }
    }
}

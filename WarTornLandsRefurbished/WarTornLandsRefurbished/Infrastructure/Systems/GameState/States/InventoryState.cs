using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WarTornLands.Infrastructure.Systems.GameState.States
{
    public class InventoryState:BaseOverlayGameState
    {
        public InventoryState(RenderTarget2D background) : base(background) { }

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
                Game1.Instance.PopState();
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Draw(gameTime);
            //Own Draw logic here
            Game1.Instance.SpriteBatch.Begin();
            Game1.Instance.Player.Inventory.Draw(gameTime);
            Game1.Instance.SpriteBatch.End();
        }
    }
}

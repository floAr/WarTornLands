using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WarTornLands.Infrastructure.Systems.GameState
{
    public abstract class BaseOverlayGameState:BaseGameState
    {
        protected RenderTarget2D _background;
        public BaseOverlayGameState(RenderTarget2D Background)
        {
            _background = Background;
        }
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Game1.Instance.SpriteBatch.Begin();
            Game1.Instance.SpriteBatch.Draw(_background, Vector2.Zero, Color.White);
            Game1.Instance.SpriteBatch.End();
        }
    }
}

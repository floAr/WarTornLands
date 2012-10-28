using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WarTornLands
{
    class Interface : DrawableGameComponent
    {
        private SpriteFont _font;

        public Interface(Game game) : base(game)
        {
            _font = Game.Content.Load<SpriteFont>("Test");
        }

        public override void Draw(GameTime gameTime)
        {
            String text = (Game as Game1)._player.GetHealth().ToString();
            (Game as Game1)._spriteBatch.Begin();
            (Game as Game1)._spriteBatch.DrawString(_font, text, new Vector2(Game.Window.ClientBounds.Width -
                _font.MeasureString(text).X - 10, 10), Color.White);
            (Game as Game1)._spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

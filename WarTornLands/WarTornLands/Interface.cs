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
        private Texture2D _heartTexture;
        private Texture2D _potionTexture;

        public Interface(Game game) : base(game)
        {
            _font = Game.Content.Load<SpriteFont>("Test");
            _heartTexture = Game.Content.Load<Texture2D>("heart");
            _potionTexture = Game.Content.Load<Texture2D>("potion");
        }

        public override void Draw(GameTime gameTime)
        {
            // Display potion count ("inventory")
            for (int i=0; i<(Game as Game1)._player.GetItemCount(); ++i)
            {
                (Game as Game1)._spriteBatch.Draw(_potionTexture, new Rectangle(10+42*i, 10, 32, 32), Color.White);
            }

            // Display health
            String text = (Game as Game1)._player.GetHealth().ToString();
            (Game as Game1)._spriteBatch.DrawString(_font, text, new Vector2(Game.Window.ClientBounds.Width -
                _font.MeasureString(text).X - 47, 10), Color.White);
            (Game as Game1)._spriteBatch.Draw(_heartTexture, new Rectangle(Game.Window.ClientBounds.Width-42, 10, 32, 32), Color.White);

            base.Draw(gameTime);
        }
    }
}

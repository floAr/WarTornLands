using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WarTornLands.PlayerClasses
{
    class Player : GameComponent
    {
        Vector2 _position = Vector2.Zero;
        Texture2D _texture;

        public Player(Game game) : base(game) { }

        public override void Update(GameTime gameTime)
        {
            Inputmanager input = (Game as Game1).input;

            if ((Game as Game1).testLevel.IsPixelAccessible(_position + input.Move))
            {
                _position += input.Move;
            }

            base.Update(gameTime);
        }

        public void LoadContent(ContentManager cm)
        {
            _texture = cm.Load<Texture2D>("player");
        }

        public void Draw(GameTime gameTime)
        {
            (Game as Game1).spriteBatch.Begin();
            (Game as Game1).spriteBatch.Draw(_texture, _position - new Vector2(_texture.Height) * .5f, Color.White);
            (Game as Game1).spriteBatch.End();
        }
    }
}

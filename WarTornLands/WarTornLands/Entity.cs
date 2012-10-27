using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WarTornLands
{
    public class Entity : DrawableGameComponent
    {
        private Texture2D _texture;
        private Vector2 _position;
        private Vector2 _offset;

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            ((SpriteBatch)Game.Services.GetService(typeof(SpriteBatch))).Draw(_texture, _position *Constants.TileSize + _offset, Color.White);
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
          //  (InputManager)Game.Services.GetService(typeof(InputManager)));
            if (_offset.X >= 16)
            {
                _offset.X -= 16;
                _position.X++;
            }
            if (_offset.Y >= 16)
            {
                _offset.Y -= 16;
                _position.Y++;
            }
            if (_offset.X <= -16)
            {
                _offset.X += 16;
                _position.X--;
            }
            if (_offset.Y <= -16)
            {
                _offset.Y += 16;
                _position.Y--;
            }
            base.Update(gameTime);
        }
      
    }
}

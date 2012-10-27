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

        public Entity(Game game) : base(game)
        {
        }

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
            if (_offset.X >= Constants.TileSize/2)
            {
                _offset.X -= Constants.TileSize;
                _position.X++;
            }
            if (_offset.Y >= Constants.TileSize/2)
            {
                _offset.Y -= Constants.TileSize;
                _position.Y++;
            }
            if (_offset.X <= -Constants.TileSize/2)
            {
                _offset.X += Constants.TileSize;
                _position.X--;
            }
            if (_offset.Y <= -Constants.TileSize/2)
            {
                _offset.Y += Constants.TileSize;
                _position.Y--;
            }
            base.Update(gameTime);
        }
      
    }
}

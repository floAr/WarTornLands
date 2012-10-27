using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WarTornLands
{
    public class Level : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private int[][,] grid;
        private List<Entity> dynamics;

        public Level(Game game) 
            : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            //((SpriteBatch)Game.Services.GetService(typeof(SpriteBatch))).Draw(_texture, _position * Constants.TileSize + _offset, Color.White);
            base.Draw(gameTime);
        }


    }
}

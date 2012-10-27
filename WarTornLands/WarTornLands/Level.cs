using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WarTornLands
{
    public class Level : DrawableGameComponent
    {
        private int[][,] grid;
        private List<Entity> dynamics;

        public Level(Game game) : base(game)
        {
        }

        public void AddLayer(int layerNumber, int[,] tiles)
        {
            grid[layerNumber] = tiles;
        }

        public void AddLayer(int[][,] tiles)
        {
            grid = tiles;
        }

        public void AddDynamicObject(Entity obj)
        {
            dynamics.Add(obj);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            for (int y = 0; y < squaresDown; y++)
            {
                for (int x = 0; x < squaresAcross; x++)
                {
                }
            }
            ((SpriteBatch)Game.Services.GetService(typeof(SpriteBatch))).Draw(_texture, _position * Constants.TileSize + _offset, Color.White);
            base.Draw(gameTime);
        }


    }
}

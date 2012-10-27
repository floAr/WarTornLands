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
        private List<Entity> dynamics = new List<Entity>();

        public Level(Game game) : base(game)
        {
            grid = new int[3][,];
        }

        public void AddLayer(int layerNumber, int[,] layer)
        {
            if (layerNumber < 0 || layerNumber > 2)
            {
                throw new Exception("Invalid layer number: 0 <= layer number <= 2!");
            }

            grid[layerNumber] = layer;
        }

        public void AddDynamics(List<Entity> obj)
        {
            dynamics.Concat(obj);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            (Game as Game1).spriteBatch.Begin();
            for (int y=0; y<grid[0].GetLength(1); ++y)
            {
                for (int x=0; x<grid[0].GetLength(0); ++x)
                {
                    if ((Game as Game1).TileSetTexture == null)
                    {
                        throw new Exception("BLa!");
                    }
                    (Game as Game1).spriteBatch.Draw(
                        (Game as Game1).TileSetTexture,
                        new Rectangle(x*Constants.TileSize, y*Constants.TileSize, Constants.TileSize, Constants.TileSize),
                        new Rectangle((grid[0][x, y] % 9) * Constants.TileSize, (grid[0][x, y] / 9) * Constants.TileSize, Constants.TileSize, Constants.TileSize),
                        Color.White);
                }
            }
            (Game as Game1).spriteBatch.End();

            base.Draw(gameTime);
        }


    }
}
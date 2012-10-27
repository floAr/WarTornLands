using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WarTornLands
{
    public class Level : GameComponent
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

        public void Draw(GameTime gameTime, int layer)
        {
            // Layer evtl. in Klasse (DrawableGameComponent) kapseln

            (Game as Game1).spriteBatch.Begin();
            for (int y=0; y<grid[layer].GetLength(1); ++y)
            {
                for (int x = 0; x < grid[layer].GetLength(0); ++x)
                {
                    int width = (int)Math.Floor((double)(Game as Game1).TileSetTexture.Width / Constants.TileSize);
                    (Game as Game1).spriteBatch.Draw(
                        (Game as Game1).TileSetTexture,
                        new Rectangle(x*Constants.TileSize, y*Constants.TileSize, Constants.TileSize, Constants.TileSize),
                        new Rectangle((grid[layer][x, y] % width) * Constants.TileSize, (grid[layer][x, y] / width) * Constants.TileSize, Constants.TileSize, Constants.TileSize),
                        Color.White);
                }
            }
            (Game as Game1).spriteBatch.End();
        }

        public bool IsPixelAccessible(Vector2 pixel)
        {
            int tilesetWidth = (int)Math.Floor((double)(Game as Game1).TileSetTexture.Width / Constants.TileSize);

            // Check Layer 1 collision
            // find corresponding tile
            Vector2 tile = new Vector2((float)Math.Floor(pixel.X / Constants.TileSize), (float)Math.Floor(pixel.Y / Constants.TileSize));
            Vector2 offset = new Vector2(pixel.X % Constants.TileSize, pixel.Y % Constants.TileSize);

            if (tile.X < grid[1].GetLength(0) && tile.Y < grid[1].GetLength(1))
            {
                if (grid[1][(int)tile.X, (int)tile.Y] != 0)
                {
                    return false;
                }
            }


            //grid[1][tile.X, tile.Y];


            // Check dynamic object collision


            return true;
        }

    }
}
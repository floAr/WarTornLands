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
            grid = new int[2][,];
        }

        public void AddFloor(int[,] layer)
        {
            grid[0] = layer;
        }

        public void AddCeiling(int[,] layer)
        {
            grid[1] = layer;
        }

        public void AddDynamics(Entity obj)
        {
            dynamics.Add(obj);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public void Draw(GameTime gameTime, int layer)
        {
            Vector2 center = (Game as Game1)._player.GetPosition();

            // Layer evtl. in Klasse (DrawableGameComponent) kapseln

            (Game as Game1)._spriteBatch.Begin();
            for (int y=0; y<grid[layer].GetLength(1); ++y)
            {
                for (int x = 0; x < grid[layer].GetLength(0); ++x)
                {
                    int width = (int)Math.Floor((double)(Game as Game1)._tileSetTexture.Width / Constants.TileSize);
                    (Game as Game1)._spriteBatch.Draw(
                        (Game as Game1)._tileSetTexture,
                        new Rectangle(x * Constants.TileSize - (int)center.X + (int)Math.Round((Game as Game1).Window.ClientBounds.Width / 2.0f),
                            y * Constants.TileSize - (int)center.Y + (int)Math.Round((Game as Game1).Window.ClientBounds.Height / 2.0f),
                            Constants.TileSize, Constants.TileSize),
                        new Rectangle((grid[layer][x, y] % width) * Constants.TileSize, (grid[layer][x, y] / width) * Constants.TileSize, Constants.TileSize, Constants.TileSize),
                        Color.White);
                }
            }

            (Game as Game1)._spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Entity ent in dynamics)
            {
                if (ent.GetHealth() == 0)
                {
                    ent.OnDie();
                    dynamics.Remove(ent);
                    GC.Collect();
                }
            }
        }

        public void DrawEntities(GameTime gameTime)
        {
            // Draw entities
            foreach (Entity ent in dynamics)
            {
                ent.Draw(gameTime);
            }
        }

        public bool IsPixelAccessible(Vector2 pixel)
        {
            // Check object collision
            if (GetEntityAt(pixel) != null)
                return false;

            return true;
        }

        public Entity GetEntityAt(Vector2 pixel)
        {
            foreach (Entity ent in dynamics)
            {
                Vector2 pos = ent.GetPosition();
                Vector2 size = ent.GetSize();

                if (pixel.X >= pos.X && pixel.X < pos.X + size.X &&
                    pixel.Y >= pos.Y && pixel.Y < pos.Y + size.Y)
                {
                    return ent;
                }
            }

            return null;
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarTornLands.EntityClasses;

namespace WarTornLands
{
    public class Level : GameComponent
    {
        private int[][,] _grid;
        private List<Entity> _dynamics = new List<Entity>();

        public Level(Game game) : base(game)
        {
            _grid = new int[2][,];
        }

        public void AddFloor(int[,] layer)
        {
            _grid[0] = layer;
        }

        public void AddCeiling(int[,] layer)
        {
            _grid[1] = layer;
        }

        public void AddDynamics(Entity obj)
        {
            _dynamics.Add(obj);
        }

        public void LoadContent()
        {
            foreach (Entity e in _dynamics)
            {
                e.LoadContent((Game as Game1).Content);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public void Draw(GameTime gameTime, int layer)
        {
            Vector2 center = (Game as Game1)._player.GetPosition();
            Game1 game = (this.Game as Game1);
            // Layer evtl. in Klasse (DrawableGameComponent) kapseln

   

            for (int y=0; y<_grid[layer].GetLength(1); ++y)
            {
                for (int x = 0; x < _grid[layer].GetLength(0); ++x)
                {
                    int width = (int)Math.Floor((double)game._tileSetTexture.Width / Constants.TileSize);

                    game._spriteBatch.Draw(
                        game._tileSetTexture,
                        new Rectangle(x * Constants.TileSize - (int)center.X + (int)Math.Round(game.Window.ClientBounds.Width / 2.0f),
                            y * Constants.TileSize - (int)center.Y + (int)Math.Round(game.Window.ClientBounds.Height / 2.0f),
                            Constants.TileSize, Constants.TileSize),
                        new Rectangle((_grid[layer][x, y] % width) * Constants.TileSize, 
                        (_grid[layer][x, y] / width) * Constants.TileSize, 
                        Constants.TileSize, 
                        Constants.TileSize),
                        Color.White);
                }
            }


        }

        public override void Update(GameTime gameTime)
        {
            try
            {
                foreach (Entity ent in _dynamics)
                {
                    ent.Update(gameTime);
                    if (ent.GetHealth() == 0)
                    {
                        ent.OnDie();
                        _dynamics.Remove(ent);
                        GC.Collect();
                    }
                }
            }
            catch { }
        }

        public void DrawEntities(GameTime gameTime)
        {
            // Draw entities
            foreach (Entity ent in _dynamics)
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
            foreach (Entity ent in _dynamics)
            {
                Vector2 pos = ent.GetPosition();
                Vector2 size = ent.GetSize();

                if (pixel.X >= pos.X - size.X * 0.5f && pixel.X < pos.X + size.X * 0.5f &&
                    pixel.Y >= pos.Y - size.Y * 0.5f && pixel.Y < pos.Y + size.Y * 0.5f)
                {
                    return ent;
                }
            }

            return null;
        }

    }
}
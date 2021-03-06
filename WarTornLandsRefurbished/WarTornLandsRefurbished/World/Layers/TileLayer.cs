﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarTornLands.Counter;
using WarTornLands.Infrastructure;

namespace WarTornLands.World.Layers
{
    public class TileLayer : Layer
    {
        private bool _isCollisionLayer;
        private CounterManager _cm;
        private int[,] _grid;
        private TileSetBox _tileSets;

        public TileLayer(int[,] grid, Area area)
            : base(area)
        {
            _grid = grid;
        }

        Rectangle sourceRec;
        Texture2D texture;

        public override void Draw(GameTime gameTime)
        {
            int tileCount = 0;
            Game1 game = Game1.Instance;
            Vector2 center = game.Camera.Center;

            // TODO:
            // Check whether Tiles are visible and just draw them if they are
            for (int y = 0; y < _grid.GetLength(1); ++y)
            {
                int yPos = (y + base._area.Bounds.Y) * Constants.TileSize;
                if (yPos < Game1.Instance.Camera.Center.Y - Game1.Instance.ClientBoundsHalf.Y - Constants.TileSize ||
                    yPos > Game1.Instance.Camera.Center.Y + Game1.Instance.ClientBoundsHalf.Y + Constants.TileSize)
                    continue;
                for (int x = 0; x < _grid.GetLength(0); ++x)
                {
                    int xPos = (x + base._area.Bounds.X) * Constants.TileSize;
                    if (xPos < Game1.Instance.Camera.Center.X - Game1.Instance.ClientBoundsHalf.X - Constants.TileSize ||
                        xPos > Game1.Instance.Camera.Center.X + Game1.Instance.ClientBoundsHalf.X + Constants.TileSize)
                        continue;
                    if (_grid[x, y] != 0)
                    {
                        int gid = _grid[x, y] - 1;

                        texture = _tileSets.GetTextureAndSourceRec(gid, out sourceRec);

                        game.SpriteBatch.Draw(
                            texture,
                            new Rectangle(xPos - (int)center.X + (int)game.ClientBoundsHalf.X,
                                yPos - (int)center.Y + (int)game.ClientBoundsHalf.Y,
                                Constants.TileSize, Constants.TileSize),
                            sourceRec,
                            Color.White);
                        tileCount++;
                    }
                }
            }
          }

        public override void Update(GameTime gameTime)
        {
            //if (_isAnimated)
            //{
            //    if (_cm != null)
            //        _cm.Update(gameTime);
            //}
        }

        public void SetTileSetBox(TileSetBox box)
        {
            _tileSets = box;
        }

        public bool IsPositionAccessible(Vector2 position)
        {
            position.X -= base._area.Bounds.X * Constants.TileSize;
            position.Y -= base._area.Bounds.Y * Constants.TileSize;

            int x = (int)position.X / Constants.TileSize;
            int y = (int)position.Y / Constants.TileSize;

            if (_grid[x, y] == 0)
                return true;

            if (_tileSets.ModifierOf(_grid[x,y] - 1) == 0)
                return false;

            return true;
        }

        public bool IsRectAccessible(Rectangle rect)
        {
            // Get corner points
            int x0 = (int)rect.Location.X / Constants.TileSize;
            int y0 = (int)rect.Location.Y / Constants.TileSize;
            int x1 = (int)(rect.Location.X + rect.Width - 1) / Constants.TileSize;
            int y1 = (int)(rect.Location.Y + rect.Height - 1) / Constants.TileSize;

            // Loop through all tiles in rectangle
            for (int x = x0; x <= x1; x++)
            {
                for (int y = y0; y <= y1; y++)
                {
                    // Skip a pixel if it lays outside of the area bounds
                    if (x < 0 || y < 0 || x >= _grid.GetLength(0) || y >= _grid.GetLength(1))
                        continue;

                    // Return false if tile is set and not accesible
                    if (_grid[x, y] != 0 && _tileSets.ModifierOf(_grid[x, y] - 1) == 0)
                        return false;
                }
            }

            // Return true if all tiles are accessible
            return true;
        }
    }
}

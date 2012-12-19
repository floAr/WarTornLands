using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.World.Layers;
using Microsoft.Xna.Framework;
using WarTornLands.Counter;
using Microsoft.Xna.Framework.Graphics;
using WarTornLands.Entities;
using WarTornLands.Infrastructure;
using WarTornLands.Infrastructure.Systems;

namespace WarTornLands.World.Layers
{
    public class TileLayer : Layer
    {
        private bool _isCollisionLayer;
        private CounterManager _cm;
        private int[,] _grid;
        private TileSetBox _tileSets;

        public TileLayer(int[,] grid)
            : base()
        {
            _grid = grid;
        }

        Rectangle sourceRec;
        Texture2D texture;

        public override void Draw(GameTime gameTime)
        {
            Game1 game = Game1.Instance;
            Vector2 center = game.Camera.Center;

            // TODO:
            // Check whether Tiles are visible and just draw them if they are
            for (int y = 0; y < _grid.GetLength(1); ++y)
            {
                for (int x = 0; x < _grid.GetLength(0); ++x)
                {
                    if (_grid[x, y] != 0)
                    {
                        int gid = _grid[x, y] - 1;

                       
                        texture = _tileSets.GetTextureAndSourceRec(gid, out sourceRec);

                        game.SpriteBatch.Draw(
                            texture,
                            new Rectangle(x * Constants.TileSize - (int)center.X + (int)game.ClientBoundsHalf.X,
                                y * Constants.TileSize - (int)center.Y + (int)game.ClientBoundsHalf.Y,
                                Constants.TileSize, Constants.TileSize),
                            sourceRec,
                            Color.White);
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
            int x = (int)position.X / Constants.TileSize;
            int y = (int)position.Y / Constants.TileSize;

            if (_grid[x, y] == 0)
                return true;

            if (_tileSets.ModifierOf(_grid[x,y] - 1) == 0)
                return false;

            return true;
        }
    }
}

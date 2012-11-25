using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.World.Layers;
using Microsoft.Xna.Framework;
using WarTornLands.Counter;
using Microsoft.Xna.Framework.Graphics;

namespace WarTornLands.World.Layers
{
    public struct Tile
    {
        /// <summary>
        /// Reference to tile in TileSet Texture, where 0 is transparent
        /// and other tiles are numbered consecutively.
        /// </summary>
        int TileNum;

        /// <summary>
        /// 
        /// </summary>
        char MoveSpeed;
    };

    public class TileLayer : Layer
    {
        private bool _isAnimated;
        private CounterManager _cm;
        private Tile[,] _grid;
        private Texture2D _tileSetTexture;

        public TileLayer(Game game, int depth)
            : base(game, depth)
        {
        }

        public void LoadGrid(Tile[,] grid, bool isAnimated, string tileSet)
        {
            _grid = grid;
            _isAnimated = isAnimated;
            _game.Content.Load<Texture2D>(tileSet);
        }

        public override void Draw(GameTime gameTime)
        {
           /* Game1 game = (_game as Game1);
            //Vector2 center = game.Player.Position;
            Vector2 center = new Vector2(0, 0);
            int width = (int)Math.Floor((double)_tileSetTexture.Width / Constants.TileSize);


            game.SpriteBatch.Begin();

            // TODO:
            // Check whether Tiles are visible and just draw them if they are
            for (int y = 0; y < _grid.GetLength(1); ++y)
            {
                for (int x = 0; x < _grid.GetLength(0); ++x)
                {
                    game.SpriteBatch.Draw(
                        _tileSetTexture,
                        new Rectangle(x * Constants.TileSize - (int)center.X + (int)Math.Round(game.Window.ClientBounds.Width / 2.0f),
                            y * Constants.TileSize - (int)center.Y + (int)Math.Round(game.Window.ClientBounds.Height / 2.0f),
                            Constants.TileSize, Constants.TileSize),
                        new Rectangle((_grid[x, y] % width) * Constants.TileSize,
                        (_grid[x, y] / width) * Constants.TileSize,
                        Constants.TileSize,
                        Constants.TileSize),
                        Color.White);
                }
            }

            game.SpriteBatch.End();*/
        }

        public override void Update(GameTime gameTime)
        {
            if (_isAnimated)
            {
                if (_cm != null)
                    _cm.Update(gameTime);
            }
        }
    }
}

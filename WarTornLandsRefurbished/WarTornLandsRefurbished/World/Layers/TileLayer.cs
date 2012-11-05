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
    class TileLayer : Layer
    {
        private Game _game;
        private bool _isAnimated;
        private CounterManager _cm;
        private int[,] _grid;
        private Texture2D _tileSetTexture;

        public TileLayer(Game game)
        {
            _game = game;
        }

        public void LoadGrid(int[,] grid, bool isAnimated, string tileSet)
        {
            _grid = grid;
            _isAnimated = isAnimated;
            _game.Content.Load<Texture2D>(tileSet);
        }

        public void Draw(GameTime gameTime)
        {
            Game1 game = (_game as Game1);
            Vector2 center = game.Player.Position;
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

            game.SpriteBatch.End();
        }

        public void Update(GameTime gameTime, Vector2 center)
        {
            if (_isAnimated)
            {
                if (_cm != null)
                    _cm.Update(gameTime);
            }
        }
    }
}

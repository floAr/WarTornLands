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

        public Level(Game game, int[,] layer0, int[,] layer1, int[,] layer2) : base(game)
        {
            grid = new int[3][,];
            grid[0] = layer0;
            grid[1] = layer1;
            grid[2] = layer2;
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
            for (int y=0; y<grid[0].GetLength(1); ++y)
            {
                for (int x=0; x<grid[0].GetLength(0); ++x)
                {
                    ((SpriteBatch)Game.Services.GetService(typeof(SpriteBatch))).Draw(
                        _texture, new Rectangle(x*Constants.TileSize, y*Constants.TileSize, Constants.TileSize, Constants.TileSize),
                         /* some texture */, Color.White);
                }
            }
            base.Draw(gameTime);
        }


    }
}
.
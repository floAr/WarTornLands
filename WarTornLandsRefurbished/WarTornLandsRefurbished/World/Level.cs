using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.World.Layers;
using Microsoft.Xna.Framework;
using WarTornLands;
using WarTornLands.Entities;

namespace WarTornLands.World
{
    public class Level
    {
        private Game _game;
        private LinkedList<Area> _areas;
 
        public Level(Game game)
        {
            _game = game;
            _areas = new LinkedList<Area>();
        }

        public void AddArea(Area area)
        {
            _areas.AddLast(area);
            area.Add();
        }

        public void RemoveArea(Area area)
        {
            area.Remove();
            _areas.Remove(area);
        }

        /// <summary>
        /// Queries the Tile Map whether a given pixel position is accessible.
        /// Entities are not considered in the collision check. This is done
        /// via GetEntitiesAt in the CollisionManager.
        /// </summary>
        /// <param name="position">Pixel position to check.</param>
        /// <returns></returns>
        public bool IsPositionAccessible(Vector2 position)
        {
            // TODO only check areas near the player
            foreach (Area area in _areas)
            {
                if (area.IsPositionAccessible(position) == false)
                    return false;
            }

            return true;
        }

        public List<Entity> GetEntitiesAt(Vector2 position)
        {
            List<Entity> result = new List<Entity>();

            // TODO only check areas near the player
            foreach (Area area in _areas)
            {
                result.Concat(area.GetEntitiesAt(position));
            }

            return result;
        }

        public void LoadLevel()
        {
            Area area1 = new Area();

            TileLayer layer1 = new TileLayer(_game, 0);
            Tile[,] grid1 = new Tile[10, 10];
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    if (x==3 || y==5)
                        grid1[x, y].TileNum = 3;
                    else
                        grid1[x, y].TileNum = 2;
                }
            }
            layer1.LoadGrid(grid1, false, "grass");
            area1.AddLayer(layer1);

            TileLayer layer2 = new TileLayer(_game, 100);
            Tile[,] grid2 = new Tile[10, 10];
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                        grid2[x, y].TileNum = 0;
                }
            }
            grid2[3, 3].TileNum = 6;
            grid2[4, 3].TileNum = 6;
            grid2[5, 3].TileNum = 6;
            grid2[5, 4].TileNum = 6;
            grid2[5, 5].TileNum = 6;
            grid2[4, 5].TileNum = 6;
            grid2[3, 5].TileNum = 6;

            layer2.LoadGrid(grid2, false, "grass");
            area1.AddLayer(layer2);

            AddArea(area1);
        }
    }
}

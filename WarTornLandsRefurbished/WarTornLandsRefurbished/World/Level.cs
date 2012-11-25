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

        public bool IsPositionAccessible(Vector2 position, Entity source)
        {
            // TODO return position accessible
            return true;
        }

        public List<Entity> GetEntitiesAt(Vector2 worldPosition)
        {
            // TODO entity at
            return null;
        }
    }
}

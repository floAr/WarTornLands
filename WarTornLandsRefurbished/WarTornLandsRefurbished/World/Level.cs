using System;
using System.Collections.Generic;
using WarTornLands.World.Layers;
using Microsoft.Xna.Framework;
using WarTornLands.Entities;
using System.Xml;
using WarTornLands.Entities.Modules.Draw;
using Microsoft.Xna.Framework.Graphics;
using WarTornLands.Entities.Modules.Die;
using WarTornLands.PlayerClasses;
using WarTornLands.Entities.Modules.Interact;
using WarTornLands.Infrastructure.Systems.DialogSystem;
using WarTornLands.Infrastructure;
using WarTornLands.Entities.Modules.Collide;
using WarTornLands.Entities.Modules.Draw.ParticleSystem;
using WarTornLands.Infrastructure.Systems.SkyLight;
using WarTornLands.Entities.Modules.Think;
using WarTornLands.PlayerClasses.Items;
using WarTornLands.Entities.Modules.Hit;
using System.Data;
using WarTornLands.Infrastructure.Systems.DrawSystem;

namespace WarTornLands.World
{
    public class Level : IDrawProvider
    {
        private Dictionary<string, Area> _areas;
        public List<DataSet> EntityTypeData { get; set; }

        public List<Entity> AreaIndependentEntities;

        private Random r = new Random();

        public Level()
        {
            AreaIndependentEntities = new List<Entity>();
            AreaIndependentEntities.Add(Game1.Instance.Player);
            _areas = new Dictionary<string, Area>();
        }

        public bool AddArea(string name, Area area)
        {
            if (!_areas.ContainsKey(name))
            {
                _areas.Add(name, area);
                return true;
            }

            return false;
        }

        public bool RemoveArea(string name)
        {
            if (_areas.ContainsKey(name))
            {
                _areas.Remove(name);
                return true;
            }

            return false;
        }

        public void Clear()
        {
            _areas.Clear();
        }

        public static Entity Ute;

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
            foreach (KeyValuePair<string, Area> pair in _areas)
            {
                if (pair.Value.IsPositionAccessible(position) == false)
                    return false;
            }

            return true;
        }

        public bool IsRectAccessible(Rectangle rect)
        {
            // TODO only check areas near the player
            foreach (KeyValuePair<string, Area> pair in _areas)
            {
                if (pair.Value.IsRectAccessible(rect) == false)
                    return false;
            }

            return true;
        }

        public List<Entity> GetEntitiesAt(Vector2 position)
        {
            List<Entity> result = new List<Entity>();

            // TODO only check areas near the player
            foreach (KeyValuePair<string, Area> pair in _areas)
            {
                result.AddRange(pair.Value.GetEntitiesAt(position));
            }

            if ((Player.Instance.Position - position).Length() < 40)
                result.Add(Player.Instance);

            return result;
        }

        public List<Entity> GetEntitiesAt(Vector2 position, int radius)
        {
            List<Entity> result = new List<Entity>();

            // TODO only check areas near the player
            foreach (KeyValuePair<string, Area> pair in _areas)
            {
                result.AddRange(pair.Value.GetEntitiesAt(position, radius));
            }

            return result;
        }

        public HashSet<Entity> GetEntitiesAt(Rectangle rect)
        {
            HashSet<Entity> result = new HashSet<Entity>();

            // TODO only check areas near the player
            foreach (KeyValuePair<string, Area> pair in _areas)
            {
                result.UnionWith(pair.Value.GetEntitiesAt(rect));
            }

            return result;
        }

        internal void Update(GameTime gameTime)
        {
            foreach (Area a in _areas.Values)
                a.Update(gameTime);

            List<Entity> buffer = new List<Entity>(AreaIndependentEntities);
            foreach (Entity e in buffer)
            {
                if (e.ToBeRemoved)
                    AreaIndependentEntities.Remove(e);
                e.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (Area a in _areas.Values)
                a.Draw(gameTime);

            foreach (Entity e in AreaIndependentEntities)
                e.Draw(gameTime);
        }
    }
}

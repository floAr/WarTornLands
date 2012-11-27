using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using WarTornLands.Entities;

namespace WarTornLands.World.Layers
{
    public class EntityLayer : Layer
    {
        private readonly List<Entity> _entities = new List<Entity>();

        public EntityLayer(Game game, int depth)
            : base(game, depth)
        {
        }

        public void AddEntity(Entity entity)
        {
            _entities.Add(entity);
        }

        public void AddRange(List<Entity> entities)
        {
            _entities.AddRange(entities);
        }

        public List<Entity> GetEntitiesAt(Vector2 position)
        {
            List<Entity> result = new List<Entity>();

            foreach (Entity ent in _entities)
            {
                Vector2 pos = ent.Position;
                Vector2 size = ent.Size;

                if (position.X >= pos.X - size.X * 0.5f && position.X < pos.X + size.X * 0.5f &&
                    position.Y >= pos.Y - size.Y * 0.5f && position.Y < pos.Y + size.Y * 0.5f)
                {
                    result.Add(ent);
                }
            }

            return result;
        }

        public List<Entity> GetEntitiesAt(Vector2 position, float radius)
        {
            List<Entity> result = new List<Entity>();

            foreach (Entity ent in _entities)
            {
                Vector2 entPos = ent.Position;
                Vector2 entSize = ent.Size;

                // TODO this alogrithm currently only works if either the circle's center
                // lies inside the entity or the entity's center / one of the cornes lies
                //  inside the circle. some cases are not caught by this!

                // Check whether circles center lies inside entity rectangle
                if (position.X >= entPos.X - entSize.X * 0.5f && position.X < entPos.X + entSize.X * 0.5f &&
                    position.Y >= entPos.Y - entSize.Y * 0.5f && position.Y < entPos.Y + entSize.Y * 0.5f)
                {
                    result.Add(ent);
                }
                else
                {
                    // Check whether entity center or one of the corners lies inside circle
                    if ((position - entPos).Length() <= radius ||
                        (position - (entPos + new Vector2(-entSize.X / 2, -entSize.Y / 2))).Length() <= radius ||
                        (position - (entPos + new Vector2(-entSize.X / 2, entSize.Y / 2))).Length() <= radius ||
                        (position - (entPos + new Vector2(entSize.X / 2, -entSize.Y / 2))).Length() <= radius ||
                        (position - (entPos + new Vector2(entSize.X / 2, entSize.Y / 2))).Length() <= radius)
                    {
                        result.Add(ent);
                    } 
                }
            }

            return result;
        }

        public override void Update(GameTime gameTime)
        {
            // TODO:
            // Check whether Entities are in the proximity of the Player and just update them if they are
            // Could also be done in the Entities class itself
            try
            {
                List<Entity> buffer = new List<Entity>(_entities);
                foreach (Entity ent in buffer)
                {
                    ent.Update(gameTime);
                    if (ent.IsDead)
                        _entities.Remove(ent);
                }
            }
            catch { }
        }

        public override void Draw(GameTime gameTime)
        {
            // TODO:
            // Check whether Entities are visible and just draw them if they are
            foreach (Entity ent in _entities)
            {
                ent.Draw(gameTime);
            }
        }


    }
}


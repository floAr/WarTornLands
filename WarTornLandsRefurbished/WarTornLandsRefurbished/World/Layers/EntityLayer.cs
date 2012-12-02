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

        public EntityLayer(int depth)
            : base(depth)
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

            // TODO fails if entity is not centered. We need a convention here!!!

            foreach (Entity ent in _entities)
            {
                // TODO remove debug code
                int resultLength = result.Count;

                Vector2 entPos = ent.Position;
                Vector2 entSize = ent.Size;

                // Check whether circles center lies inside entity rectangle
                if (position.X >= entPos.X - entSize.X * 0.5f && position.X < entPos.X + entSize.X * 0.5f &&
                    position.Y >= entPos.Y - entSize.Y * 0.5f && position.Y < entPos.Y + entSize.Y * 0.5f)
                {
                    result.Add(ent);
                }
                else
                {
                    // Check whether circle center lies straight next to one of the sides of the
                    // entity rectangle, not on one of the corners, and if so, compare distance
                    // Begin with top / bottom of the rectangle
                    if (position.X >= (entPos.X - entSize.X / 2.0f) && position.X <= (entPos.X + entSize.X / 2.0f))
                    {
                        if (position.Y < (entPos.Y - entSize.Y / 2.0f))
                        {
                            // Circle sphere on top
                            if ((entPos.Y - entSize.Y / 2.0f) - position.Y <= radius)
                            {
                                result.Add(ent);
                            }
                        }
                        else
                        {
                            // Circle sphere on the bottom
                            if (position.Y - (entPos.Y + entSize.Y / 2.0f) <= radius)
                            {
                                result.Add(ent);
                            }
                        }
                    }
                    else if (position.Y >= (entPos.Y - entSize.Y / 2.0f) && position.Y <= (entPos.Y + entSize.Y / 2.0f))
                    {
                        if (position.X < (entPos.X - entSize.X / 2.0f))
                        {
                            // Circle sphere on the left
                            if ((entPos.X - entSize.X / 2.0f) - position.X <= radius)
                            {
                                result.Add(ent);
                            }
                        }
                        else
                        {
                            // Circle sphere on the right
                            if (position.X - (entPos.X + entSize.X / 2.0f) <= radius)
                            {
                                result.Add(ent);
                            }
                        }
                    }
                    // Check whether one of the corners lies inside circle
                    if ((position - (entPos + new Vector2(-entSize.X / 2.0f, -entSize.Y / 2.0f))).Length() <= radius ||
                        (position - (entPos + new Vector2(-entSize.X / 2.0f, entSize.Y / 2.0f))).Length() <= radius ||
                        (position - (entPos + new Vector2(entSize.X / 2.0f, -entSize.Y / 2.0f))).Length() <= radius ||
                        (position - (entPos + new Vector2(entSize.X / 2.0f, entSize.Y / 2.0f))).Length() <= radius)
                    {
                        result.Add(ent);
                    }
                }

                // TODO Remove debug code
                if (result.Count - resultLength > 1)
                    throw new Exception("Collision code is sucky!");
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


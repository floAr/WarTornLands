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

                result.Add(ent);

                //if (position.X >= pos.X - size.X * 0.5f && position.X < pos.X + size.X * 0.5f &&
                //    position.Y >= pos.Y - size.Y * 0.5f && position.Y < pos.Y + size.Y * 0.5f)
                //{
                //    result.Add(ent);
                //}
            }

            return result;
        }

        public List<Entity> GetEntitiesAt(Vector2 position, float radius)
        {
            List<Entity> result = new List<Entity>();

            foreach (Entity ent in _entities)
            {
                Vector2 pos = ent.Position;
                Vector2 size = ent.Size;

                // Check whether circles center lies inside entity rectangle
                if (position.X >= pos.X - size.X * 0.5f && position.X < pos.X + size.X * 0.5f &&
                    position.Y >= pos.Y - size.Y * 0.5f && position.Y < pos.Y + size.Y * 0.5f)
                {
                    result.Add(ent);
                }
                else
                {
                    Vector2 A = ent.Position;
                    Vector2 B = ent.Position; B.X += size.X;
                    Vector2 C = ent.Position + ent.Size;
                    Vector2 D = ent.Position; D.Y += size.Y;

                    Vector2 AB = B - A;
                    Vector2 BC = C - B;
                    Vector2 CD = D - C;
                    Vector2 DA = A - D;
                    
                    // TODO get circle line intersections
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


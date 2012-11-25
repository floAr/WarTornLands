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
            Game1 game = (_game as Game1);

            game.SpriteBatch.Begin();

            // TODO:
            // Check whether Entities are visible and just draw them if they are
            foreach (Entity ent in _entities)
            {
                ent.Draw(gameTime);
            }

            game.SpriteBatch.End();
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLandsRefurbished.Entities;
using Microsoft.Xna.Framework;
using WarTornLands.Entities;

namespace WarTornLands.World.Layers
{
    class DynamicsLayer : Layer
    {
        private Game _game;
        private readonly List<Entity> _dynamics = new List<Entity>();

        public DynamicsLayer(Game game)
        {
            _game = game;
        }

        public void AddEntity(Entity entity)
        {
            _dynamics.Add(entity);
        }

        public void AddRange(List<Entity> entities)
        {
            _dynamics.AddRange(entities);
        }

        public Entity GetEntityAt(Vector2 worldPosition)
        {
            foreach (Entity ent in _dynamics)
            {
                Vector2 pos = ent.Position;
                Vector2 size = ent.Size;

                if (worldPosition.X >= pos.X - size.X * 0.5f && worldPosition.X < pos.X + size.X * 0.5f &&
                    worldPosition.Y >= pos.Y - size.Y * 0.5f && worldPosition.Y < pos.Y + size.Y * 0.5f)
                {
                    return ent;
                }
            }

            return null;
        }

        public void LoadContent()
        {
            foreach (Entity e in _dynamics)
            {
                e.LoadContent((_game as Game1).Content);
            }
        }

        public override void Update(GameTime gameTime)
        {
            // TODO:
            // Check whether Entities are in the proximity of the Player and just update them if they are
            // Could also be done in the Entities class itself
            try
            {
                foreach (Entity ent in _dynamics)
                {
                    ent.Update(gameTime);
                    if (ent.Health == 0)
                    {
                        ent.OnDie();
                        _dynamics.Remove(ent);
                        GC.Collect();
                    }
                }
            }
            catch { }
        }

        public void Draw(GameTime gameTime)
        {
            Game1 game = (_game as Game1);

            game.SpriteBatch.Begin();

            // TODO:
            // Check whether Entities are visible and just draw them if they are
            foreach (Entity ent in _dynamics)
            {
                ent.Draw(gameTime);
            }

            game.SpriteBatch.End();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarTornLands.Infrastructure;

namespace WarTornLands.Entities.Implementations
{
    class EntityTree : Entity
    {
        public new event EventHandler<DieEventArgs> Die;

        public EntityTree(Game game, Vector2 position, Texture2D texture)
            : base(game, position, texture)
        {
            Health = 300;
            CanBeAttacked = true;
        }

        public override void OnDie()
        {
            // TODO drop wood
            if(Die != null)
                Die(this, new DieEventArgs(Game, Position, TextureCatalog.DeadTree));

            base.OnDie();
        }
    }
}

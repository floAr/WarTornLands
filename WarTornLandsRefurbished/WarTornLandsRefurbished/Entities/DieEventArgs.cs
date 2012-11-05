using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Entities.Implementations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WarTornLands.Entities
{
    class DieEventArgs
    {
        public DyingEntity Corpse { get; private set; }

        public DieEventArgs(Game game, Vector2 position, Texture2D texture, string name = "Entity")
        {
            Corpse = new DyingEntity(game, position, texture, name);
        }
    }
}

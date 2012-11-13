using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WarTornLands.Entities.Modules.Die
{
    class DyingEntity : WarTornLands.Entities.Entity
    {
        public DyingEntity(Game game, Vector2 position, Texture2D texture, string name)
            : base(game, position, texture, name)
        {
        }
    }
}

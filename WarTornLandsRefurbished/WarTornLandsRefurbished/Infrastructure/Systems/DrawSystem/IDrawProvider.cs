using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WarTornLands.Infrastructure.Systems.DrawSystem
{
   public interface IDrawProvider
    {
        void Draw(GameTime gameTime);
    }
}

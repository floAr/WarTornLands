using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WarTornLands.World.Layers
{
    interface Layer
    {
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }
}

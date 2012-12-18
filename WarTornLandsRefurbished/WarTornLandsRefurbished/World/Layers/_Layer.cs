using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WarTornLands.Entities;

namespace WarTornLands.World.Layers
{
    public abstract class Layer
    {
        public Layer()           
        {
           
        }


     public abstract void Update(GameTime gameTime);
     public  abstract void Draw(GameTime gameTime);
    }
}

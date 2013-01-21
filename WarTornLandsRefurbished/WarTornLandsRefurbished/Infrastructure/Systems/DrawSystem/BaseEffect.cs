using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WarTornLands.Infrastructure.Systems.DrawSystem
{
   public abstract class BaseEffect
    {
       protected Effect _effect;
       public bool Finished=false;

       public virtual void Apply()
       {
           _effect.Techniques[0].Passes[0].Apply();
       }
       public abstract void Update(GameTime gameTime);
    }
}

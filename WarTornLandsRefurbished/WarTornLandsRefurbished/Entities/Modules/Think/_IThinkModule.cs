using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WarTornLands.Entities.AI;

namespace WarTornLands.Entities.Modules.Think
{
    public interface IThinkModule
    {
        void Update(GameTime gameTime);
        void SetZone(Zone zone);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarTornLands.Entities.Modules.Collide
{
    class Obstacle:BaseModule, ICollideModule
    {
        public bool OnCollide(CollideInformation info)
        {
            return false;
        }
    }
}

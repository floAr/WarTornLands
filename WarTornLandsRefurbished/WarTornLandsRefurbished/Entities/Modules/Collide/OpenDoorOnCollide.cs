using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarTornLands.Entities.Modules.Collide
{
    class OpenDoorOnCollide : BaseModule, ICollideModule
    {
        public bool OnCollide(CollideInformation info)
        {
            // TODO open only if player has key!
            _owner.IsDead = true;
            return true;
        }
    }
}

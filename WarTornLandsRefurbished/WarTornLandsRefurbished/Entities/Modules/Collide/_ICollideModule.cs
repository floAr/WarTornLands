using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarTornLands.Entities.Modules.Collide
{
    public struct CollideInformation
    {
        public Entity Collider { get; set; }
        public bool IsPlayer { get; set; }

    }
    public interface ICollideModule
    {

        void OnCollide(CollideInformation info);
    }
}

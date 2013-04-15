using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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
         bool IsPassable(CollideInformation info);
         Rectangle BodyShape { get; set; }
         Rectangle MovingShape { get; set; }
    }
}

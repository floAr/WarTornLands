using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WarTornLands.Entities.Modules.Collide
{
    class Obstacle:BaseModule, ICollideModule
    {
        public bool OnCollide(CollideInformation info)
        {
            return false;
        }

        public Obstacle()
        { }

        public Obstacle(DataRow data)
            : this()
        { }

        public Microsoft.Xna.Framework.Rectangle BodyShape
        {
            get;
            set;
        }

        public Microsoft.Xna.Framework.Rectangle MovingShape
        {
            get;
            set;
        }
    }
}

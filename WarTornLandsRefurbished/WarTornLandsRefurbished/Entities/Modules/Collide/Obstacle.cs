using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WarTornLands.Entities.Modules.Collide
{
    class Obstacle : BaseModule, ICollideModule
    {
        public void OnCollide(CollideInformation info)
        {
            // Do nothing
        }

        public bool IsPassable(CollideInformation info)
        {
            // Source check
            if (info.Collider == _owner)
                return true;

            // Altitude / height check
            if (info.Collider.Altitude + info.Collider.BodyHeight < _owner.Altitude ||
                info.Collider.Altitude > _owner.Altitude + _owner.BodyHeight)
                return true;
            
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

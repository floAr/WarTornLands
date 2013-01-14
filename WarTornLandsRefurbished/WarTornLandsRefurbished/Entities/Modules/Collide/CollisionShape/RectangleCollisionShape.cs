using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WarTornLands.Entities.Modules.Collide.CollisionShape
{
    public class RectangleCollisionShape:ICollisionShape
    {
        private Rectangle _shape;
        public Rectangle Shape { get { return _shape; } set { _shape = value; } }
        public RectangleCollisionShape(Rectangle shape)
        {
            _shape = shape;
        }
    }
}

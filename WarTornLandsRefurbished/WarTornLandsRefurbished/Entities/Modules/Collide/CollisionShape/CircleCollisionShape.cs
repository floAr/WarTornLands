using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WarTornLands.Entities.Modules.Collide.CollisionShape
{
    public class CircleCollisionShape:ICollisionShape
    {
        private Vector2 _center;
        private float _radius;

        public Vector2 Center { get { return _center; } set { _center = value; } }
        public float Radius { get { return _radius; } set { _radius = value; } }

        public CircleCollisionShape(Vector2 center, float radius)
        {
            _center = center;
            _radius = radius;
        }
    }
}

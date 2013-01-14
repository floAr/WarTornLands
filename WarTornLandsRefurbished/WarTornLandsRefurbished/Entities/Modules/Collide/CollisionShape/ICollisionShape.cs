using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WarTornLands.Entities.Modules.Collide.CollisionShape
{
    public interface ICollisionShape
    {
    }

    public class ShapeCollider
    {
        public static bool Collide(ICollisionShape s1, ICollisionShape s2)
        {
            if (s1 is CompoundCollisionShape)
            {
                if (s2 is CompoundCollisionShape)
                    return Collide(s1 as CompoundCollisionShape, s2 as CompoundCollisionShape);
                if (s2 is RectangleCollisionShape)
                    return Collide(s2 as RectangleCollisionShape, s1 as CompoundCollisionShape);
                if (s2 is CircleCollisionShape)
                    return Collide(s2 as CircleCollisionShape, s1 as CompoundCollisionShape);
            }
            if (s1 is RectangleCollisionShape)
            {
                if (s2 is CompoundCollisionShape)
                    return Collide(s1 as RectangleCollisionShape, s2 as CompoundCollisionShape);
                if (s2 is RectangleCollisionShape)
                    return Collide(s1 as RectangleCollisionShape, s2 as RectangleCollisionShape);
                if (s2 is CircleCollisionShape)
                    return Collide(s1 as RectangleCollisionShape, s2 as CircleCollisionShape);
            }
            if (s1 is CircleCollisionShape)
            {
                if (s2 is CompoundCollisionShape)
                    return Collide(s1 as CircleCollisionShape, s2 as CompoundCollisionShape);
                if (s2 is RectangleCollisionShape)
                    return Collide(s2 as RectangleCollisionShape, s1 as CircleCollisionShape);
                if (s2 is CircleCollisionShape)
                    return Collide(s1 as CircleCollisionShape, s2 as CircleCollisionShape);
            }
            throw new Exception("Collision Shape unknown");

        }
        public static bool Collide(RectangleCollisionShape s1, RectangleCollisionShape s2)
        {
            return s1.Shape.Intersects(s2.Shape);
        }
        public static bool Collide(RectangleCollisionShape s1, CircleCollisionShape s2)
        {

            float circleDistanceX = Math.Abs(s2.Center.X - s1.Shape.X);
            float circleDistanceY = Math.Abs(s2.Center.Y - s1.Shape.Y);

            if (circleDistanceX > (s1.Shape.Width / 2 + s2.Radius)) { return false; }
            if (circleDistanceY > (s1.Shape.Height / 2 + s2.Radius)) { return false; }

            if (circleDistanceX <= (s1.Shape.Width / 2)) { return true; }
            if (circleDistanceY <= (s1.Shape.Height / 2)) { return true; }

            double cornerDistance_sq = Math.Pow((circleDistanceX - s1.Shape.Width / 2), 2) +
                                 Math.Pow((circleDistanceY - s1.Shape.Height / 2), 2);

            return cornerDistance_sq < Math.Pow(s2.Radius, 2);
        }
        public static bool Collide(CircleCollisionShape s1, CircleCollisionShape s2)
        {
            return Vector2.Distance(s1.Center, s2.Center) > s1.Radius + s2.Radius;
        }

        public static bool Collide(CompoundCollisionShape s1, CompoundCollisionShape s2)
        {
            foreach (ICollisionShape c1 in s1.Shapes)
            {
                foreach (ICollisionShape c2 in s2.Shapes)
                {
                    if (Collide(c1, c2))
                        return true;
                }
            }
            return false;
        }

        public static bool Collide(RectangleCollisionShape s1, CompoundCollisionShape s2)
        {
            foreach (ICollisionShape c2 in s2.Shapes)
            {
                if (Collide(s1, c2))
                    return true;
            }
            return false;
        }
        public static bool Collide(CircleCollisionShape s1, CompoundCollisionShape s2)
        {
            foreach (ICollisionShape c2 in s2.Shapes)
            {
                if (Collide(s1, c2))
                    return true;
            }
            return false;
        }
    }
}

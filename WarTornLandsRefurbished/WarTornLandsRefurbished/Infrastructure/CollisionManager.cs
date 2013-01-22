using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WarTornLands.Entities;
using WarTornLands.World;
using WarTornLands.Entities.Modules.Collide;
using WarTornLands.PlayerClasses;
using WarTornLands.Entities.Modules.Collide.CollisionShape;

namespace WarTornLands.Infrastructure
{
    public class CollisionManager
    {


        #region Singleton Stuff
        private static CollisionManager _instance;

        public static CollisionManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CollisionManager();
                }
                return _instance;
            }
        }
        #endregion

        private CollisionManager()
        {
        }

        /// <summary>
        /// Tries to move an entity by a given move vector. Calls Entity.Collide functions of both
        /// the source and the entities it collides with along the way. The resulting move vector needs
        /// to be applied manually by the calling function!
        /// </summary>
        /// <param name="source">Entity that is to be moved.</param>
        /// <param name="moveVector">Vector by which the entity is to be moved.</param>
        /// <returns>A possibly corrected move vector along which it is safe to move the entity.</returns>
        public Vector2 TryMove(Entity source, Vector2 moveVector, bool slide = true)
        {
            // Don't do anything for a Vector2.Zero move vector
            if (moveVector.LengthSquared() == 0)
                return moveVector;

            // Save squared length of move vector
            float distance = moveVector.Length();

            // List of entities that the source collides with
            HashSet<Entity> entities = new HashSet<Entity>();

            // Calculate step vector for continuous collision detection
            Vector2 step = moveVector / moveVector.Length();

            // Loop for stepwise collision detection
            Vector2 pos = new Vector2();
            Vector2 newPos = new Vector2();
            bool blocked = false;
            while (!blocked && distance > 0)
            {
                // Step the position
                if (distance >= 1)
                {
                    // Do a full step of length 1
                    newPos += step;
                    distance--;
                }
                else
                {
                    // Do a partial step for the remaining length
                    newPos += step * distance;
                    distance = 0;
                }
                
                // Check whether the rectangle at newPos collides
                Rectangle rect = source.MovingRect;
                if (!IsAccessible(OffsetRectangle(rect, newPos), true, source))
                {
                    // Add entities to collision list
                    entities.UnionWith(Game1.Instance.Level.GetEntitiesAt(OffsetRectangle(rect, newPos)));

                    if (slide)
                    {
                        // Get possibly capped vector for this step
                        Vector2 thisStep = newPos - pos;

                        // Check in X direction
                        rect = source.MovingRect;
                        Vector2 newPosSlide = pos + new Vector2(thisStep.X, 0);
                        if (IsAccessible(OffsetRectangle(rect, newPosSlide), true, source))
                        {
                            // X successful
                            entities.Union(Game1.Instance.Level.GetEntitiesAt(rect));
                            newPos = newPosSlide;
                        }
                        else
                        {
                            // Check in Y direction
                            rect = source.MovingRect;
                            newPosSlide = pos + new Vector2(0, thisStep.Y);
                            if (IsAccessible(OffsetRectangle(rect, newPosSlide), true, source))
                            {
                                // Y successful
                                newPos = newPosSlide;
                            }
                            else
                            {
                                // Both X and Y blocked
                                blocked = true;
                            }
                        }
                    }
                    else
                    {
                        // Path is blocked
                        blocked = true;
                    }
                }
                
                // Update position if check was successful
                if (!blocked)
                {
                    pos = newPos;
                }
            }
            
            // Remove source so it doesn't collide with itself
            entities.Remove(source);

            // Call Collide methods
            foreach (Entity ent in entities)
            {
                ent.Collide(source);
                source.Collide(ent);
            }

            return pos;
        }

        /// <summary>
        /// Offset a rectangle by a given vector using the ceiling of the absolute vector values.
        /// The original rectangle is not modified, since the rect is passed by value.
        /// </summary>
        /// <param name="rect">Rectangle to be modified.</param>
        /// <param name="offset">Offset vector, absolute value rounded up.</param>
        /// <returns>Offset rectangle.</returns>
        public Rectangle OffsetRectangle(Rectangle rect, Vector2 offset)
        {
            rect.Offset(offset.X == 0 ? 0 : (offset.X > 0 ? (int)offset.X + 1 : (int)offset.X - 1),
                offset.Y == 0 ? 0 : (offset.Y > 0 ? (int)offset.Y + 1 : (int)offset.Y - 1));
            return rect;
        }

        /// <summary>
        /// Check whether a rectangle is accessible or not.
        /// </summary>
        /// <param name="rect">Rectangle to be tested.</param>
        /// <param name="testTiles">Whether tile collision should be tested.</param>
        /// <param name="source">Entity that's colliding, e.g. if some thing's collide only with the player.</param>
        /// <returns>True if rectangle is accesible, false otherwise.</returns>
        public bool IsAccessible(Rectangle rect, bool testTiles, Entity source = null)
        {
            foreach (Entity e in Game1.Instance.Level.GetEntitiesAt(rect))
            {
                // Break if entity blocks way
                if (e.CollideModule != null &&
                    !e.CollideModule.IsPassable(new CollideInformation()
                    {
                        Collider = source,
                        IsPlayer = source is Player
                    }))
                {
                    return false;
                }
            }

            // Return result of tile collision or true if we don't want to check
            return testTiles? Game1.Instance.Level.IsRectAccessible(rect) : true;
        }

        // TODO IsAccessible for point+radius, maybe lines and stuff...

        #region COLLISION SHAPE CODE
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

        #region DEEP COLLISION SHAPE CODE

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

        #endregion
        #endregion
    }
}

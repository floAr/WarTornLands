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


        public Vector2 TryMove(Entity source, Vector2 moveVector)
        {
            if (source.DrawModule == null)
                return source.Position + moveVector;

            List<Entity> entities;
            Vector2 move = CollideRectangle(source, moveVector, true, true, out entities);

            // Call Collide methods
            foreach (Entity ent in entities)
            {
                ent.Collide(source);
                source.Collide(ent);
            }

            return move;
        }
        /// <summary>
        /// Test an entity collision at a point.
        /// </summary>
        /// <param name="point">Position of the point.</param>
        /// <param name="radius">Radius of the check.</param>
        /// <returns>List of entities at the point.</returns>
        /// TODO altitude & height check!
        /// TODO add tile check!
        public List<Entity> CollidePoint(Vector2 point, int radius = 1)
        {
            return Game1.Instance.Level.GetEntitiesAt(point, radius);
        }

        /// <summary>
        /// Test a collision on a line. Does not call any Collide events.
        /// </summary>
        /// <param name="start">Start point.</param>
        /// <param name="end">Absolute end point.</param>
        /// <param name="source">The entity executing the move.</param>
        /// <param name="testTiles">Set true if tile collision should be checked.</param>
        /// <param name="altitude">Altitude of the colliding body.</param>
        /// <param name="bodyHeight">Height of the colliding body.</param>
        /// <param name="list">Out param for a list of entities, sorted by distance.</param>
        /// <returns>Absolute end position of the collision.</returns>
        public Vector2 CollideLine(Vector2 start, Vector2 end, Entity source, bool testTiles,
            float altitude, float bodyHeight, out List<Entity> list)
        {
            // TODO add tile check!

            list = new List<Entity>();

            float distance = Vector2.Distance(start, end);
            Vector2 pos = start;
            Vector2 step = new Vector2((end.X - start.X) / distance, (end.Y - start.Y) / distance);

            for (int i = 0; Math.Abs(i) < distance; i++)
            {
                foreach (Entity e in CollidePoint(pos))
                {
                    // Add entity, if it's not in list
                    if (!list.Contains(e))
                        list.Add(e);

                    // Break if entity blocks way
                    if (e.CollideModule != null &&
                        !e.CollideModule.IsPassable(new CollideInformation()
                        {
                            Collider = source,
                            IsPlayer = source is Player
                        }))
                    {
                        return pos;
                    }
                }

                pos += step;
            }

            return pos;
        }

        /// <summary>
        /// Test an entity collision on a moving rectangle. Does not call any Collide events.
        /// </summary>
        /// <param name="source">The entity executing the move.</param>
        /// <param name="toEnd">Movement vector to the end position.</param>
        /// <param name="testTile">Set true if tile collision should be checked.</param>
        /// <param name="slide">Set true if you want to slide on walls.</param>
        /// <param name="list">Out param for a list of entities, sorted by distance.</param>
        /// <returns>Relative end position of the collision.</returns>
        public Vector2 CollideRectangle(Entity source, Vector2 toEnd, bool testTile, bool slide, out List<Entity> list)
        {
            list = new List<Entity>();

            float distance = toEnd.Length();
            Vector2 pos = new Vector2();
            Vector2 step = toEnd / distance;

            for (int i = 0; Math.Abs(i) < distance; i++)
            {
                // Pre-calculate next step's rectangle
                Rectangle newRect = source.BoundingRect;
                newRect.Offset((int)Math.Round(pos.X + step.X), (int)Math.Round(pos.Y + step.Y));

                // Test next step's rectangle
                foreach (Entity e in Game1.Instance.Level.GetEntitiesAt(newRect))
                {
                    // Add entity, if it's not in list
                    if (!list.Contains(e))
                        list.Add(e);

                    // Break if entity blocks way
                    if (e.CollideModule != null &&
                        !e.CollideModule.IsPassable(new CollideInformation()
                        {
                            Collider = source,
                            IsPlayer = source is Player
                        }))
                    {
                        // Return this step's position
                        return pos;
                    }
                }

                // Break if tiles block way
                if (testTile && !Game1.Instance.Level.IsRectAccessible(newRect))
                {
                    // Collide'n'slide, yeah!
                    if (slide)
                    {
                        // Try only X
                        newRect = source.BoundingRect;
                        newRect.Offset((int)Math.Round(pos.X + step.X), (int)Math.Round(pos.Y));
                        if (Game1.Instance.Level.IsRectAccessible(newRect))
                        {
                            pos.X += step.X;
                        }
                        else
                        {
                            newRect = source.BoundingRect;
                            newRect.Offset((int)Math.Round(pos.X), (int)Math.Round(pos.Y + step.Y));
                            if (Game1.Instance.Level.IsRectAccessible(newRect))
                            {
                                pos.Y += step.Y;
                            }
                        }
                    }

                    // Return this step's position
                    return pos;
                }

                // Step current position and move rect
                pos += step;
            }

            return pos;
        }
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

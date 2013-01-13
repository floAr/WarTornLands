using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WarTornLands.Entities;
using WarTornLands.World;
using WarTornLands.Entities.Modules.Collide;
using WarTornLands.PlayerClasses;

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
        /// Tries to execute a move from 'start' 'toGoal'.
        /// ToGoal is relative to the start position.
        /// Returns the actually possible move vector in respect to collisions
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="toGoal">To goal.</param>
        /// <param name="radius">The radius.</param>
        /// <param name="source">The source.</param>
        /// <param name="altitude">Altitude of the colliding body.</param>
        /// <param name="bodyHeight">Height of the colliding body.</param>
        /// <returns></returns>
        /*public Vector2 TryMove(Vector2 start, Vector2 toGoal, float radius, Entity source,
            float altitude, float bodyHeight)
        {
            float range = 20;  // TODO Get Entity radius from whereever

            Vector2 move = toGoal;

            Vector2 topRight = new Vector2(1f, -.5f) * range;
            Vector2 topLeft = new Vector2(-1f, -.5f) * range;
            Vector2 bottomRight = new Vector2(1f, 2.1f) * range;
            Vector2 bottomLeft = new Vector2(-1f, 2.1f) * range;

            List<Vector2> checkpoints = new List<Vector2>() { topRight, bottomRight, bottomLeft, topLeft };

            if (toGoal.Y < 0)
                move.Y = ComputeMoveDirection(start, toGoal, topLeft, topRight, false, source, altitude, bodyHeight);
            if (toGoal.Y > 0)
                move.Y = ComputeMoveDirection(start, toGoal, bottomLeft, bottomRight, false, source, altitude, bodyHeight);

            if (toGoal.X < 0)
                move.X = ComputeMoveDirection(start, toGoal, topLeft, bottomLeft, true, source, altitude, bodyHeight);
            if (toGoal.X > 0)
                move.X = ComputeMoveDirection(start, toGoal, topRight, bottomRight, true, source, altitude, bodyHeight);

            return move;
        }*/

        /// <summary>
        /// Computes the move direction.
        /// </summary>
        /// <param name="start">The start position of the move.</param>
        /// <param name="toGoal">The goal position relative to the start.</param>
        /// <param name="pointOne">The collision test point one.</param>
        /// <param name="pointTwo">The collision test point two.</param>
        /// <param name="xDir">If set to <c>true</c> X direction is computed. If false Y direction.</param>
        /// <param name="source">The entity executing the move.</param>
        /// <param name="altitude">Altitude of the colliding body.</param>
        /// <param name="bodyHeight">Height of the colliding body.</param>
        /// <returns>Returns the X or Y value (see xDir) of the position the move ends at (relative to the start) in respect to collisions with Tiles and entities.</returns>
        /*private float ComputeMoveDirection(Vector2 start, Vector2 toGoal, Vector2 pointOne, Vector2 pointTwo, bool xDir, Entity source,
            float altitude, float bodyHeight)
        {
            Vector2 move = toGoal;
            if (xDir)
                move.Y = 0;
            else
                move.X = 0;

            Vector2 direction = Vector2.Normalize(toGoal);
            float sensor = (float)toGoal.Length();


            // Tile collision
            while ((!Game1.Instance.Level.IsPositionAccessible(start + pointOne + move) ||
                   !Game1.Instance.Level.IsPositionAccessible(start + pointTwo + move)) &&
                    sensor > 0)
            {
                move = direction * sensor;
                if (xDir)
                    move.Y = 0;
                else
                    move.X = 0;
                if (sensor > 1)
                    sensor--;
                else
                    return 0;
            }

            // Entity collision
            List<Entity> lastEntities = new List<Entity>();
            List<Entity> curEntities = Game1.Instance.Level.GetEntitiesAt(start + pointOne + move);
            curEntities.AddRange(Game1.Instance.Level.GetEntitiesAt(start + pointTwo + move));

            List<Entity> temp = new List<Entity>(curEntities);
            foreach (Entity ent in temp)
            {
                if (ent.CollideModule == null || ent.Equals(source))
                    curEntities.Remove(ent);
            }

            while (curEntities.Count > 0 && sensor > 0)
            {
                lastEntities.Clear();
                lastEntities.AddRange(curEntities);
                if (sensor >= 1)
                    sensor--;
                else
                    sensor = 0;
                move = direction * sensor;
                curEntities = Game1.Instance.Level.GetEntitiesAt(start + pointOne + move);
                curEntities.AddRange(Game1.Instance.Level.GetEntitiesAt(start + pointTwo + move));

                temp = new List<Entity>(curEntities);
                foreach (Entity ent in temp)
                {
                    if (ent.CollideModule == null || ent.Equals(source))
                        curEntities.Remove(ent);
                }
            }

            // Get list of colliding entities
            // TODO not use center, but points and use union of entity lists
            List<Entity> entities1, entities2;
            Vector2 move1 = CollideLine(start + pointOne, start + pointOne + toGoal, source, true, altitude, bodyHeight, out entities1) - (start + pointOne);
            Vector2 move2 = CollideLine(start + pointTwo, start + pointTwo + toGoal, source, true, altitude, bodyHeight, out entities2) - (start + pointTwo);

            List<Entity> entities = entities1.Union<Entity>(entities2).ToList();

            foreach (Entity ent in entities)
            {
                ent.Collide(source);
                source.Collide(ent);
            }

            if (xDir)
                return Math.Min(move1.X, move2.X);
            else
                return Math.Min(move1.Y, move2.Y);
        }*/

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
                        !e.CollideModule.IsPassable(new CollideInformation() { Collider = source,
                        IsPlayer = source is Player }))
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
                        !e.CollideModule.IsPassable(new CollideInformation() { Collider = source,
                        IsPlayer = source is Player }))
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
    }
}

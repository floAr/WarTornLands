using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WarTornLands.Entities;
using WarTornLands.World;

namespace WarTornLands.Infrastructure
{
    public class CollisionManager
    {
        private Level _level;

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
            _level = Game1.Instance.Level;
        }

        public Vector2 TryMove(Vector2 start, Vector2 toGoal, float radius, Entity source)
        {
            float range = 20;  //Get Entity radius from whereever

            Vector2 move = toGoal;

            Vector2 topRight = new Vector2(1f, -.5f) * range;
            Vector2 topLeft = new Vector2(-1f, -.5f) * range;
            Vector2 bottomRight = new Vector2(1f, 2.1f) * range;
            Vector2 bottomLeft = new Vector2(-1f, 2.1f) * range;

            List<Vector2> checkpoints = new List<Vector2>() { topRight, bottomRight, bottomLeft, topLeft };

            if (toGoal.Y < 0)
                move.Y = ComputeMoveDirection(start, toGoal, topLeft, topRight, false, source);
            if (toGoal.Y > 0)
                move.Y = ComputeMoveDirection(start, toGoal, bottomLeft, bottomRight, false, source);

            if (toGoal.X < 0)
                move.X = ComputeMoveDirection(start, toGoal, topLeft, bottomLeft, true, source);
            if (toGoal.X > 0)
                move.X = ComputeMoveDirection(start, toGoal, topRight, bottomRight, true, source);

            return move + start;
        }

        /// <summary>
        /// Computes the move direction.
        /// </summary>
        /// <param name="start">The start position of the move.</param>
        /// <param name="toGoal">The goal position relative to the start.</param>
        /// <param name="pointOne">The collision test point one.</param>
        /// <param name="pointTwo">The collision test point two.</param>
        /// <param name="xDir">If set to <c>true</c> X direction is computed. If false Y direction.</param>
        /// <param name="source">The entity executing the move.</param>
        /// <returns>Returns the X or Y value (see xDir) of the position the move ends at (relative to the start) in respect to collisions with Tiles and entities.</returns>
        private float ComputeMoveDirection(Vector2 start, Vector2 toGoal, Vector2 pointOne, Vector2 pointTwo, bool xDir, Entity source)
        {
            Vector2 move = toGoal;
            if (xDir)
                move.Y = 0;
            else
                move.X = 0;

            Vector2 direction = Vector2.Normalize(toGoal);
            int sensor = (int)toGoal.Length();

            // Tile collision
            while (!_level.IsPositionAccessible(start + pointOne + move) ||
                   !_level.IsPositionAccessible(start + pointTwo + move) ||
                    sensor == 1)
            {
                sensor--;
                move = direction * sensor;
                if (xDir)
                    move.Y = 0;
                else
                    move.X = 0;
            }

            // Entity collision
            List<Entity> lastEntities = new List<Entity>();
            List<Entity> curEntities = _level.GetEntitiesAt(start + pointOne + move);
            curEntities.AddRange(_level.GetEntitiesAt(start + pointTwo + move));

            while (curEntities.Count > 0 &&
                   sensor >= 1)
            {
                //lastEntities = new List<Entity>(curEntities);
                lastEntities.Clear();
                lastEntities.AddRange(curEntities);
                sensor--;
                move = direction * sensor;
                curEntities = _level.GetEntitiesAt(start + pointOne + move);
                curEntities.AddRange(_level.GetEntitiesAt(start + pointTwo + move));
            }

            foreach (Entity ent in lastEntities)
            {
                ent.Collide(source);
                source.Collide(ent);
            }

            if (xDir)
                return move.X;
            else
                return move.Y;
        }
    }
}

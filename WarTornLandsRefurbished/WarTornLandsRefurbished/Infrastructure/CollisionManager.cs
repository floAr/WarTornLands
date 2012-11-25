using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WarTornLands.Entities;

namespace WarTornLands.Infrastructure
{
    public class CollisionManager
    {
        #region Singleton Stuff
        private static CollisionManager instance;

        public static CollisionManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CollisionManager();
                }
                return instance;
            }
        }
        #endregion

        private CollisionManager()
        {
        }

        public  Vector2 TryMove(Vector2 start, Vector2 toGoal, float radius, Entity source)
        {
            float range = 14;  //Get Entity radius from whereever

            Vector2 move = toGoal;

            Vector2 topRight = new Vector2(1, -1) * range;
            Vector2 topLeft = new Vector2(-1, -1) * range;
            Vector2 bottomRight = new Vector2(1, 1) * range;
            Vector2 bottomLeft = new Vector2(-1, 1) * range;

            List<Vector2> checkpoints = new List<Vector2>() { topRight, bottomRight, bottomLeft, topLeft };

            if (toGoal.Y < 0 &&
                (!Level.IsPixelAccessible(start + topRight + toGoal, source) ||
                    !Level.IsPixelAccessible(start + topLeft + toGoal, source)))
                move.Y = 0;
            if (toGoal.Y > 0 &&
                (!Level.IsPixelAccessible(start + bottomRight + toGoal, source) ||
                    !Level.IsPixelAccessible(start + bottomLeft + toGoal)))
                move.Y = 0;

            if (toGoal.X < 0 &&
                (!Level.IsPixelAccessible(start + topLeft + toGoal, source) ||
                    !Level.IsPixelAccessible(start + bottomLeft + toGoal, source)))
                move.X = 0;
            if (toGoal.X > 0 &&
                (!Level.IsPixelAccessible(start + topRight + toGoal, source) ||
                    !Level.IsPixelAccessible(start + bottomRight + toGoal, source)))
                move.X = 0;

            return move + start;
        }

        private float ComputeMoveDirection(Vector2 start, Vector2 toGoal, Vector2 pointOne, Vector2 pointTwo, Entity source)
        {
            Vector2 move = toGoal;
            Vector2 direction = Vector2.Normalize(toGoal);
            int sensor = (int)toGoal.Length();

            // Tile collision
            while (!Level.IsPixelAccessible(start + pointOne + move) ||
                   !Level.IsPixelAccessible(start + pointTwo + move) ||
                    sensor == 0)
            {
                sensor--;
                move = direction * sensor;
            }

            //// Entity collision
            //List<Entity> lastEntities = new List<Entity>;
            //List<Entity> curEntities;

            //while (!(curEntities = Level.GetEntitiesAt(start + pointOne + move)).Count > 0 ||
            //       !(curEntities.AddRange(Level.GetEntitiesAt(start + pointOne + move))).Count ||
            //        sensor == 0)
            //{
            //    sensor--;
            //    move = direction * sensor;
            //}

            return 0;
        }
    }
}

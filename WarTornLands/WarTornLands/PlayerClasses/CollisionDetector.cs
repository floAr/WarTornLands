using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WarTornLands.PlayerClasses
{
    static class CollisionDetector
    {
        private static Level _level;

        public static void Setup(Level level)
        {
            _level = level;
        }

        public static Vector2 GetPosition(Vector2 start, Vector2 toGoal)
        {
            float range = toGoal.Length();
            toGoal.Normalize();

            if(range == 0)
            {
                return start;
            }

            Vector2 curGoal = start + toGoal * range;

            while(!_level.IsPixelAccessible((curGoal)))
            {
                if (range > 1)
                {
                    range -= 1;
                }
                else
                {
                    return start;
                }

                curGoal = start + toGoal * range;
            }

            return curGoal;
        }

    }
}

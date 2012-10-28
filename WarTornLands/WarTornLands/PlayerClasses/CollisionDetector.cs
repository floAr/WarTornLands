using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WarTornLands.EntityClasses;

namespace WarTornLands.PlayerClasses
{
    static class CollisionDetector
    {
        private static Level _level;

        public static void Setup(Level level)
        {
            _level = level;
        }

        public static Vector2 GetPosition(Vector2 start, Vector2 toGoal, float radius, Entity source)
        {
            float range = toGoal.Length();
            toGoal.Normalize();

            if(range == 0)
            {
                return start;
            }

            #region Standard method

            Vector2 curGoal = start + toGoal * (range + radius);

            float standardRange = range;

            if (_level.IsPixelAccessible((curGoal)))
            {
                return curGoal - toGoal * radius;
            }
            else
            {
                if (source != null)
                {
                    Entity ent = _level.GetEntityAt(curGoal);
                    ent.OnCollide(source);
                    if (ent.CanBePickedUp())
                    {
                        return curGoal - toGoal * radius;
                    }
                }
            }

            while (!_level.IsPixelAccessible((curGoal)))
            {
                if (standardRange > 1)
                {
                    standardRange -= 1;
                }
                else
                {
                    break;
                }

                curGoal = start + toGoal * (standardRange + radius);

                if (_level.IsPixelAccessible((curGoal)))
                {
                    return curGoal - toGoal * radius;
                }
                else
                {
                    if (source != null)
                    {
                        Entity ent = _level.GetEntityAt(curGoal);
                        ent.OnCollide(source);
                        if (ent.CanBePickedUp())
                        {
                            return curGoal - toGoal * radius;
                        }
                    }
                }
            }

            #endregion


            #region "Slide" method

            if (Math.Abs(toGoal.X) > Math.Abs(toGoal.Y))
            {
                toGoal = new Vector2(toGoal.X, 0);
            }
            else
            {
                toGoal = new Vector2(0, toGoal.Y);
            }

            curGoal = start + toGoal * (range + radius);

            while (!_level.IsPixelAccessible((curGoal)))
            {
                if (standardRange > 1)
                {
                    standardRange -= 1;
                }
                else
                {
                    return start;
                }

                curGoal = start + toGoal * (standardRange + radius);
            }

            #endregion

            return curGoal - toGoal * radius;
        }

    }
}

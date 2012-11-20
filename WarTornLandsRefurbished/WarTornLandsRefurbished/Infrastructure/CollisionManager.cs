using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WarTornLands.Entities;

namespace WarTornLands.Infrastructure
{
    public static class CollisionManager
    {
        public static Vector2 TryMove(Vector2 start, Vector2 toGoal, float radius, Entity source)
        {
            return start + toGoal;
        }
    }
}

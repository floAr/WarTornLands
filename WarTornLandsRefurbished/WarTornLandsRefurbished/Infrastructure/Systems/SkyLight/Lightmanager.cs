using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Entities.Modules.Draw;
using Microsoft.Xna.Framework;

namespace WarTornLands.Infrastructure.Systems.SkyLight
{
    public static class Lightmanager
    {
        private static List<Entities.Entity> _lights = new List<Entities.Entity>();
        public static void AddLight(Entities.Entity light)
        {
            _lights.Add(light);
        }

        public static void Draw(GameTime gameTime)
        {
            foreach (Entities.Entity e in _lights)
            {
                e.Draw(gameTime);
            }
        }
    }
}

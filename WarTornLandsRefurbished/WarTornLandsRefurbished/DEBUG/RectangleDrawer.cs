using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Entities.Modules.Draw;
using WarTornLands.Entities.Modules;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WarTornLands.DEBUG
{
    public static class RectangleDrawer
    {
        static List<Entities.Entity> _target = new List<Entities.Entity>();
        static Texture2D _pixel = new Texture2D(Game1.Instance.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
        static Color _color = Color.White;
        static public Color CurrentColor { get { return _color; } set { _color = value; } }
        static bool set = false;

        public static void BindEntity(Entities.Entity entitiy)
        {
            _target.Add(entitiy);
        }
        public static void DetachEntity(Entities.Entity entitiy)
        {
            if (_target.Contains(entitiy))
                _target.Remove(entitiy);
        }

        public static void Draw()
        {
            if (!set)
            {
                _pixel.SetData(new[] { Color.White });
                set = true;
            }
            int centerX = (int)Game1.Instance.Camera.Center.X;
            int centerY = (int)Game1.Instance.Camera.Center.Y;
            foreach (Entities.Entity e in _target)
            {
                Game1.Instance.SpriteBatch.Draw(_pixel, new Rectangle(e.BoundingRect.X - centerX, e.BoundingRect.Y - centerY, e.BoundingRect.Width, 1), _color);
                Game1.Instance.SpriteBatch.Draw(_pixel, new Rectangle((int)(e.BoundingRect.X - centerX), (int)(e.BoundingRect.Y - centerY), 1, e.BoundingRect.Height), _color);
                Game1.Instance.SpriteBatch.Draw(_pixel, new Rectangle(e.BoundingRect.X + e.BoundingRect.Width - centerX, e.BoundingRect.Y - centerY, 1, e.BoundingRect.Height), _color);
                Game1.Instance.SpriteBatch.Draw(_pixel, new Rectangle(e.BoundingRect.X - centerX, e.BoundingRect.Y + e.BoundingRect.Height - centerY, e.BoundingRect.Width, 1), _color);
            }

        }

 
    }
}

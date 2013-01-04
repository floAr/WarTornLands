using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WarTornLands.Entities.AI
{
    interface Zone
    {
        bool Contains(Vector2 point);
    }

    class Circle : Zone
    {
        private Vector2 _position;
        private float _radius;

        public Circle(Vector2 position, float radius)
        {
            _position = position;
            _radius = radius;
        }

        public bool Contains(Vector2 point)
        {
            if ((_position - point).LengthSquared() > _radius * _radius)
                return false;
            else return true;
        }
    }

    class Rectangular : Zone
    {
        private Rectangle _rec;

        public Rectangular(Rectangle rec)
        {
            _rec = rec;
        }

        public bool Contains(Vector2 point)
        {
            return _rec.Contains(new Point((int)point.X, (int)point.Y));
        }
    }

    class Polygon : Zone
    {
        private List<Point> _vertices;

        public Polygon(List<Point> vertices)
        {
            _vertices = vertices;
        }

        public bool Contains(Vector2 point)
        {
            // This will get fucked up

            return false;
        }
    }
}

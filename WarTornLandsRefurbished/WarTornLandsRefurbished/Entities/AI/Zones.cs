using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Data;

namespace WarTornLands.Entities.AI
{
    public interface Zone
    {
        bool Contains(Vector2 point);
    }

    class ZoneFactory
    {
        public static Zone ProduceZone(DataRow data)
        {
            // Identify Circle
            try
            {
                object test = data.GetChildRows("object_polyline")[0];
                return ProduceCircle(data);
            }
            catch(IndexOutOfRangeException e) { }
            
            // Identify Rectangle
            try
            {
                object test = data["width"];
                return ProduceRectangular(data);
            }
            catch { }
            
            // Identify Polygon
            try
            {
                object test = data.GetChildRows("object_polygon")[0];
                return ProducePolygon(data);
            }
            catch (IndexOutOfRangeException e) { }

            throw new Exception("Zone faulty exception.");
        }

        private static Circle ProduceCircle(DataRow data)
        {
            List<Point> points = ParsePointList(data.GetChildRows("object_polyline")[0]["points"].ToString());

            Point position = points[1];
            int radius = (int)Math.Sqrt((points[1].X) ^ 2 + (points[1].Y) ^ 2);

            return new Circle(position, radius);
        }

        private static Rectangular ProduceRectangular(DataRow data)
        {
            return new Rectangular(
                new Rectangle(
                    int.Parse(data["x"].ToString()), 
                    int.Parse(data["y"].ToString()), 
                    int.Parse(data["width"].ToString()), 
                    int.Parse(data["height"].ToString())));
        }

        private static Polygon ProducePolygon(DataRow data)
        {
            return new Polygon(ParsePointList(data.GetChildRows("object_polygon")[0]["points"].ToString()));
        }

        private static List<Point> ParsePointList(string data)
        {
            List<Point> res = new List<Point>();

            int x = 0;
            int y = 0;
            int start = 0;
            for (int i = 0; i < data.Length; ++i)
            {
                if (data[i].Equals(','))
                {
                    x = int.Parse(data.Substring(start, i - start));
                    start = i + 1;
                }
                if (data[i].Equals(' ') || i == data.Length - 1)
                {
                    y = int.Parse(data.Substring(start, i - start + 1));
                    res.Add(new Point(x, y));
                    start = i + 1;
                }
            }

            return res;
        }
    }

    public class Circle : Zone
    {
        private Vector2 _position;
        private int _radius;

        public Circle(Point position, int radius)
        {
            _position = new Vector2(position.X, position.Y);
            _radius = radius;
        }

        public bool Contains(Vector2 point)
        {
            if ((_position - point).LengthSquared() > _radius * _radius)
                return false;
            else return true;
        }
    }

    public class Rectangular : Zone
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

    public class Polygon : Zone
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

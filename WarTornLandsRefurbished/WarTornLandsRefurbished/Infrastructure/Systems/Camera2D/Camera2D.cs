using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WarTornLands.Infrastructure.Systems.Camera2D
{
    public class Camera2D
    {
        public Vector2 Center { get { return _center; } }
        private Vector2 _center;

        private float _lerpBack;

        private bool _cinematics;
        private Curve splineX;
        private Curve splineY;
        private int _duration;
        private float _counter;

        private Entities.Entity _target;

        public Camera2D(Entities.Entity target)
        {
            _target = target;
            _center = target.Position;
        }

        public void SetTarget(Entities.Entity anchor)
        {
            _target = anchor;
        }
        internal void PlayCinematic(List<Vector2> points)
        {
            PlayCinematic(points, points.Count * 1000);
        }
        public void PlayCinematic(List<Vector2> points, int durationMS)
        {
            splineX = new Curve();
            splineY = new Curve();
            _duration = durationMS;
            int frac = _duration / points.Count;
            int i = 0;
            foreach (var point in points)
            {
                splineX.Keys.Add(new CurveKey(i * frac, point.X));
                splineY.Keys.Add(new CurveKey(i * frac, point.Y));
                i++;
            }
            _counter = 0;
            _cinematics = true;
        }

        public void Update(GameTime gameTime)
        {
             _counter += gameTime.ElapsedGameTime.Milliseconds;

            // Check whether we are in a cinematic or not
            if (!_cinematics)
            {
                // Check whether camera position equals target position
                if (_lerpBack == 0)
                    _center = Vector2.Lerp(_center, _target.Position, 1);
                else
                    _center = Vector2.Lerp(_center, _target.Position, Math.Min(_counter/_lerpBack, 1));
            }
            else
            {
                // Spline interpolation
                _center.X = splineX.Evaluate(Math.Min(_counter, _duration));
                _center.Y = splineY.Evaluate(Math.Min(_counter, _duration));

                if (_counter > _duration)
                {
                    _counter = 0;
                    _cinematics = false;
                    _lerpBack = Math.Abs(Vector2.Distance(_target.Position, _center)) * 10;
                }

            }
        }



        internal void BreakCinematic()
        {
            _counter = 0;
            _cinematics = false;
            _lerpBack = Math.Abs(Vector2.Distance(_target.Position, _center)) * 10;
        }
    }
}

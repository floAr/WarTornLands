using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using WarTornLands.DEBUG;

namespace WarTornLands.Infrastructure.Systems.DrawSystem.Effects
{
    class ShockwaveEffect : BaseEffect
    {
        private Vector2 _center;
        private float _radius;
        private double _time;
        private double _factor;
        private float _strength;
        private float _width;
        public ShockwaveEffect(Vector2 positionOnScreen, float radius, TimeSpan duration,float strength=1,float width=0.3f)
        {
            _factor = 1 / duration.TotalMilliseconds;
            _center = positionOnScreen;
            _effect = Game1.Instance.Content.Load<Effect>("effect/Shockwave");
            _radius = radius;
            _time = 0;
            _strength = strength;
            _width = width;
        }

        public override void Apply()
        {
            if (_time >= 1)
                return;
            _effect.Parameters["width"].SetValue(_width);
            _effect.Parameters["radius"].SetValue((float)_time * _radius);
            _effect.Parameters["magnitude"].SetValue((_strength - (_strength*(float)_time)));
            _effect.Parameters["centerCoord"].SetValue(_center);
            base.Apply();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            _time += _factor * gameTime.ElapsedGameTime.Milliseconds;
            // _distortion=   ms += gameTime.ElapsedGameTime.Milliseconds;
           
        }
    }
}

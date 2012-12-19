using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarTornLands.Infrastructure.Systems;

namespace WarTornLands.Entities.Modules.Draw.ParticleSystem
{
    public struct Range
    {
        public Range(float value)
        {
            Min=value;
            Max=value;
        }

        public Range(float min, float max)
        {
            Min = min;
            Max = max;
        }
        private static Random r=new Random();
        public float Min;
        public float Max;
        public float ValueInRange {
            get {
                return (float)(r.NextDouble() * (Max - Min) + Min);
            }
        }

    }
    public struct EmitterSetting
    {
        public Range MaxParticles;
        public Range DirectionX;
        public Range DirectionY;
         public Range SpeedX;
         public Range SpeedY;
         public Range StartingAngle;
         public Range AnglePermutation;
         public Range Lifetime;
         public Range LifeDecay;
         public Range ColorOffsetR;
         public Range ColorOffsetG;
         public Range ColorOffsetB;
         public Range Size;
         public Range AlphaDecay;
         public Range Alpha;

    }
    public class ParticleSystem : BaseModule, IDrawExecuter
    {
        private Pool<Particle> _particlePool;
        private EmitterSetting _setting;
        private Random _random = new Random();
        private List<Particle> _particles;
        private List<Texture2D> _textures;
        private bool _isLight = false;

        public bool IsLight { get { return _isLight; } set { _isLight = value; } }
        public ParticleSystem(EmitterSetting setting, List<Texture2D> textures)
        {

            this._textures = textures;
            this._particles = new List<Particle>();
            _setting = setting;
            _random = new Random();
            _particlePool = new Pool<Particle>(GenerateNewParticle);

        }

        private Particle GenerateNewParticle()
        {
            Texture2D texture = _textures[_random.Next(_textures.Count)];
            Vector2 velocity = new Vector2(_setting.SpeedX.ValueInRange, _setting.SpeedY.ValueInRange);
            float angle = _setting.StartingAngle.ValueInRange;
            float angularVelocity = _setting.AnglePermutation.ValueInRange;
            Color color = Color.White;
            float size = _setting.Size.ValueInRange;
            float ttl = _setting.Lifetime.ValueInRange;
            float alpha = _setting.Alpha.ValueInRange;
            float alphaDecay = _setting.AlphaDecay.ValueInRange;
            return new Particle(texture,  velocity, angle, angularVelocity, color, size, ttl,alpha,alphaDecay);
        }




        public void Draw(SpriteBatch batch, DrawInformation information)
        {
            if (_isLight != information.DrawLights)
                return;
            for (int index = 0; index < _particles.Count; index++)
            {
                _particles[index].Draw(Game1.Instance.SpriteBatch,information);
            }
        }

        public void Update(GameTime gameTime)
        {
            for (int i = _particles.Count-1; i >= 0; i--)
            {
                _particles[i].Update(gameTime);
                if (_particles[i].TTL <= 0)
                {
                    _particlePool.GiveBackObject(_particles[i]);
                    _particles.RemoveAt(i);

                }
            }

            for (int i = _particles.Count; i < _setting.MaxParticles.ValueInRange; i++)
            {
                _particles.Add(_particlePool.AllocateObject());
            }
        }

        public Vector2 Size
        {
            get {return Vector2.Zero; }
        }
    }
}
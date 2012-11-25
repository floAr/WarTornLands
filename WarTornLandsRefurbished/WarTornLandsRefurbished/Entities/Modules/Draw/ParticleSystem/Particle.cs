using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WarTornLands.Entities.Modules.Draw.ParticleSystem
{
   public class Particle
    {
        public Texture2D Texture { get; set; }        // The texture that will be drawn to represent the particle
        public Vector2 StartingPosition { get; set; }        // The current position of the particle        
        public Vector2 PositionOffset { get; set; } // Offset from Starting Position
        public Vector2 Velocity { get; set; }        // The speed of the particle at the current instance
        public float Angle { get; set; }            // The current angle of rotation of the particle
        public float AngularVelocity { get; set; }    // The speed that the angle is changing
        public Color Color { get; set; }            // The color of the particle
        public float Size { get; set; }                // The size of the particle
        public float TTL { get; set; }                // The 'time to live' of the particle in seconds
        public float Alpha { get; set; }            // The current Alpha
        public float AlphaDecay { get; set; }       //Alpha lost per second

        public Particle(Texture2D texture, Vector2 position, Vector2 velocity,
             float angle, float angularVelocity, Color color, float size, float ttl,float alpha,float alphaD)
        {
            Texture = texture;
            StartingPosition = position;
            PositionOffset = Vector2.Zero;
            Velocity = velocity;
            Angle = angle;
            AngularVelocity = angularVelocity;
            Color = color;
            Size = size;
            TTL = ttl;
            Alpha = alpha;
            AlphaDecay = alphaD;
        }

        public void Update(GameTime gameTime)
        {
            TTL-=gameTime.ElapsedGameTime.Milliseconds;
            PositionOffset += Velocity;
            Alpha -= AlphaDecay;
            if (Alpha <= 0)
                TTL = 0;
            Angle += AngularVelocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            spriteBatch.Draw(Texture, StartingPosition+PositionOffset, sourceRectangle, Color*Alpha,
                Angle, origin, Size, SpriteEffects.None, 0f);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WarTornLands.Entities.Modules.Draw
{
    /// <summary>
    /// Static drawer class, to draw entities consisting of a single static texture
    /// </summary>
    public class StaticDrawer :BaseModule, IDrawExecuter
    {
        private Vector2 _size;
        private Texture2D _tex;
        private bool _isLight = false;
        /// <summary>
        /// Gets or sets the texture.
        /// </summary>
        /// <value>
        /// The texture.
        /// </value>
        public Texture2D Texture { get { return _tex; } set { _tex = value; _size = new Vector2(_tex.Width, _tex.Height); } }
        public bool IsLight { get { return _isLight; } set { _isLight = value; } }

        // Whether the last draw call was invulnerable
        private bool _flashing = false;
        // Counts the time in the current "blink" state when invulnerable (ms)
        private float _blinkTime = 0;
        // Total duration of a normal/highlighted blink duration (ms)
        private float _blinkDuration = 400;

        /// <summary>
        /// Draws the specified batch.
        /// </summary>
        /// <param name="batch">The batch.</param>
        /// <param name="information">The information.</param>
        public void Draw(SpriteBatch batch, DrawInformation information)
        {
            if (_isLight != information.DrawLights)
                return;

            Vector2 drawLocation;
            if (information.Centered)
                drawLocation = information.Position - (_size / 2);
            else
                drawLocation = information.Position;

            Vector2 center = Game1.Instance.Camera.Center;
            Rectangle bounds = Game1.Instance.Window.ClientBounds;

            // Invulnerable counter configuration
            if (_flashing != information.Flashing)
            {
                _flashing = information.Flashing;
                if (!_flashing)
                    _blinkTime = 0;
            }
            
            // Set color according to invulnerable and counter state
            Color color;
            if (_flashing && _blinkTime < _blinkDuration / 2.0)
                color = Color.Red; // Red in the first half of the counter
            else
                color = Color.White; // White in the second half or when not invulnerable

            // Draw shadow if requested
            if (information.Shadow)
            {
                Texture2D tex = Game1.Instance.Content.Load<Texture2D>("sprite/shadow");

                batch.Draw(tex,
                    new Rectangle(
                        (int)drawLocation.X - (int)center.X + (int)Math.Round(bounds.Width / 2f) - (int)(_size.X / 2f),
                        (int)drawLocation.Y - (int)center.Y + (int)Math.Round(bounds.Height / 2f) + (int)(_size.Y / 2f) - (int)(tex.Bounds.Height / 2f) - 24,
                        tex.Bounds.Width, tex.Bounds.Height),
                    Color.White);
            }

            // Consider entity altitude
            drawLocation.Y -= information.Altitude;

            batch.Draw(Texture, new Rectangle((int)drawLocation.X - (int)center.X + (int)Math.Round(bounds.Width / 2.0f),
                (int)drawLocation.Y - (int)center.Y + (int)Math.Round(bounds.Height / 2.0f), (int)_size.X, (int)_size.Y),
                new Rectangle(0, 0, (int)_size.X, (int)_size.Y), color, information.Rotation, _size / 2, SpriteEffects.None, 0.5f);
        }

        public Vector2 Size
        {
            get { return new Vector2(_tex.Width, _tex.Height); }
        }


        public void Update(GameTime gameTime)
        {
            if (_flashing)
            {
                _blinkTime += gameTime.ElapsedGameTime.Milliseconds;
                while (_blinkTime > _blinkDuration)
                    _blinkTime -= _blinkDuration;
            }
        }

    }
}

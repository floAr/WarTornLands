using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WarTornLands.Entities.Modules.Draw
{

    public class AnimatedDrawer :BaseModule, IDrawExecuter
    {
        private Dictionary<string, Animation> _animations;
        private Texture2D _spriteSheet;
        private string _currentAnimation;

        private Vector2 _size;

        // Whether the last draw call was invulnerable
        private bool _flashing = false;
        // Counts the time in the current "blink" state when invulnerable (ms)
        private float _blinkTime = 0;
        // Total duration of a normal/highlighted blink duration (ms)
        private float _blinkDuration = 400;

        private bool _isLight = false;
        public bool HasEnded { get { return _animations[_currentAnimation].HasEnded; } }
        public bool IsLight { get { return _isLight; } set { _isLight = value; } }
        public AnimatedDrawer(Texture2D spriteSheet)
        {
            _spriteSheet = spriteSheet;
            _animations = new Dictionary<string, Animation>();

        }

        public void SetCurrentAnimation(string name)
        {
            if (_animations.ContainsKey(name))
            {
                _currentAnimation = name;
                _animations[_currentAnimation].Reset();
            }
        }

        public void Update(GameTime gameTime)
        {
            if (_flashing)
            {
                _blinkTime += gameTime.ElapsedGameTime.Milliseconds;
                while (_blinkTime > _blinkDuration)
                    _blinkTime -= _blinkDuration;
            }

            _animations[_currentAnimation].Update(gameTime);
        }

        public void Draw(SpriteBatch batch, DrawInformation information)
        {
            // TODO That's 90% the same code as in StaticDrawer.Draw!!

            if (_isLight != information.DrawLights)
                return;
            Animation _current = _animations[_currentAnimation];
            _size = new Vector2(_current.CurrentFrame.Width, _current.CurrentFrame.Height);

            Vector2 drawLocation = information.Position;

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

            // Draw entity
            batch.Draw(_spriteSheet, Game1.Instance.Camera.GetDrawRectangle(_owner.BoundingRect, information.Altitude), _current.CurrentFrame, color, information.Rotation, _size / 2, SpriteEffects.None, 0.5f);
        }

        internal void AddAnimation(Animation anim)
        {
            anim.Parent = this;
            _animations.Add(anim.Name, anim);
        }

        internal void AddAnimation(Animation anim,float offsetMS)
        {
            anim.Parent = this;
            anim.AddOffset(offsetMS);
            _animations.Add(anim.Name, anim);
        }





        public Vector2 Size
        {
            get { return new Vector2(_animations[_currentAnimation].CurrentFrame.Width, _animations[_currentAnimation].CurrentFrame.Height); }
        }


        public static AnimatedDrawer Explosion
        {
            get
            {
                AnimatedDrawer explosion = new AnimatedDrawer(Game1.Instance.Content.Load<Texture2D>("sprite/explosion"));
                Animation explode = new Animation("explode");
                for (int y = 0; y < 2; ++y)
                {
                    for (int x = 0; x < 8; ++x)
                    {
                        explode.AddFrame(new Rectangle(x * 64, y * 64, 64, 64));
                    }
                }
                explode.IsLooping = false;
                explode.IsRepeating = false;
                explosion.AddAnimation(explode);
                explosion.SetCurrentAnimation("explode");
                return explosion;
            }
        }
    }
}

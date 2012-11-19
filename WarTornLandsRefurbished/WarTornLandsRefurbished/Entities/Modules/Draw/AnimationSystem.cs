using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WarTornLands.Entities.Modules.Draw
{

    public class AnimatedDrawer : IDrawExecuter
    {
        private Dictionary<string, Animation> _animations;
        private Texture2D _spriteSheet;
        private string _currentAnimation;

        private Vector2 _loc;
        private Vector2 _size;

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
            _animations[_currentAnimation].Update(gameTime);
        }

        public void Draw(SpriteBatch batch, DrawInformation information)
        {
            Animation _current = _animations[_currentAnimation];
            _size = new Vector2(_current.CurrentFrame.Width, _current.CurrentFrame.Height);
            if (information.Centered)
                _loc = information.Position - (_size / 2);
            else
                _loc = information.Position;
            batch.Draw(_spriteSheet, new Rectangle((int)_loc.X, (int)_loc.Y, (int)_size.X, (int)_size.Y), _current.CurrentFrame, Color.White, information.Rotation, _size / 2, SpriteEffects.None, 0.5f);
        }

        internal void AddAnimation(Animation anim)
        {
            anim.Parent = this;
            _animations.Add(anim.Name, anim);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WarTornLands.PlayerClasses
{
    public class Player : GameComponent
    {
        Vector2 _position = Vector2.Zero;
        float _radius = 16;
        float _speed = .125f;
        Texture2D _texture;

        float _hitTimer;
        const float _maxHitTime = 1.0f;

        public Player(Game game) : base(game) { }

        public override void Update(GameTime gameTime)
        {
            InputManager input = (Game as Game1).input;
            
            _position = CollisionDetector.GetPosition(_position,
                                                      input.Move * _speed * gameTime.ElapsedGameTime.Milliseconds,
                                                      _radius);

            // Attack! Attack!
            _hitTimer = Math.Min(0, _hitTimer - (gameTime.ElapsedGameTime.Milliseconds/1000.0f));
            if (input.Hit)
            {
                Hit(gameTime);
            }

            base.Update(gameTime);
        }

        private void Hit(GameTime gameTime)
        {
            if (_hitTimer <= 0)
            {
                _hitTimer = _maxHitTime;
                // TODO hit
            }
        }

        public void LoadContent(ContentManager cm)
        {
            _texture = cm.Load<Texture2D>("player");
        }

        public Vector2 GetPosition()
        {
            return _position;
        }

        public void Draw(GameTime gameTime)
        {
            (Game as Game1).spriteBatch.Begin();
            (Game as Game1).spriteBatch.Draw(_texture,
                new Rectangle((int)Math.Round((Game as Game1).Window.ClientBounds.Width / 2.0 -_texture.Width * 0.5f),
                    (int)Math.Round((Game as Game1).Window.ClientBounds.Height / 2.0 - _texture.Height * 0.5f),
                    _texture.Height, _texture.Width) , Color.White);
            (Game as Game1).spriteBatch.End();
        }
    }
}

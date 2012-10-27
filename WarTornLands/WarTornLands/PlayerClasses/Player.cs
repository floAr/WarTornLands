using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WarTornLands.Counter;

namespace WarTornLands.PlayerClasses
{
    public class Player
    {
        Game _game;
        Vector2 _position = Vector2.Zero;
        Vector2 _weaponPos;
        float _radius = Constants.Radius;
        float _speed = Constants.Speed;
        Texture2D _texture;
        Texture2D _weaponTex;
        CounterManager _cm;
        float _weaponRange = Constants.WeaponRange;

        string _hitCounter = "hit_counter";

        public Player(Game game)
        {
            _game = game;

            _cm = new CounterManager();
            _cm.Bang += new EventHandler<BangEventArgs>(OnBang);

            _cm.AddCounter(_hitCounter, Constants.HitDuration);
        }

        public void Update(GameTime gameTime)
        {
            InputManager input = (_game as Game1)._input;
            
            _position = CollisionDetector.GetPosition(_position,
                                                      input.Move * _speed * gameTime.ElapsedGameTime.Milliseconds,
                                                      _radius);

            // Attack! Attack!
            if (input.Hit)
            {
                _cm.StartCounter(_hitCounter, false);
            }

            if(_cm.GetPercentage(_hitCounter) > 0)
            {
                float baseAngle = Constants.WeaponStartAngle;
                float maxAddition = Constants.WeaponGoalAngle - Constants.WeaponStartAngle;
                float finalAngle = _cm.GetPercentage(_hitCounter) * maxAddition + baseAngle;
                _weaponPos = new Vector2(_weaponRange * (float)Math.Cos(finalAngle),
                                                _weaponRange * (float)Math.Sin(finalAngle));
            }

            _cm.Update(gameTime);
        }

        private void Hit(GameTime gameTime)
        {

        }

        public void LoadContent(ContentManager cm)
        {
            _texture = cm.Load<Texture2D>("player");
            _weaponTex = cm.Load<Texture2D>("weapontest");
        }

        public Vector2 GetPosition()
        {
            return _position;
        }

        public void Draw(GameTime gameTime)
        {
            SpriteBatch sb = (_game as Game1)._spriteBatch;

            sb.Begin();

            Vector2 drawPos = new Vector2((float)Math.Round((_game as Game1).Window.ClientBounds.Width / 2.0 -_texture.Width * 0.5f),
                                          (float)Math.Round((_game as Game1).Window.ClientBounds.Height / 2.0 - _texture.Height * 0.5f));

            sb.Draw(_texture,
                new Rectangle((int)drawPos.X, (int)drawPos.Y,
                              _texture.Height, _texture.Width), Color.White);


            if (_cm.GetPercentage(_hitCounter) != 0)
            {
                float baseAngle = Constants.WeaponStartAngle;
                float maxAddition = Constants.WeaponGoalAngle - Constants.WeaponStartAngle;
                float finalAngle = _cm.GetPercentage(_hitCounter) * maxAddition + baseAngle;
                _weaponPos = new Vector2(_weaponRange * (float)Math.Cos(finalAngle),
                                                _weaponRange * (float)Math.Sin(finalAngle));
                _weaponPos += drawPos;
                sb.Draw(_weaponTex, _weaponPos, null, Color.White, 0, Vector2.Zero, .1f, SpriteEffects.None, 0);
            }

            sb.End();
        }

        #region Subscribed events

        private void OnBang(object sender, BangEventArgs e)
        {

        }

        #endregion
    }
}

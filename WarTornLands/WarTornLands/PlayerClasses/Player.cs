using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WarTornLands.Counter;
using WarTornLands.EntityClasses;

namespace WarTornLands.PlayerClasses
{
    enum Facing
    {
        LEFT,
        UP,
        RIGHT,
        DOWN
    }
    enum AnimationType
    {
        MOVING,
        STANDING
    }
    public class Player : Entity
    {
        //64*128
        Game _game;
        Vector2 _position = Vector2.Zero;
        Vector2 _weaponPos;
        float _radius = Constants.Radius;
        float _speed = Constants.Speed;
        Vector2 _direction;

        Texture2D _texture;
        Texture2D _weaponTex;

        //Animation////////////////////////7
        Facing _animFacing;
        AnimationType _animationType;
        Texture2D _animTexture;
        // Rectangle _animSource;
        Vector2 _animSource;
        Rectangle _animTarget;
        int _frame;
        /////////////////////////////////////
        CounterManager _cm;
        float _weaponRange = Constants.WeaponRange;


        string _hitCounter = "hit_counter";
        string _animCounter = "anim_counter";

        public Player(Game game)
            : base(game, Vector2.Zero, null)
        {
            _game = game;

            _cm = new CounterManager();
            _cm.Bang += new EventHandler<BangEventArgs>(OnBang);

            _cm.AddCounter(_hitCounter, Constants.HitDuration);

            //FrameCounter
            _cm.AddCounter(_animCounter, 250);
            _cm.StartCounter(_animCounter,false);
        }

        /// <summary>
        /// Gibt den aktuellen Blickwinkel des Spielers im Bogenmaß zurück, wobei Oben = 0.
        /// </summary>
        public double GetRoundedAngle()
        {
            switch (_animFacing)
            {
                case Facing.LEFT:   return 0;
                case Facing.UP:     return 0.5*Math.PI;
                case Facing.RIGHT:  return Math.PI;
                case Facing.DOWN:   return 1.5 * Math.PI;
            }
            return 0;
        }

        public void Update(GameTime gameTime)
        {
            _cm.StartCounter(_animCounter, false);
            InputManager input = (_game as Game1)._input;

            Vector2 oldPos = _position;
            _position = CollisionDetector.GetPosition(_position,
                                                      input.Move * _speed * gameTime.ElapsedGameTime.Milliseconds,
                                                      _radius, this);
            _direction =  _position-oldPos;
             
            if (_direction.Length() == 0)
            {
                _animationType = AnimationType.STANDING;
            }
            else
            {
                _direction.Normalize();
                _animationType = AnimationType.MOVING;

                if (Math.Abs(_direction.X) > Math.Abs(_direction.Y))
                {
                    if (_direction.X >= 0)
                        _animFacing = Facing.RIGHT;
                    else
                        _animFacing = Facing.LEFT;
                }
                else
                {
                    if (_direction.Y >= 0)
                        _animFacing = Facing.DOWN;
                    else
                        _animFacing = Facing.UP;
                }
            }
            switch (_animFacing)
            {
                case Facing.LEFT:
                    switch (_animationType)
                    {
                        case AnimationType.MOVING:
                            _animSource = new Vector2(6, 0);
                            break;
                        case AnimationType.STANDING:
                            _animSource = new Vector2(4, 0);
                            break;
                        default:
                            break;
                    }
                    break;
                case Facing.UP:
                    switch (_animationType)
                    {
                        case AnimationType.MOVING:
                            _animSource = new Vector2(6, 1);
                            break;
                        case AnimationType.STANDING:
                            _animSource = new Vector2(4, 1);
                            break;
                        default:
                            break;
                    }
                    break;
                case Facing.RIGHT:
                    switch (_animationType)
                    {
                        case AnimationType.MOVING:
                            _animSource = new Vector2(2, 1);
                            break;
                        case AnimationType.STANDING:
                            _animSource = new Vector2(0, 1);
                            break;
                        default:
                            break;
                    }
                    break;
                case Facing.DOWN:
                    switch (_animationType)
                    {
                        case AnimationType.MOVING:
                            _animSource = new Vector2(2, 0);
                            break;
                        case AnimationType.STANDING:
                            _animSource = new Vector2(0, 0);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            // Attack! Attack!
            if (input.Hit)
            {
                _cm.StartCounter(_hitCounter, false);
            }

            if (_cm.GetPercentage(_hitCounter) > 0)
            {
                float baseAngle = Constants.WeaponStartAngle + (float)GetRoundedAngle();
                float maxAddition = Constants.WeaponGoalAngle - Constants.WeaponStartAngle;
                float finalAngle = _cm.GetPercentage(_hitCounter) * maxAddition + baseAngle;
                _weaponPos = new Vector2(_weaponRange * (float)Math.Cos(finalAngle),
                                                _weaponRange * (float)Math.Sin(finalAngle));
            }

            _cm.Update(gameTime);
        }

        public void LoadContent(ContentManager cm)
        {
            _texture = cm.Load<Texture2D>("player");
            _weaponTex = cm.Load<Texture2D>("weapontest");
            _animTexture = cm.Load<Texture2D>("character_64x128");
        }

        public Vector2 GetPosition()
        {
            return _position;
        }

        public void Draw(GameTime gameTime)
        {
            SpriteBatch sb = (_game as Game1)._spriteBatch;

            sb.Begin();

            Vector2 drawPos = new Vector2((float)Math.Round((_game as Game1).Window.ClientBounds.Width / 2.0 - 64 * 0.5f),
                                          (float)Math.Round((_game as Game1).Window.ClientBounds.Height / 2.0 - 128 * 0.5f));

            sb.Draw(_animTexture, new Rectangle((int)drawPos.X, (int)drawPos.Y, 64, 128),
                new Rectangle((int)(_animSource.X + _frame) * 64, (int)_animSource.Y * 128, 64, 128), Color.White);


            if (_cm.GetPercentage(_hitCounter) != 0)
            {
                float baseAngle = Constants.WeaponStartAngle + (float)GetRoundedAngle();
                
                float maxAddition = Constants.WeaponGoalAngle - Constants.WeaponStartAngle;
                float finalAngle = _cm.GetPercentage(_hitCounter) * maxAddition + baseAngle;
                _weaponPos = new Vector2(_weaponRange * (float)Math.Cos(finalAngle),
                                                _weaponRange * (float)Math.Sin(finalAngle));

                Entity victim = (_game as Game1)._currentLevel.GetEntityAt(_position + _weaponPos);
                if (victim != null)
                {
                    victim.Damage(8);
                }

                _weaponPos += drawPos;

                sb.Draw(_weaponTex, _weaponPos, null, Color.White, 0, Vector2.Zero, .1f, SpriteEffects.None, 0);
            }

            sb.End();
        }

        #region Subscribed events

        private void OnBang(object sender, BangEventArgs e)
        {
            if (e.ID.Equals(_animCounter))
            {
                _frame = (_frame + 1) % 2;
            }

        }

        #endregion
    }
}

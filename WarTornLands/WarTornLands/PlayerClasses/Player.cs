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
    public class Player : Entity
    {
        //64*128
        float _speed = Constants.Player_Speed;
        Vector2 _size;
        Vector2 _direction;

        int _potionCount = 0;

        string _animCounter = "anim_counter";

        public Player(Game game)
            : base(game, Vector2.Zero, null)
        {
            _weaponRange = Constants.Player_WeaponRange;
            _cm = new CounterManager();
            _cm.Bang += new EventHandler<BangEventArgs>(OnBang);

            (Game as Game1)._input.Potion.Pressed += new EventHandler(UsePotion);

            _cm.AddCounter(_hitCounter, Constants.Player_HitDuration);
            _radius = Constants.Player_Radius;

            //FrameCounter
            _cm.AddCounter(_animCounter, 250);
            _cm.StartCounter(_animCounter,false);
        }

        /// <summary>
        /// Gibt den aktuellen Blickwinkel des Spielers im Bogenmaß zurück, wobei Oben = 0.
        /// </summary>


        public override void Update(GameTime gameTime)
        {
            _cm.StartCounter(_animCounter, false);
            InputManager input = (Game as Game1)._input;

            Vector2 oldPos = _position;
            _position = CollisionDetector.GetPosition(_position,
                                                      input.Move.Value * _speed * gameTime.ElapsedGameTime.Milliseconds,
                                                      _radius, this);

            #region Animation

            _direction =  _position - oldPos;
             
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
            
            #endregion
            
            // Attack! Attack!
            if (input.Hit.Value)
            {
                _cm.StartCounter(_hitCounter, false);
            }

            _cm.Update(gameTime);
            base.Update(gameTime);
        }

        public override void LoadContent(ContentManager cm)
        {
            _size = new Vector2(64, 128);
            _weaponTex = cm.Load<Texture2D>("weapontest");
            _animTexture = cm.Load<Texture2D>("character_64x128");
        }

        public override Vector2 GetDrawPosition()
        {
            return new Vector2((float)Math.Round((Game as Game1).Window.ClientBounds.Width / 2.0 - _size.X * 0.5f),
                                          (float)Math.Round((Game as Game1).Window.ClientBounds.Height / 2.0 - _size.Y * 0.5f));
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sb = (Game as Game1)._spriteBatch;

            _tilepos = _tilepos;

            Vector2 drawPos = GetDrawPosition();

            sb.Draw(_animTexture, new Rectangle((int)drawPos.X, (int)drawPos.Y, 64, 128),
                new Rectangle((int)(_animSource.X + _frame) * 64, (int)_animSource.Y * 128, 64, 128), Color.White);


            Vector2 weaponPos = base.GetRelWeaponDrawPos();

            if(weaponPos.LengthSquared() < 9001)
            {
                weaponPos += drawPos;
                sb.Draw(_weaponTex, weaponPos, null, Color.White, 0, Vector2.Zero, .1f, SpriteEffects.None, 0);
            }

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

        public int GetItemCount()
        {
            // TODO durch vernünftiges inventar ersetzen XD
            return _potionCount;
        }

        public void GivePotion()
        {
            // TODO durch vernünftiges inventar ersetzen XD
            _potionCount++;
        }

        public void UsePotion(object sender, EventArgs e)
        {
            // TODO durch vernünftiges inventar ersetzen XD
            if (_potionCount > 0)
            {
                _health += 3 + new System.Random().Next(8);
                _potionCount--;
            }
        }
    }
}

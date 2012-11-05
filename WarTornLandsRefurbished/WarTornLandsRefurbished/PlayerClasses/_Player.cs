using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLandsRefurbished.Entities;
using Microsoft.Xna.Framework;
using WarTornLands.Counter;
using Microsoft.Xna.Framework.Graphics;
using WarTornLands.Infrastructure;
using WarTornLands.Entities;

namespace WarTornLands.PlayerClasses
{
    public class Player : Entity
    {
        private static Player _player;

        public Inventory Inventory { get; private set; }

        private Vector2 _direction;
        private int _potionCount;
        private float _speed = Constants.Player_Speed;


        private Player(Game game)
            : base(game, Vector2.Zero, null, "Player")
        {
            _weaponRange = Constants.Player_WeaponRange;
            _radius = Constants.Player_Radius;
            Health = 0;
            CanBeAttacked = true;
            this.Inventory = Inventory.GetInstance();

            _cm = new CounterManager();
            _cm.Bang += new EventHandler<BangEventArgs>(OnBang);

            (Game as Game1).Input.UsePotion.Pressed += new EventHandler(OnUsePotion);
            (Game as Game1).Input.ExecuteHit.Pressed += new EventHandler(OnExecuteHit);
            (Game as Game1).Input.Interact.Pressed += new EventHandler(OnInteract);
        }

        public Player GetInstance(Game game)
        {
            if(_player == null)
                _player = new Player(game);

            return _player;
        }

        public void InitializeFromSavegame()
        {
            // Set position, health, potions blablabla
        }

        public new void Reset(int health)
        {
            // Used to reset Player after death
        }

        public bool Give(Items item)
        {
            return Inventory.Insert(item);
        }

        /// <summary>
        /// Gibt den aktuellen Blickwinkel des Spielers im Bogenmaß zurück, wobei Oben = 0.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            _cm.StartCounter(_animCounter, false);

            HandleMovement(gameTime);

            #region Animation

            //_direction = _position - oldPos;

            //if (_direction.Length() == 0)
            //{
            //    _animationType = AnimationType.STANDING;
            //}
            //else
            //{
            //    _direction.Normalize();
            //    _animationType = AnimationType.MOVING;

            //    if (Math.Abs(_direction.X) > Math.Abs(_direction.Y))
            //    {
            //        if (_direction.X >= 0)
            //            _animFacing = Facing.RIGHT;
            //        else
            //            _animFacing = Facing.LEFT;
            //    }
            //    else
            //    {
            //        if (_direction.Y >= 0)
            //            _animFacing = Facing.DOWN;
            //        else
            //            _animFacing = Facing.UP;
            //    }
            //}
            //switch (_animFacing)
            //{
            //    case Facing.LEFT:
            //        switch (_animationType)
            //        {
            //            case AnimationType.MOVING:
            //                _animSource = new Vector2(6, 0);
            //                break;
            //            case AnimationType.STANDING:
            //                _animSource = new Vector2(4, 0);
            //                break;
            //            default:
            //                break;
            //        }
            //        break;
            //    case Facing.UP:
            //        switch (_animationType)
            //        {
            //            case AnimationType.MOVING:
            //                _animSource = new Vector2(6, 1);
            //                break;
            //            case AnimationType.STANDING:
            //                _animSource = new Vector2(4, 1);
            //                break;
            //            default:
            //                break;
            //        }
            //        break;
            //    case Facing.RIGHT:
            //        switch (_animationType)
            //        {
            //            case AnimationType.MOVING:
            //                _animSource = new Vector2(2, 1);
            //                break;
            //            case AnimationType.STANDING:
            //                _animSource = new Vector2(0, 1);
            //                break;
            //            default:
            //                break;
            //        }
            //        break;
            //    case Facing.DOWN:
            //        switch (_animationType)
            //        {
            //            case AnimationType.MOVING:
            //                _animSource = new Vector2(2, 0);
            //                break;
            //            case AnimationType.STANDING:
            //                _animSource = new Vector2(0, 0);
            //                break;
            //            default:
            //                break;
            //        }
            //        break;
            //    default:
            //        break;
            //}

            #endregion

            _cm.Update(gameTime);
            base.Update(gameTime);
        }

        private void HandleMovement(GameTime gameTime)
        {
            InputManager input = (Game as Game1).Input;

            Vector2 oldPos = Position;
            Position = CollisionManager.CalcPosition(Position,
                                                      input.Move.Value * _speed * gameTime.ElapsedGameTime.Milliseconds,
                                                      _radius, this);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sb = (Game as Game1).SpriteBatch;

            Vector2 drawPos = GetDrawPosition();

            sb.Draw(_animTexture, new Rectangle((int)drawPos.X, (int)drawPos.Y, 64, 128),
                new Rectangle((int)(_animSource.X + _frame) * 64, (int)_animSource.Y * 128, 64, 128), Color.White);


            Vector2 weaponPos = base.GetRelWeaponDrawPos();


            if (weaponPos.LengthSquared() < 9001)
            {
                weaponPos += drawPos;
                sb.Draw(_weaponTex, new Vector2(32, 64) + weaponPos, null, Color.White, 0, Vector2.Zero, .1f, SpriteEffects.None, 0);
            }

        }

        #region Subscribed events

        private void OnBang(object sender, BangEventArgs e)
        {
            //if (e.ID.Equals(_animCounter))
            //{
            //    _frame = (_frame + 1) % 2;
            //}

        }

        public void OnUsePotion(object sender, EventArgs e)
        {
            // TODO durch vernünftiges inventar ersetzen XD
            if (_potionCount > 0)
            {
                Health += 3 + new System.Random().Next(8);
                _potionCount--;
            }
        }

        public void OnExecuteHit(object sender, EventArgs e)
        {
            _cm.StartCounter(_hitCounter, false);
        }

        public void OnInteract(object sender, EventArgs e)
        { 
        }

        #endregion
    }
}

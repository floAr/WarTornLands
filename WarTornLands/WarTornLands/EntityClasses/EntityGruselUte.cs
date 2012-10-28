using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarTornLands.PlayerClasses;
using WarTornLands.Counter;

namespace WarTornLands.EntityClasses
{
    enum AIstate { IDLE, AGGRO }

    class EntityGruselUte : Entity
    {
        AIstate _state = AIstate.IDLE;
        Entity _victim;
        float _speed = Constants.GruselUte_Speed;
        String _animCounter3 = "anim_counter3";
        String _animCounter2 = "anim_counter2";
        int _frame2 = 0;
        int _frame3 = 0;

        public EntityGruselUte(Game game, Vector2 position, Texture2D texture) : base(game, position, texture)
        {
            _health = Constants.GruselUte_Health;
            _canBeAttacked = true;
            _radius = Constants.GruselUte_Radius;
            _animTexture = texture;
            _weaponRange = Constants.GruselUte_HitRange;
            _animTexture = texture;
            _animSource = new Vector2(0, 0);
            _animated = true;

            _cm = new CounterManager();
            _cm.Bang += new EventHandler<BangEventArgs>(OnBang);
            _cm.AddCounter(_hitCounter, Constants.Player_HitDuration);
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager cm)
        {
            _weaponTex = cm.Load<Texture2D>("weapontest");
        }

        public override void Update(GameTime gameTime)
        {
            _victim = (Game as Game1)._player;
            Vector2 toPlayer = this._position - _victim.GetPosition();

            #region Check state

            if (toPlayer.LengthSquared() < Constants.GruselUte_SightRange * Constants.GruselUte_SightRange)
            {
                _state = AIstate.AGGRO;
            }
            else
            {
                _state = AIstate.IDLE;
            }

            switch (_state)
            {
                case AIstate.IDLE:
                    IdleBehavior(gameTime);
                    break;
                case AIstate.AGGRO:
                    AggroBehavior(gameTime);
                    break;
                default:
                    IdleBehavior(gameTime);
                    break;
            }
            #endregion
 
            _cm.Update(gameTime);

            base.Update(gameTime);
        }

        private void IdleBehavior(GameTime gameTime)
        {
 
        }

        private void AggroBehavior(GameTime gameTime)
        {
            Vector2 toPlayer = _victim.GetPosition() - this._position;

            if (toPlayer.LengthSquared() < Constants.GruselUte_HitRange * Constants.GruselUte_HitRange)
            {
                _cm.StartCounter(_hitCounter, false);
            }
            else
            {
                Move(toPlayer, gameTime);
            }
        }

        private void Move(Vector2 movement, GameTime gameTime)
        {
            movement.Normalize();
            //_position = CollisionDetector.GetPosition(_position,
            //                                         movement * _speed * gameTime.ElapsedGameTime.Milliseconds,
            //                                         _radius, this);
            _position = _position + movement * _speed * gameTime.ElapsedGameTime.Milliseconds;
        }

        private void HitPlayer()
        {
 
        }

        public override void Draw(GameTime gameTime)
        {
           // base.Draw(gameTime);


         /*   int width = (int)Math.Floor((double)(Game as Game1)._tileSetTexture.Width / Constants.TileSize);
            Vector2 size = GetSize();

            Rectangle drawRec = new Rectangle(
                            drawPos.X,
                            drawPos.Y,
                            (int)size.X, (int)size.Y);

            (Game as Game1)._spriteBatch.Draw(_texture, drawRec, Color.White);
*/
            switch (_state)
            {
                case AIstate.IDLE:
                    _frame = _frame3;
                    _animSource.Y = 0;
                    break;
                case AIstate.AGGRO:
                    _frame = _frame2;
                    _animSource.Y = 1;
                    break;
                default:
                    break;
            }
            SpriteBatch sb = (Game as Game1)._spriteBatch;
            Vector2 drawPos = GetDrawPosition();

            sb.Draw(_animTexture, new Rectangle((int)drawPos.X, (int)drawPos.Y, 94, 72),
                new Rectangle((int)(_animSource.X + _frame) * 94, (int)_animSource.Y * 72, 94, 72), Color.White);


            Vector2 weaponPos = base.GetRelWeaponDrawPos();

            if (weaponPos.LengthSquared() < 9001)
            {
                weaponPos += drawPos;
                sb.Draw(_weaponTex, weaponPos, null, Color.White, 0, Vector2.Zero, .1f, SpriteEffects.None, 0);
            }
        }

        public override void OnDie()
        {
            // TODO drop

            base.OnDie();
        }

        #region Subscribed events

        protected virtual void OnBang(object sender, BangEventArgs e)
        {
            if (e.ID.Equals(_animCounter2))
            {
                _frame2 = (_frame2 + 1) % 2;
            }
            if (e.ID.Equals(_animCounter3))
            {
                _frame3 = (_frame3 + 1) % 3;
            }
        }

        #endregion
    }
}

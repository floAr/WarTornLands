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

        public EntityGruselUte(Game game, Vector2 position, Texture2D texture) : base(game, position, texture)
        {
            _health = Constants.GruselUte_Health;
            _canbeattacked = true;
            _radius = Constants.GruselUte_Radius;

            _weaponRange = Constants.GruselUte_HitRange;

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
            base.Draw(gameTime);

            SpriteBatch sb = (Game as Game1)._spriteBatch;
            Vector2 drawPos = GetDrawPosition();
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

        }

        #endregion
    }
}

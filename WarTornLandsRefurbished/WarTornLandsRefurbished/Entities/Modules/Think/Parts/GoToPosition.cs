using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WarTornLands.Infrastructure;

namespace WarTornLands.Entities.Modules.Think.Parts
{
    class GoToPosition : BaseAbility
    {
        /// <summary>
        /// Gets or sets the movespeed.
        /// </summary>
        /// <value>
        /// The speed.
        /// </value>
        public float Speed { get; set; }
        /// <summary>
        /// Gets or sets the position the entity will run towards to.
        /// The bait entity will be cleared by setting this value
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Vector2 TargetPosition 
        {
            get { return _targetPosition; }
            set { _bait = null; _targetPosition = value; } 
        }
        /// <summary>
        /// Gets or sets an entity this entity will run towards to.
        /// The Position attribute will be cleared by setting this value
        /// </summary>
        /// <value>
        /// The bait.
        /// </value>
        public Entity Bait { get { return _bait; } set { _targetPosition = value.Position; _bait = value; } }
        public bool Active { get; private set; }

        private Vector2 _fail = new Vector2(9001);
        private Entity _owner;
        private Entity _bait;
        private Vector2 _targetPosition;
        private bool _frozen;

        public GoToPosition(float speed = .1f)
        {
            Speed = speed;
        }

        public void SetOwner(Entity owner)
        {
            _owner = owner;
        }

        public void Reset()
        {
            Active = false;
        }

        public void Freeze()
        {
            _frozen = true;
        }

        public void Unfreeze()
        {
            _frozen = false;
        }

        public void Update(GameTime gameTime)
        {
            if (Active && !_frozen)
            {
                if (!TargetPosition.Equals(_fail))
                {
                    TowardsPositionRoutine(gameTime);
                }
                if (Bait != null)
                {
                    TowardsEntityRoutine(gameTime);
                }
            }
        }

        private void TowardsPositionRoutine(GameTime gameTime)
        {
            Vector2 move = TargetPosition - _owner.Position;

            if (move.LengthSquared() < Speed * Speed)
            {
                _owner.Position = TargetPosition;
                Active = false;
                return;
            }

            move.Normalize();
            move *= Speed;
            Vector2 actualMove = CollisionManager.Instance.TryMove(_owner.Position, move, 0, _owner);
            _owner.Position = actualMove + _owner.Position;

            if (actualMove.Length() != move.Length())
                Active = false;
        }

        private void TowardsEntityRoutine(GameTime gameTime)
        {
            TowardsPositionRoutine(gameTime);
        }

        /// <summary>
        /// .
        /// </summary>
        /// <returns></returns>
        public bool TryExecute()
        {
            Active = true;
            return false;
        }

        /// <summary>
        /// Will cause the entity to stand still.
        /// </summary>
        /// <returns></returns>
        public bool TryCancel()
        {
            return false;
        }


    }
}

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
            get { return TargetPosition; } 
            set { Bait = null; TargetPosition = value; } 
        }
        /// <summary>
        /// Gets or sets an entity this entity will run towards to.
        /// The Position attribute will be cleared by setting this value
        /// </summary>
        /// <value>
        /// The bait.
        /// </value>
        public Entity Bait { get { return Bait; } set { TargetPosition = _fail; Bait = value; } }
        public bool Active { get; private set; }

        private Vector2 _fail = new Vector2(9001);
        private Entity _owner;

        public GoToPosition(float speed = .1f)
        { 
        }

        public void Update(GameTime gameTime)
        {
            if (Active)
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
            Vector2 move = _owner.Position - TargetPosition;

            if (move.LengthSquared() > Speed * Speed)
            {
                _owner.Position = TargetPosition;
                Active = false;
                return;
            }

            move.Normalize();
            _owner.Position = CollisionManager.Instance.TryMove(_owner.Position, move, 0, _owner);
        }

        private void TowardsEntityRoutine(GameTime gameTime)
        {

        }

        /// <summary>
        /// The entity will try to move as long as its updated so dont use this.
        /// </summary>
        /// <returns></returns>
        public bool TryExecute()
        {
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

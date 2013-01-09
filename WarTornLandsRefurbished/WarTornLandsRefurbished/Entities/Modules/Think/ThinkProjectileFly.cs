using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WarTornLands.Entities.AI;

namespace WarTornLands.Entities.Modules.Think
{
    class ThinkProjectileFly : BaseModule, IThinkModule
    {
        public float Speed { get; set; }
        private Vector2 _startPos;

        public ThinkProjectileFly(float speed = 1.0f) : base()
        {
            Speed = speed;
        }

        public void SetZone(Zone zone)
        {
        }

        public override void SetOwner(Entity owner)
        {
            base.SetOwner(owner);
            _startPos = _owner.Position;
        }

        public void Update(GameTime gameTime)
        {
            switch (_owner.Face)
            {
                case Facing.DOWN:
                    _owner.Position += new Vector2(0, Speed * gameTime.ElapsedGameTime.Milliseconds);
                    break;
                case Facing.UP:
                    _owner.Position -= new Vector2(0, Speed * gameTime.ElapsedGameTime.Milliseconds);
                    break;
                case Facing.LEFT:
                    _owner.Position -= new Vector2(Speed * gameTime.ElapsedGameTime.Milliseconds, 0);
                    break;
                case Facing.RIGHT:
                    _owner.Position += new Vector2(Speed * gameTime.ElapsedGameTime.Milliseconds, 0);
                    break;
            }
        }
    }
}

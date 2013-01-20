using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WarTornLands.Entities.AI;
using WarTornLands.Infrastructure;

namespace WarTornLands.Entities.Modules.Think
{
    class ThinkProjectileFly : BaseModule, IThinkModule
    {
        private Entity _shooter;
        public int Damage { get; set; }
        public int RangeSquared { get; set; }
        public float Speed { get; set; }
        private Vector2 _startPos;

        public ThinkProjectileFly(Entity shooter, int damage, int range, float speed) : base()
        {
            _shooter = shooter;
            Damage = damage;
            RangeSquared = range * range;
            Speed = speed / 1000.0f;
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
            // Backup position
            Vector2 oldPos = _owner.Position;

            // Move projectile
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

            // Check range
            if ((_startPos - _owner.Position).LengthSquared() > RangeSquared)
            {
                // Remove projectile, don't do any damage
                _owner.ToBeRemoved = true;
                return;
            }


            // Damage
            List<Entity> hit;
            CollisionManager.Instance.CollideRectangle(_owner, _owner.Position - oldPos, false, false, out hit);

            hit.RemoveAll(delegate(Entity ent) { return ent.Equals(_shooter) || ent.HitModule == null; });

            if (hit.Count > 0)
            {
                // Do damage to all entites but the shooter
                // TODO sort and only do damage to nearest entity
                foreach (Entity e in hit)
                {
                    e.Damage(Damage);
                }
                _owner.ToBeRemoved = true;
            }
            
        }
    }
}

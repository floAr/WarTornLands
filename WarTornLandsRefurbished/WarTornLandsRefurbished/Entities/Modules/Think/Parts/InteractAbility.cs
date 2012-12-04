using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.World;
using Microsoft.Xna.Framework;

namespace WarTornLands.Entities.Modules.Think.Parts
{
    class InteractAbility : BaseAbility
    {
        /// <summary>
        /// Gets or sets the interation range.
        /// </summary>
        /// <value>
        /// The range in roughly pixels.
        /// </value>
        public float Range { get; set; }


        private Entity _owner;
        private Level _level;

        public InteractAbility(float range = 50)
        {
            Range = range;
            _level = Game1.Instance.Level;
        }

        public void SetOwner(Entity owner)
        {
            _owner = owner;
        }

        public bool TryExecute()
        {
            Vector2 testPos = _owner.Position;
            float rangeModifier = 1.2f;

            switch (_owner.Face)
            {
                case Facing.UP:
                    testPos += new Vector2(0, -1) * Range * rangeModifier;
                    break;
                case Facing.DOWN:
                    testPos += new Vector2(0, 1) * Range * rangeModifier;
                    break;
                case Facing.LEFT:
                    testPos += new Vector2(-1, 0) * Range * rangeModifier;
                    break;
                case Facing.RIGHT:
                    testPos += new Vector2(1, 0) * Range * rangeModifier;
                    break;
            }

            List<Entity> targets = _level.GetEntitiesAt(testPos);

            if (targets.Count == 0 || (targets.Count == 1 && targets.First().Equals(_owner)))
                return false;

            Entity closest = targets.First();
            float closestDistSquared = float.PositiveInfinity;

            foreach (Entity ent in targets)
            {
                if (!ent.Equals(_owner))
                {
                    Vector2 distance = _owner.Position - ent.Position;
                    if (distance.LengthSquared() < closestDistSquared)
                    {
                        closest = ent;
                        closestDistSquared = distance.LengthSquared();
                    }
                }
            }

            if (!closest.Equals(_owner))
            {
                closest.Interact(_owner);
                return true;
            }
            else
                return false;
        }

        public bool TryCancel()
        {
            return false;
        }
    }
}

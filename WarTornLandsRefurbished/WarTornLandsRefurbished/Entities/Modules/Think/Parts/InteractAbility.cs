using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        public int Range { get; set; }


        private Entity _owner;

        public InteractAbility(int range = 50)
        {
            Range = range;
        }

        public void SetOwner(Entity owner)
        {
            _owner = owner;
        }

        public void Update(GameTime gameTime)
        {
 
        }

        public bool TryExecute()
        {
            // Get owner's boundings as a starting point
            // TODO Tweak - should we take the BoundingRect here?
            Rectangle rect = _owner.MovingRect;

            // Stretch rectangle in face direction
            switch (_owner.Face)
            {
                case Facing.UP:
                    rect.Y -= Range;
                    rect.Height += Range;
                    break;
                case Facing.DOWN:
                    rect.Y += Range;
                    rect.Height += Range;
                    break;
                case Facing.LEFT:
                    rect.X -= Range;
                    rect.Width += Range;
                    break;
                case Facing.RIGHT:
                    rect.X += Range;
                    rect.Width += Range;
                    break;
            }

            HashSet<Entity> targets = Game1.Instance.Level.GetEntitiesAt(rect);

            if (targets.Count == 0 || (targets.Count == 1 && targets.First().Equals(_owner)))
                return false;

            Entity closest = targets.First();
            SortedList<float, Entity> resultList = new SortedList<float, Entity>();
            foreach (Entity ent in targets)
            {
                if (!ent.Equals(_owner))
                {
                    Vector2 distance = _owner.Position - ent.Position;
                    float key = distance.LengthSquared();
                    while (resultList.ContainsKey(key))
                        key += 0.01f;                    
                    resultList.Add(key, ent);
                }
            }

            for (int i = 0; i < resultList.Count; ++i)
            {
                Entity e = resultList.Values[i];
                if (e.InteractModule != null)
                {
                    e.Interact(_owner);
                    return true;
                }
            }
            return false;
        }

        public bool TryCancel()
        {
            return false;
        }
    }
}

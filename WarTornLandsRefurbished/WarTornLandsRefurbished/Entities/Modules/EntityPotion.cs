using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarTornLands.PlayerClasses;

namespace WarTornLands.Entities.Implementations
{
    class EntityPotion : Entity
    {
        public EntityPotion(Game game, Vector2 position, Texture2D texture) 
            : base(game, position, texture)
        {
            CanBePickedUp = true;
        }

        public override void OnCollide(Entity source)
        {
            // "einsammeln"
            bool sucess = false;

            if (CanBePickedUp)
            {
                if (source.Equals((Game as Game1).Player))
                {
                    sucess = (source as PlayerClasses.Player).Give(Items.Potion);
                }
            }

            if(sucess)
                base.OnCollide(source);
        }
    }
}

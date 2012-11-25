using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Entities;
using Microsoft.Xna.Framework;

namespace WarTornLands.Entities.Modules.Interact
{
   public class PickUp :BaseModule, IInteractModule
    {
       private WarTornLands.PlayerClasses.Items _loot;

       public PickUp(WarTornLands.PlayerClasses.Items loot)
       {
           _loot = loot;
       }
        public void Interact(InteractInformation information)
        {
            Game1.Instance.Player.GiveItem(_loot);
        }

        public void Update(GameTime gameTime)
        { }
    }
}

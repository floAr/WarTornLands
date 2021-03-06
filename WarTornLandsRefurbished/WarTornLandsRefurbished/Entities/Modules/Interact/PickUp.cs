﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Entities;
using Microsoft.Xna.Framework;

namespace WarTornLands.Entities.Modules.Interact
{
   public class PickUp : BaseModule, IInteractModule
    {
       private WarTornLands.PlayerClasses.Items.Item _loot;

       public PickUp(WarTornLands.PlayerClasses.Items.Item loot)
       {
           _loot = loot;
       }
        public void Interact(Entity information)
        {
            Game1.Instance.Player.GiveItem(_loot);
        }

        public void Update(GameTime gameTime)
        { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Entities;
using WarTornLandsRefurbished.Entities.Modules;

namespace WarTornLands.Entities.Modules.Interact
{
   public class PickUp:BaseModule, IInteractModule
    {
       public PickUp(Entity owner)
           : base(owner)
       {

       }
        public void Interact(Entity invoker, Entity target,InteractInformation information)
        {
            
        }
    }
}

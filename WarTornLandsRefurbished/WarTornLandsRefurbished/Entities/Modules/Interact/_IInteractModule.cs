using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Entities;

namespace WarTornLands.Entities.Modules.Interact
{
    /// <summary>
    /// Additional information to pass to interaction
    /// </summary>
    public struct InteractInformation
    {
    }
  public  interface IInteractModule
    {
       
        /// <summary>
        /// Called when an interaction is invoked
        /// </summary>
        /// <param name="invoker">Entity which started the interaction</param>
        /// <param name="target">Entity that has been interacted with</param>
        /// <param name="information">additional interaction information</param>
         void Interact(Entity invoker, Entity target, InteractInformation information);
    }
}

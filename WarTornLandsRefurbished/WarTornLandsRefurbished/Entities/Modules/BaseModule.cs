using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Entities;

namespace WarTornLandsRefurbished.Entities.Modules
{
    public abstract class BaseModule
    {
        protected Entity _owner;
        public BaseModule(Entity owner)
        {
            _owner = owner;
        }
    }
}

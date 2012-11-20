using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Entities;

namespace WarTornLandsRefurbished.Entities.Modules
{
    public abstract class BaseModule
    {
        protected Entity _owner = null;
        public Entity Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }
    }
}

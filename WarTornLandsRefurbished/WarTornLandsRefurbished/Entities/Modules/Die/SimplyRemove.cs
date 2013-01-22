using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WarTornLands.Entities.Modules.Die
{
    class SimplyRemove : BaseModule, IDieModule
    {
        public SimplyRemove()
        {
        }

        public SimplyRemove(DataRow data) : this()
        {
        }

        public void Die()
        {
            // Just remove the owner
            _owner.ToBeRemoved = true;
        }
    }
}

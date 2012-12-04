using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarTornLands.Entities.Modules.Think.Parts
{
    interface BaseAbility
    {
        bool TryExecute();
        bool TryCancel();
        void SetOwner(Entity owner);
    }
}

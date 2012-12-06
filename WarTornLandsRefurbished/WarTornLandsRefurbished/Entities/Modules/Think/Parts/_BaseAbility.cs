using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WarTornLands.Entities.Modules.Think.Parts
{
    interface BaseAbility
    {
        bool TryExecute();
        bool TryCancel();
        void Update(GameTime gameTime);
        void SetOwner(Entity owner);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WarTornLands.Entities.Modules.Hit
{
    public interface IHitModule
    {
        int Damage(int damage);
        bool IsFlashing();
    }
}

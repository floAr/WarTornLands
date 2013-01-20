using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WarTornLands.Entities.Modules.Hit
{
    class BlockHit : BaseModule, IHitModule
    {
        public BlockHit()
        {
        }

        public BlockHit(DataRow data)
        {
        }

        public int Damage(int damage)
        {
            // Block damage
            return 0;
        }

        public bool IsFlashing()
        {
            // Never flash
            return false;
        }
    }
}

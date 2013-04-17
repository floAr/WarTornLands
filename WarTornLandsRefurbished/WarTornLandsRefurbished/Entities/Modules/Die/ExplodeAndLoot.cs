using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Entities.Modules.Draw;
using WarTornLands.Entities.Modules.Interact;
using WarTornLands.Entities.Modules.Collide;
using System.Data;
using WarTornLands.PlayerClasses.Items;
using System.Runtime.Serialization;

namespace WarTornLands.Entities.Modules.Die
{
    [Serializable]
    public class ExplodeAndLoot : BaseModule, IDieModule
    {
        private PickUpOnCollide _pick;
        private AnimatedDrawer _explosion;

        public ExplodeAndLoot(Item loot)
        {
            _pick = new PickUpOnCollide(loot);
            _explosion = AnimatedDrawer.Explosion;
        }
        public ExplodeAndLoot(DataRow data)
            : this(Item.Nothing)
        { }

        public void Die()
        {
            _owner.RemoveAllModules();
            _owner.AddModule(_explosion);
            _owner.AddModule(_pick);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {


        }

        public ExplodeAndLoot(SerializationInfo info, StreamingContext context)
        {

        }
    }
}

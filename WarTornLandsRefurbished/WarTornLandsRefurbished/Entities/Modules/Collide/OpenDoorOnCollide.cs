using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarTornLands.Entities.Modules.Collide
{
    class OpenDoorOnCollide : BaseModule, ICollideModule
    {
        private bool _locked;
        private string _id;

        public OpenDoorOnCollide()
        {
            _locked=false;
        }
        public OpenDoorOnCollide(String id)
        {
            _id = id;
            _locked = true;
        }
        public bool OnCollide(CollideInformation info)
        {
            // TODO open only if player has key!
            if (_locked)
            {
                if (Game1.Instance.Player.Inventory.HasKey(_id))
                {
                    _owner.IsDead = true;
                    return true;
                }
                else
                {
                    return false;
                }


            }
            else
            {
                _owner.IsDead = true;
                return true;
            }
        }
    }
}

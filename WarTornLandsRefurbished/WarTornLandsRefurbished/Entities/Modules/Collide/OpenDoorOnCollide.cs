using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WarTornLands.Entities.Modules.Collide
{
    class OpenDoorOnCollide : BaseModule, ICollideModule
    {
        private bool _locked;
        private int _id;

        public OpenDoorOnCollide()
        {
            _locked=false;
        }
        public OpenDoorOnCollide(int id)
        {
            _id = id;
            _locked = true;
        }
        public OpenDoorOnCollide(DataRow data)
        {
            _id = int.Parse(data["KeyID"].ToString());
            _locked = true;
        }

        public bool OnCollide(CollideInformation info)
        {
            // TODO open only if player has key!
            if (_locked)
            {
                if (Game1.Instance.Player.Inventory.HasKey(_id))
                {
                    _owner.ToBeRemoved = true;
                    return true;
                }
                else
                {
                    return false;
                }


            }
            else
            {
                _owner.ToBeRemoved = true;
                return true;
            }
        }

        public Microsoft.Xna.Framework.Rectangle BodyShape
        {
            get;
            set;
        }

        public Microsoft.Xna.Framework.Rectangle MovingShape
        {
            get;
            set;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using WarTornLands.Infrastructure;

namespace WarTornLands.Entities.Modules.Collide
{
    class OpenDoorOnCollide : BaseModule, ICollideModule
    {
        private bool _locked;
        private string _areaID;


        public OpenDoorOnCollide(string areaID)
        {
            _areaID = areaID;
            _locked = true;
        }
        public OpenDoorOnCollide(DataRow data)
            : this(EntityBuilder.Instance.CurrentArea.AreaID)
        {
        }

        public void OnCollide(CollideInformation info)
        {
            // TODO open only if player has key!
            if (_locked)
            {
                if (Game1.Instance.Player.Inventory.HasKey(_areaID))
                {
                    _owner.ToBeRemoved = true;
                    return;
                }
                else
                {
                    return;
                }
            }
            else
            {
                _owner.ToBeRemoved = true;
            }
        }

        public bool IsPassable(CollideInformation info)
        {
            if (!_locked || _owner.ToBeRemoved)
                return true;

            return false;
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

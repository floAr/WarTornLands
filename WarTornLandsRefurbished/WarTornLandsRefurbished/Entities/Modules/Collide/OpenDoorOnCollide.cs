using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using WarTornLands.Infrastructure;
using System.Runtime.Serialization;
using WarTornLands.Infrastructure.Systems.SaveLoad;

namespace WarTornLands.Entities.Modules.Collide
{
    [Serializable]
    class OpenDoorOnCollide : BaseModule, ICollideModule, ISerializable
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

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {

            info.AddValue("locked", _locked);
            info.AddValue("areaID", _areaID);
            SaveLoadHelper.SaveRectangle(ref info, BodyShape, "bodyShape");
            SaveLoadHelper.SaveRectangle(ref info, MovingShape, "movingShape");

        }

        public OpenDoorOnCollide(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _locked = info.GetBoolean("locked");
            _areaID = info.GetString("areaID");
            BodyShape = SaveLoadHelper.LoadRectangle(ref info, "bodyShape");
            MovingShape = SaveLoadHelper.LoadRectangle(ref info, "movingShape");
        }
    }
}

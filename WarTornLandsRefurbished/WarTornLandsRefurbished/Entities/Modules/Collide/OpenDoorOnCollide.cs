﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using WarTornLands.Infrastructure;
using System.Runtime.Serialization;
using WarTornLands.Infrastructure.Systems.SaveLoad;
using WarTornLands.PlayerClasses;
using Microsoft.Xna.Framework;

namespace WarTornLands.Entities.Modules.Collide
{
    [Serializable]
    class OpenDoorOnCollide : BaseModule, ICollideModule, ISerializable
    {
        private bool _locked;
        private string _areaID;
        private bool _needsMaster;

        private OpenDoorOnCollide(string areaID)
        {
            _areaID = areaID;
            _locked = true;
        }
        public OpenDoorOnCollide(DataRow data)
            : this(EntityBuilder.Instance.CurrentArea.AreaID)
        {
            DataRow props = data.GetChildRows("object_properties")[0];

            try
            {
                _needsMaster = bool.Parse(XMLParser.ValueOfProperty(props, "master"));
            }
            catch (PropertyNotFoundException e)
            { }
        }

        public Vector2 OnCollide(CollideInformation info)
        {
            if (_locked)
            {
                if (Player.Instance.Inventory.HasKey(_areaID))
                {
                    _owner.ToBeRemoved = true;
                    return Vector2.Zero;
                }
                else
                {
                    return Vector2.Zero;
                }
            }
            else
            {
                _owner.ToBeRemoved = true;
            }

            return Vector2.Zero;
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

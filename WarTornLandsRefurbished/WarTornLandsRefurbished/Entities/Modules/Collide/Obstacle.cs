using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using WarTornLands.Infrastructure.Systems.SaveLoad;
using Microsoft.Xna.Framework;

namespace WarTornLands.Entities.Modules.Collide
{
      [Serializable]
    class Obstacle : BaseModule, ICollideModule, ISerializable
    {
        public Vector2 OnCollide(CollideInformation info)
        {
            // Do nothing

            return Vector2.Zero;
        }

        public bool IsPassable(CollideInformation info)
        {
            // Source check
            if (info.Collider == _owner)
                return true;

            // Altitude / height check
            if (info.Collider.Altitude + info.Collider.BodyHeight < _owner.Altitude ||
                info.Collider.Altitude > _owner.Altitude + _owner.BodyHeight)
                return true;
            
            return false;
        }

        public Obstacle()
        { }

        public Obstacle(DataRow data)
            : this()
        { }

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
            SaveLoadHelper.SaveRectangle(ref info, BodyShape, "bodyShape");
            SaveLoadHelper.SaveRectangle(ref info, MovingShape, "movingShape");

        }

        public Obstacle(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            BodyShape = SaveLoadHelper.LoadRectangle(ref info, "bodyShape");
            MovingShape = SaveLoadHelper.LoadRectangle(ref info, "movingShape");
        }
    }
}

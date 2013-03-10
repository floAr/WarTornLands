using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using WarTornLands.Infrastructure;
using Microsoft.Xna.Framework;
using WarTornLands.Counter;
using System.Runtime.Serialization;
using System.Security.Permissions;
using WarTornLands.Infrastructure.Systems.SaveLoad;

namespace WarTornLands.Entities.Modules.Collide
{
    [Serializable]
    class JumpPoint : BaseModule, ICollideModule, ISerializable
    {
        public string JumpID { get; private set; }
        public string TargetID { get; private set; }
        private Entity _target;
        private Entity _collider;

        private string _cUpdater = "JPUpdater";

        private bool _locked;
        private bool _collided;
        // TODO:
        // Maybe save ID of target area and preload it when the player is near the JumpPoint

        public JumpPoint(string jumpID, string target)
        {
            JumpID = jumpID;
            TargetID = target;
            _locked = false;
        }
        public JumpPoint(DataRow data)
            : this("", "")
        {
            throw new NotImplementedException();
        }

        public override void SetOwner(Entity owner)
        {
            base.SetOwner(owner);

            Owner.CM.AddCounter(_cUpdater, 500, true);
            Owner.CM.Bang += new EventHandler<BangEventArgs>(OnBang);
        }

        public void Connect(Entity target)
        {
            if (!(target.CollideModule is JumpPoint))
                throw new Exception();

            _target = target;
        }

        public void OnCollide(CollideInformation info)
        {
            // If you want that only the Player may use JumpPoint uncomment these 2 lines
            if (!info.IsPlayer)
                return;

            if (_locked && Owner.CM.GetPercentage(_cUpdater) > 0)
            {
                _collided = true;
                return;
            }

            Owner.CM.StartCounter(_cUpdater, false, true);

            (_target.CollideModule as JumpPoint).LockJP();

            _collider = info.Collider;
            _collided = true;
        }

        public void LockJP()
        {
            _locked = true;
        }

        public bool IsPassable(CollideInformation info)
        {
            return true;
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

        #region Subscribed events

        private void OnBang(object sender, BangEventArgs e)
        {
            if (e.IsDesiredCounter(_cUpdater))
            {
                if (_collided)
                {
                    _collided = false;
                    _collider.Position = _target.Position;
                    return;
                }
                else
                {
                    _locked = false;
                    Owner.CM.CancelCounter(_cUpdater);
                    return;
                }
            }
        }

        #endregion

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("jumpID", JumpID);
            info.AddValue("targetID", TargetID);
            info.AddValue("target", Entity.Global(_target));
            info.AddValue("collider", Entity.Global(_collider));
            info.AddValue("locked", _locked);
            info.AddValue("collided", _collided);
            SaveLoadHelper.SaveRectangle(ref info, BodyShape, "bodyShape");
            SaveLoadHelper.SaveRectangle(ref info, MovingShape, "movingShape");

        }

        public JumpPoint(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            JumpID = info.GetString("jumpID");
            TargetID = info.GetString("targetID");
            _target = Entity.Global(info.GetInt16("target"));
            _collider = Entity.Global(info.GetInt16("collider"));
            _locked = info.GetBoolean("locked");
            _collided = info.GetBoolean("collided");
            BodyShape = SaveLoadHelper.LoadRectangle(ref info, "bodyShape");
            MovingShape = SaveLoadHelper.LoadRectangle(ref info, "movingShape");
        }
    }
}

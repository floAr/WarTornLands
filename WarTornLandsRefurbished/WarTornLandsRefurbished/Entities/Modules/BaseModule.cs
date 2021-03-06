﻿
using Microsoft.Xna.Framework;
using System.Data;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
namespace WarTornLands.Entities.Modules
{
    [Serializable]
    public abstract class BaseModule : ISerializable
    {

        public Entity Owner { get { return _owner; } }
        protected Entity _owner = null;

        public BaseModule()
        {}

        public virtual void SetOwner(Entity owner)
        {
            _owner = owner;
        }

        public static BaseModule GetModule(DataRow data)
        {
            string type = "";

            try
            {
                type = data["Type"].ToString();
            }
            catch { return null; }


            switch (type)
            {
                // Collide modules
                case "Obstacle":
                    return new Collide.Obstacle(data);
                case "OpenDoorOnCollide":
                    return new Collide.OpenDoorOnCollide(data);
                case "PickUpOnCollide":
                    return new Collide.PickUpOnCollide(data);

                // Die modules
                case "ExplodeAndLoot":
                    return new Die.ExplodeAndLoot(data);
                case "ReplaceByStatic":
                    return new Die.ReplaceByStatic(data);
                case "SimplyRemove":
                    return new Die.SimplyRemove(data);

                // Draw modules
                case "StaticDrawer":
                    return new Draw.StaticDrawer(data);
                case "DualDraw":
                    return new Draw.DualDraw(data);
                case "AnimatedDrawer":
                    return new Draw.AnimatedDrawer(data);

                // Interact modules
                case "Dialog":
                    return new Interact.Dialog(data);

                // Think modules
                case "ThinkInputGuided":
                    return new Think.ThinkInputGuided(data);
                case "ThinkRoamAround":
                    return new Think.ThinkRoamAround(data);

                // Hit modules
                case "DamageFlash":
                    return new Hit.DamageFlash(data);
                case "BlockHit":
                    return new Hit.BlockHit(data);
            }

            if (!type.Equals(""))
                throw new Exception("Module type " + type + " not recognised.");

            return null;
        }

           [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("owner", Entity.Global(_owner));
        }

     
        protected BaseModule(SerializationInfo info, StreamingContext context)
        {
            _owner = Entity.Global(info.GetInt16("owner"));
        }
    }
}

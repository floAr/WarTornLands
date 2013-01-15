
using Microsoft.Xna.Framework;
using System.Data;
using System;
namespace WarTornLands.Entities.Modules
{
    public abstract class BaseModule
    {
        public Entity Owner { get { return _owner; } }

        protected Entity _owner = null;

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
                case "Obstacle":
                    return new Collide.Obstacle(data);
                case "OpenDoorOnCollide":
                    return new Collide.OpenDoorOnCollide(data);
                case "PickUpOnCollide":
                    return new Collide.PickUpOnCollide(data);
                case "ExplodeAndLoot":
                    return new Die.ExplodeAndLoot(data);
                case "StaticDrawer":
                    return new Draw.StaticDrawer(data);
                case "DualDraw":
                    return new Draw.DualDraw(data);
                case "AnimatedDrawer":
                    return new Draw.AnimatedDrawer(data);
                case "Dialog":
                    return new Interact.Dialog(data);
                case "ThinkInputGuided":
                    return new Think.ThinkInputGuided(data);
                case "ThinkRoamAround":
                    return new Think.ThinkRoamAround(data);
                case "ReplaceByStatic":
                    return new Die.ReplaceByStatic(data);
                case "DamageFlash":
                    return new Hit.DamageFlash(data);
            }

            if (!type.Equals(""))
                throw new Exception("Module type "+type+" not recognised.");

            return null;
        }
    }
}

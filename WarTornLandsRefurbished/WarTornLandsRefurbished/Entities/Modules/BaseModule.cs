﻿
using Microsoft.Xna.Framework;
using System.Data;
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
            string type = data["Type"].ToString();

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

                // Draw modules
                case "StaticDrawer":
                    return new Draw.StaticDrawer(data);

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

            return null;
        }
    }
}

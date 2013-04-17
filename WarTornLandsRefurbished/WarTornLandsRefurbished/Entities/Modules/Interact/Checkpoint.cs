using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.World;
using WarTornLands.Infrastructure.Systems.SaveLoad;

namespace WarTornLands.Entities.Modules.Interact
{
    class Checkpoint:BaseModule,IInteractModule
    {
        public void Interact(Entity user)
        {
            if(user is PlayerClasses.Player)
            {
                //Activate checkpoint
                GlobalState.RegisterCheckpoint(user.Position);
                user.Health = user.MaxHealth;
              //  SmartStorage<SaveGameData>.Init();
                SmartStorage<SaveGameData>.Save(0, new SaveGameData());
            }
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            return;
        }
    }
}

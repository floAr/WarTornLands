using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Infrastructure.Systems;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using WarTornLands;
using WarTornLands.PlayerClasses;

namespace WarTornLands.Infrastructure
{
   public class DialogManager //: GameComponent
   {
       #region Singleton Stuff
       private static DialogManager instance;

        public static DialogManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DialogManager();
                }
                return instance;
            }
        }
       #endregion

        private DialogManager()
        {

        }
    }
}

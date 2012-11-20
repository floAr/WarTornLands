using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using WarTornLands.Entities;

namespace WarTornLands.Infrastructure
{
    public class CollisionManager
    {
        #region Singleton Stuff
        private static CollisionManager instance;

        public static CollisionManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CollisionManager();
                }
                return instance;
            }
        }
        #endregion

        private CollisionManager()
        {

        }
    }
}

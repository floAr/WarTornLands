using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarTornLands.Infrastructure
{
   public class SoundManager
    {
         #region Singleton Stuff
        private static SoundManager instance;

        public static SoundManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SoundManager();
                }
                return instance;
            }
        }
        #endregion

        private SoundManager()
        {

        }
    }
}

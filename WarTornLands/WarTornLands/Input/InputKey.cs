using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WarTornLands
{
    interface InputKey
    {
        void Update(GameTime gt);
        void SetMode(int mode);

        int Held();
    }
}

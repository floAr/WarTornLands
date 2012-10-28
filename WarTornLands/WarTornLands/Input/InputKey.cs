using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WarTornLands
{
    public abstract class InputKey
    {
        public InputKey()
        {
        }

        public abstract void Update(GameTime gt, KeyboardState oldKeys);
        public abstract void SetMode(int mode);
        public abstract int Held();
    }
}

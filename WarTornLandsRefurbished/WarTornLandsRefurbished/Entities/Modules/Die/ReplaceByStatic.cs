using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Entities.Modules.Draw;
using Microsoft.Xna.Framework.Graphics;

namespace WarTornLands.Entities.Modules.Die
{
    class ReplaceByStatic : BaseModule, IDieModule
    {
        protected string _name;

        public ReplaceByStatic(string name)
        {
            _name = name;
        }

        public void Die()
        {
            _owner.RemoveAllModules();
            StaticDrawer sd = new StaticDrawer();
            sd.Texture = Game1.Instance.Content.Load<Texture2D>(_name);
            _owner.AddModule(sd);
        }
    }
}

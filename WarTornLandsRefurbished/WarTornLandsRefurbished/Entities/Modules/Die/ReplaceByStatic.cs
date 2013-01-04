using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Entities.Modules.Draw;
using Microsoft.Xna.Framework.Graphics;
using WarTornLands.Entities.Modules.Collide;
using System.Data;

namespace WarTornLands.Entities.Modules.Die
{
    class ReplaceByStatic : BaseModule, IDieModule
    {
        protected string _texName;

        public ReplaceByStatic(string texName)
        {
            _texName = texName;
        }

        public ReplaceByStatic(DataRow data)
            : this(data["Texture"].ToString())
        {
            
        }

        public void Die()
        {
            _owner.RemoveAllModules();
            StaticDrawer sd = new StaticDrawer();
            sd.Texture = Game1.Instance.Content.Load<Texture2D>("sprite/"+_texName);
            _owner.AddModule(new Obstacle());
            _owner.AddModule(sd);
        }
    }
}

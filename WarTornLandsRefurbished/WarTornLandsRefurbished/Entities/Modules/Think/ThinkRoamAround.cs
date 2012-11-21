using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Entities.Modules.Think;
using Microsoft.Xna.Framework;
using WarTornLands.Entities;
using WarTornLands.Infrastructure;
using WarTornLands.Entities.Modules;

namespace WarTornLandsRefurbished.Entities.Modules.Think
{
    public class ThinkRoamAround:BaseModule,IThinkModule
    {
        private Vector2 _anchor;
        private Vector2 _target;
        private float _radius;


        public ThinkRoamAround(Entity owner, Vector2 anchor, float radius):base()
        {
            _anchor = anchor;
            _target = anchor;
            _radius = radius;
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            //if(_owner.Position is close to _target)
            //  wähle neues target im radius um den anchor
            //  lasse Pfad berechnen
            //
            //Gehe einen Schritt in Richtung des nächsten targets
        }
    }
}

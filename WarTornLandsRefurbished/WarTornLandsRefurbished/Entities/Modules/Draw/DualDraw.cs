using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Data;

namespace WarTornLands.Entities.Modules.Draw
{
    class DualDraw:BaseModule,IDrawExecuter
    {
        IDrawExecuter _lower;
        IDrawExecuter _upper;

        public DualDraw(IDrawExecuter lower, IDrawExecuter upper)
        {
            _lower = lower;
            _upper = upper;
        }
        public DualDraw(DataRow data)
        {
            DataRow[] parts = data.GetChildRows("Module_Module");

            _lower = (IDrawExecuter)BaseModule.GetModule(parts[0]);
            _upper = (IDrawExecuter)BaseModule.GetModule(parts[1]);
        }
        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch, DrawInformation information)
        {
            _lower.Draw(batch, information);
            _upper.Draw(batch, information);
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            _lower.Update(gameTime);
            _upper.Update(gameTime);
        }

        public Microsoft.Xna.Framework.Vector2 Size
        {
            get { return Vector2.Max(_lower.Size, _upper.Size); }
        }
    }
}

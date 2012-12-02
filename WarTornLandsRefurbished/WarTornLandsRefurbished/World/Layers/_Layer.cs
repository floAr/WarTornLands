using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WarTornLands.Entities;

namespace WarTornLands.World.Layers
{
    public abstract class Layer : DrawableGameComponent
    {
        public Layer(int depth)
            : base(Game1.Instance)
        {
            DrawOrder = depth;
        }

        public void Add()
        {
            Game.Components.Add(this);
        }

        internal void Remove()
        {
            Game.Components.Remove(this);
        }
    }
}

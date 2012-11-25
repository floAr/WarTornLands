using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WarTornLands.World.Layers
{
    public abstract class Layer : DrawableGameComponent
    {
        protected Game _game;

        public Layer(Game game, int depth)
            : base(game)
        {
            _game = game;
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

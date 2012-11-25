using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.World.Layers;

namespace WarTornLands.World
{
    public class Area
    {
        LinkedList<Layer> _layers;

        public Area()
        {
            _layers = new LinkedList<Layer>();
        }

        public void AddLayer(Layer layer)
        {
            _layers.AddLast(layer);
        }

        /// <summary>
        /// Adds the area's layers to the Game's component list, so they will be
        /// automatically updated and drawn by the game loop.
        /// </summary>
        internal void Add()
        {
            foreach (Layer layer in _layers)
            {
                layer.Add();
            }
        }

        /// <summary>
        /// Removes the area's layers from the Game's component list, so they won't be
        /// automatically updated or drawn by the game loop.
        /// </summary>
        internal void Remove()
        {
            foreach (Layer layer in _layers)
            {
                layer.Remove();
            }
        }
    }
}

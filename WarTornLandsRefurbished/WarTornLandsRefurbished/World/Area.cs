using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.World.Layers;
using Microsoft.Xna.Framework;

namespace WarTornLandsRefurbished.World
{
    public class Area
    {
        private Game _game;
        private LinkedList<Layer> _layers;
        
        /// <summary>
        /// Create an area objects. The area is not automatically added to the
        /// Game's Component list. Use Add() or Remove() to do this.
        /// </summary>
        /// <param name="game"></param>
        public Area(Game game)
        {
            _game = game;
            _layers = new LinkedList<Layer>();
        }

        /// <summary>
        /// Add a Layer to the Area. A layer can contain either Tiles or Entities.
        /// </summary>
        /// <param name="layer"></param>
        public void AddLayer(Layer layer)
        {
            _layers.AddLast(layer);
        }

        /// <summary>
        /// Adds all layers of the area to the Game's Component list.
        /// </summary>
        public void Add()
        {
            foreach (Layer layer in _layers)
            {
                _game.Components.Add(layer);
            }
        }

        /// <summary>
        /// Removes all layers of the area from the Game's Component list.
        /// </summary>
        public void Remove()
        {
            foreach (Layer layer in _layers)
            {
                _game.Components.Remove(layer);
            }
        }
    }
}

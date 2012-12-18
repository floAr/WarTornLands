using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.World.Layers;
using Microsoft.Xna.Framework;
using WarTornLands.Entities;
using WarTornLands.Infrastructure;
using System.Data;

namespace WarTornLands.World
{
    public class Area
    {
        public Rectangle Bounds { get; private set; }
        public TileSetBox TileSets;

        List<TileLayer> _lowTileLayers;
        EntityLayer _entityLayer;
        List<TileLayer> _highTileLayers;

        public Area(Rectangle bounds)
        {
            Bounds = bounds;
            _lowTileLayers = new List<TileLayer>();
            _highTileLayers = new List<TileLayer>();

            _entityLayer = new EntityLayer();
        }



        public void AddEntityLayer(EntityLayer layer)
        {
            _entityLayer = layer;
        }

        public void AddLowLayer(TileLayer layer)
        {
            layer.SetTileSetBox(this.TileSets);

            _lowTileLayers.Add(layer);
        }

        public void AddHighLayer(TileLayer layer)
        {
            _highTileLayers.Add(layer);
        }

        private bool Contains(Vector2 position)
        {
            if (Bounds.Left * Constants.TileSize < position.X &&
                Bounds.Top * Constants.TileSize < position.Y &&
                Bounds.Right * Constants.TileSize > position.X &&
                Bounds.Bottom * Constants.TileSize > position.Y)
            {
                return true;
            }

            return false;
        }

        internal bool IsPositionAccessible(Vector2 position)
        {
            // Check whether the area even contains the pixel position
            if (!Contains(position))
            {
                return true;
            }

            Vector2 localPos = new Vector2(position.X, position.Y);

            // Iterate through layers, check all TileLayers
            foreach (Layer layer in _lowTileLayers)
            {
                if (layer is TileLayer && (layer as TileLayer).IsPositionAccessible(position) == false)
                    return false;
            }

            // Accessible if all TileLayers had empty tiles
            return true;
        }

        public List<Entity> GetEntitiesAt(Vector2 position)
        {
            List<Entity> result = new List<Entity>();

            result.AddRange(_entityLayer.GetEntitiesAt(position));

            return result;
        }

        public List<Entity> GetEntitiesAt(Vector2 position, float radius)
        {
            List<Entity> result = new List<Entity>();

            result.AddRange(_entityLayer.GetEntitiesAt(position, radius));

            return result;
        }

        internal void Update(GameTime gameTime)
        {
            foreach (Layer layer in _lowTileLayers)
            {
                layer.Update(gameTime);
            }

            _entityLayer.Update(gameTime);

            foreach (Layer layer in _highTileLayers)
            {
                layer.Update(gameTime);
            }
        }

        internal void Draw(GameTime gameTime)
        {
            foreach (Layer layer in _lowTileLayers)
            {
                layer.Draw(gameTime);
            }

            // TODO: sort Entities
            _entityLayer.Draw(gameTime);

            foreach (Layer layer in _highTileLayers)
            {
                layer.Draw(gameTime);
            }
        }
    }
}

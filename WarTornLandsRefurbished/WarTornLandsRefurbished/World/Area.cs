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
        public string Name { get; private set; }
        public string AreaID { get; private set; }
        public bool IsDungeon { get; private set; }

        List<TileLayer> _lowTileLayers;
        EntityLayer _entityLayer;
        List<TileLayer> _highTileLayers;

        public Area(Rectangle bounds, string name, string id, bool isDungeon)
        {
            Bounds = bounds;
            _lowTileLayers = new List<TileLayer>();
            _highTileLayers = new List<TileLayer>();
            Name = name;
            AreaID = id;
            IsDungeon = isDungeon;

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
            layer.SetTileSetBox(this.TileSets);

            _highTileLayers.Add(layer);
        }

        public bool Contains(Vector2 position)
        {
            Rectangle pxBounds = new Rectangle(Bounds.Location.X * Constants.TileSize, Bounds.Location.Y * Constants.TileSize,
                Bounds.Width * Constants.TileSize, Bounds.Height * Constants.TileSize);
            return pxBounds.Contains(new Point((int)Math.Round(position.X), (int)Math.Round(position.Y)));
        }

        public bool Contains(Rectangle rect)
        {
            Rectangle pxBounds = new Rectangle(Bounds.Location.X * Constants.TileSize, Bounds.Location.Y * Constants.TileSize,
               Bounds.Width * Constants.TileSize, Bounds.Height * Constants.TileSize);
            return pxBounds.Contains(rect);
        }

        internal bool IsPositionAccessible(Vector2 position)
        {
            // Check whether the area even contains the pixel position
            if (!Contains(position))
                return true;
            
            // Iterate through layers, check all TileLayers
            foreach (Layer layer in _lowTileLayers)
            {
                if (layer is TileLayer && (layer as TileLayer).IsPositionAccessible(position) == false)
                    return false;
            }

            // Accessible if all TileLayers had empty tiles
            return true;
        }

        public bool IsRectAccessible(Rectangle rect)
        {
            // Check whether the area even contains the pixel position
            if (!Contains(rect))
                return true;

            // Iterate through layers, check all TileLayers
            foreach (Layer layer in _lowTileLayers)
            {
                if (layer is TileLayer && (layer as TileLayer).IsRectAccessible(rect) == false)
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

        public List<Entity> GetEntitiesAt(Vector2 position, int radius)
        {
            List<Entity> result = new List<Entity>();

            result.AddRange(_entityLayer.GetEntitiesAt(position, radius));

            return result;
        }

        public HashSet<Entity> GetEntitiesAt(Rectangle rect)
        {
            HashSet<Entity> result = new HashSet<Entity>();

            result.UnionWith(_entityLayer.GetEntitiesAt(rect));

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

        internal List<Entity> GetAllEntities()
        {
            List<Entity> result = new List<Entity>();
            //foreach (Layer layer in _layers)
            //{
            //    if (layer is EntityLayer)
            //    {
            //        result.AddRange((layer as EntityLayer).GetAllEntities());
            //    }
            //}
            return _entityLayer.GetAllEntities();
        }
    }
}

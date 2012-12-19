using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WarTornLands.World
{
    struct TileType
    {
        public int ID;
        public float SpeedModifier;
        public string Type;
    }

    public class TileSet
    {
        /// <summary>
        /// Gets the first global ID registered in this tileset.
        /// </summary>
        /// <value>
        /// The first ID.
        /// </value>
        public int FirstID
        {
            get { return _firstID; }
        }
        /// <summary>
        /// Gets the image this tileset draws its sprites from.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        public Texture2D Image { get; private set; }
        /// <summary>
        /// Gets a Point containing the number of rows in its X, and the number of rows in its Y property.
        /// </summary>
        /// <value>
        /// The rows colls.
        /// </value>
        public Point RowsColls { get; private set; }
        /// <summary>
        /// Gets the tile dimensions.
        /// </summary>
        /// <value>
        /// The tile dimensions.
        /// </value>
        public Point TileDimensions { get; private set; }

        private int _firstID;
        private string _name;
        private List<TileType> _types;

        public TileSet(DataRow data)
        {
            _firstID = int.Parse(data["firstgid"].ToString());
            _name = data["name"].ToString();
            TileDimensions = new Point(int.Parse(data["tilewidth"].ToString()), int.Parse(data["tileheight"].ToString()));
            Image = Game1.Instance.Content.Load<Texture2D>("tileset/" + data.GetChildRows("tileset_properties")[0].GetChildRows("properties_property")[0]["value"].ToString());
            //Image = Game1.Instance.Content.Load<Texture2D>("tileset/dg_dungeon32");
            RowsColls = new Point(
                int.Parse(data.GetChildRows("tileset_image")[0]["height"].ToString()) / TileDimensions.Y,
                                int.Parse(data.GetChildRows("tileset_image")[0]["width"].ToString()) / TileDimensions.X);
            
            _types = new List<TileType>();

            foreach (DataRow typeData in data.GetChildRows("tileset_tile"))
            {
                TileType type = new TileType();
                type.ID = int.Parse(typeData["id"].ToString());

                foreach (DataRow propData in typeData.GetChildRows("tile_properties")[0].GetChildRows("properties_property"))
                {
                    if (propData["name"].ToString().Equals("Modifier"))
                    {
                        type.SpeedModifier = int.Parse(propData["value"].ToString());
                        continue;
                    }
                    if (propData["name"].ToString().Equals("Type"))
                    {
                        type.Type = propData["value"].ToString();
                        continue;
                    }

                    throw new Exception("Faulty property in tiletype with ID " + type.ID + ".");
                }

                _types.Add(type);
            }
        }

        public float ModifierOf(int gid)
        {
            foreach (TileType type in _types)
            {
                if (type.ID == gid)
                    return type.SpeedModifier;
            }

            return 1;
        }

        public string TypeOf(int gid)
        {
            foreach (TileType type in _types)
            {
                if (type.ID == gid)
                    return type.Type;
            }

            throw new Exception("It is no EntityType defined for gID " + gid + "." );
        }
    }

    public class TileSetBox
    {
        List<TileSet> _sets;

        public TileSetBox()
        {
            _sets = new List<TileSet>();
        }

        public void Add(DataRow setData)
        {
            TileSet set = new TileSet(setData);
            _sets.Add(set);
        }

        public float ModifierOf(int gid)
        {
            List<TileSet> setsReversed = new List<TileSet>(_sets);
            setsReversed.Reverse();

            foreach (TileSet set in setsReversed)
            {
                if (set.FirstID <= gid + 1)
                    return set.ModifierOf(gid - set.FirstID);
            }

            throw new Exception();
        }

        public string TypeOf(int gid)
        {
            List<TileSet> setsReversed = new List<TileSet>(_sets);
            setsReversed.Reverse();

            foreach (TileSet set in setsReversed)
            {
                if (set.FirstID <= gid)
                    return set.TypeOf(gid - set.FirstID);
            }

            throw new Exception();
        }

        public Point DimensionsOf(int gid)
        {
            List<TileSet> setsReversed = new List<TileSet>(_sets);
            setsReversed.Reverse();

            foreach (TileSet set in setsReversed)
            {
                if (set.FirstID <= gid)
                    return set.TileDimensions;
            }

            throw new Exception();
        }


        public Texture2D GetTextureAndSourceRec(int gid, out Rectangle sourceRec)
        {
           // List<TileSet> setsReversed = new List<TileSet>(_sets);
           // setsReversed.Reverse();

            for (int i=_sets.Count-1;i>=0;i--)
            {
                if (_sets[i].FirstID <= gid + 1)
                {
                    int lid = gid - _sets[i].FirstID + 1;

                    sourceRec = new Rectangle(
                            (lid % _sets[i].RowsColls.Y) * _sets[i].TileDimensions.X,
                            (lid / _sets[i].RowsColls.Y) * _sets[i].TileDimensions.Y,
                            _sets[i].TileDimensions.X,
                            _sets[i].TileDimensions.Y);

                    return _sets[i].Image;
                }
            }

            throw new Exception("Faulty tile gid");
        }
    }

}

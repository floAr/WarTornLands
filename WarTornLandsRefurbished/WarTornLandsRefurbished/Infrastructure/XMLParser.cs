using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using WarTornLands.Entities;
using Microsoft.Xna.Framework;
using WarTornLands.Entities.Modules;
using WarTornLands.World;
using WarTornLands.World.Layers;

namespace WarTornLands.Infrastructure
{
    public enum AreaVersion 
    {
        GOOD,
        EVIL,
        NEUTRAL
    }

    public class XMLParser
    {
        #region Singleton Stuff
        private static XMLParser _instance;

        public static XMLParser Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new XMLParser();
                }
                return _instance;
            }
        }
        #endregion

        private Game1 _game;

        private XMLParser()
        {
            _game = Game1.Instance;
        }

        /// <summary>
        /// Reads a safegame.
        /// Not yet implemented
        /// </summary>
        public void ReadSafegame()
        {

        }

        /// <summary>
        /// Searches the Data directory in the content project for XML files ending on _Area.xml.
        /// The Level is reseted and a new one created, containing one Area for each file.
        /// </summary>
        public void ReadWorld()
        {
            string path = _game.Content.RootDirectory + "/Data";

            IEnumerable<string> files = Directory.EnumerateFiles(path, "*_Area.xml", SearchOption.AllDirectories);

            Game1.Instance.Level.Clear();
            foreach (string file in files)
            {
                DataSet data = new DataSet();
                data.ReadXml(file);

                Area area = CreateArea(data, AreaVersion.EVIL);
                string id = data.Tables["AreaInfo"].Rows[0]["ID"].ToString();

                Game1.Instance.Level.AddArea(id, area);
            }
        }

        /// <summary>
        /// Creates an Area from a given XML DataSet.
        /// </summary>
        /// <param name="areaMeta">The data.</param>
        /// <returns></returns>
        public Area CreateArea(DataSet areaMeta, AreaVersion version)
        {
            string areaID = areaMeta.Tables["AreaInfo"].Rows[0]["ID"].ToString();
            string versionFile = "";

            if(version == AreaVersion.GOOD)
                versionFile = areaMeta.Tables["BaseInfo"].Rows[0]["Good"].ToString();
            if(version == AreaVersion.EVIL)
                versionFile = areaMeta.Tables["BaseInfo"].Rows[0]["Evil"].ToString();
            if(version == AreaVersion.NEUTRAL)
                versionFile = areaMeta.Tables["BaseInfo"].Rows[0]["Neutral"].ToString();

            #region Read BaseInfo

            DataRow info = areaMeta.Tables["BaseInfo"].Rows[0];
            DataRow posData = info.GetChildRows("BaseInfo_Position")[0];
            int width = int.Parse(info["Width"].ToString());
            int height = int.Parse(info["Height"].ToString());
            Rectangle bounds = new Rectangle(int.Parse(posData["X"].ToString()) * Constants.TileSize, int.Parse(posData["Y"].ToString()) * Constants.TileSize, width, height);

            #endregion

            #region Read TMX

            string path = _game.Content.RootDirectory + "/Data";
            IEnumerable<string> files = Directory.EnumerateFiles(path, versionFile, SearchOption.AllDirectories);

            DataSet data = new DataSet();

            foreach (string file in files)
            {
                data.ReadXml(file);
            }

            Area area = new Area(bounds);

            List<DataSet> dataCollection = ReadEntityTypes();
            Game1.Instance.Level.EntityTypeData = dataCollection;
            ReadTMX(ref area, data);

            #endregion

            return area;
        }

        private void ReadTMX(ref Area area, DataSet data)
        {
            TileSetBox tileSets = new TileSetBox();

            foreach(DataRow setData in data.Tables["tileset"].Rows)
            {
                tileSets.Add(setData);
            }

            area.TileSets = tileSets;

            #region Read TileLayers

            foreach(DataRow layerData in data.Tables["layer"].Rows)
            {
                try
                {
                    if (layerData["visible"].ToString().Equals("0"))
                        continue;
                }
                catch { }

                Point layerDims = new Point(int.Parse(layerData["width"].ToString()), int.Parse(layerData["height"].ToString()));
                int[,] tileGrid = new int[layerDims.X, layerDims.Y];

                int x = 0;
                int y = 0;

                foreach (DataRow mapData in layerData.GetChildRows("layer_data"))
                {
                    foreach (DataRow tileData in mapData.GetChildRows("data_tile"))
                    {
                        tileGrid[x, y] = int.Parse(tileData["gid"].ToString());

                        if (x < layerDims.X - 1)
                            ++x;
                        else
                        {
                            x = 0;
                            ++y;
                        }
                    }
                }

                TileLayer tileLayer = new TileLayer(tileGrid);

                foreach (DataRow properties in layerData.GetChildRows("layer_properties"))
                {
                    foreach (DataRow propertyData in properties.GetChildRows("properties_property"))
                    {
                        if (propertyData["name"].Equals("Type") || propertyData["name"].Equals("type"))
                        {
                            if (propertyData["value"].Equals("Low") || propertyData["value"].Equals("low"))
                            {
                                area.AddLowLayer(tileLayer);
                                continue;
                            }

                            if (propertyData["value"].Equals("High") || propertyData["value"].Equals("high"))
                            {
                                area.AddHighLayer(tileLayer);
                                continue;
                            }

                            throw new Exception("No or faulty 'Type' property set for tileLayer. Accepted values are 'Low' and 'High'.");
                        }
                    }
                }
            }

            #endregion

            #region Read Objectgroups

            EntityBuilder.Begin();

            foreach (DataRow groupData in data.Tables["objectgroup"].Rows)
            {
                try
                {
                    if (groupData["visible"].ToString().Equals("0"))
                        continue;
                }
                catch { }

                EntityLayer entLayer = new EntityLayer();

                foreach (DataRow objectData in groupData.GetChildRows("objectgroup_object"))
                {
                    entLayer.AddEntity(EntityBuilder.Instance.CreateEntity(area.TileSets, objectData));
                }

                area.AddEntityLayer(entLayer);
            }

            EntityBuilder.End();

            #endregion
        }

        /// <summary>
        /// Searches for a directory of the same name as the currently build Area.
        /// Creates a list containing the data for each EntityType file (XML ending on _Type.xml) found in the respective 'Types' directory.
        /// Said list is meant to be saved in the Area instance for later use.
        /// </summary>
        /// <param name="areaID">The area ID.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">More than one directory for area  + areaID +  found.</exception>
        private List<DataSet> ReadEntityTypes()
        {
            string path = _game.Content.RootDirectory + "/Data/Types";

            IEnumerable<string> files = Directory.EnumerateFiles( path, "*_Type.xml", SearchOption.AllDirectories);

            List<DataSet> dataCollection = new List<DataSet>();

            foreach (string f in files)
            {
                DataSet data = new DataSet();
                data.ReadXml(f);

                dataCollection.Add(data);
            }

            return dataCollection;
        }

        /// <summary>
        /// Searches a propertie structure for a specified propertie.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static string ValueOfProperty(DataRow data, string propertyName)
        {
            foreach (DataRow property in data.GetChildRows("properties_property"))
            {
                if (property["name"].ToString().Equals(propertyName))
                    return property["value"].ToString();
            }

            throw new Exception("Property "+ propertyName + " not found.");
        }
    }

    public class ParseErrorException : Exception
    {
        public string ID { get; private set; }
        public string Attribute { get; private set; }

        public ParseErrorException(string id, string attribute)
            : base("Error in EntityType " + id + " in attribute " + attribute + ".")
        {
            ID = id;
            Attribute = attribute;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Entities;
using System.Data;
using Microsoft.Xna.Framework;
using WarTornLands.World;
using WarTornLands.Entities.Modules.Interact;
using WarTornLands.Infrastructure.Systems.DialogSystem;
using WarTornLands.PlayerClasses.Items;
using WarTornLands.Entities.Modules;
using WarTornLands.Entities.AI;
using WarTornLands.Entities.Modules.Collide;

namespace WarTornLands.Infrastructure
{
    /// <summary>
    /// This class provides helper methods for the XML parser to encapsulate extensive Entity constructing routines.
    /// </summary>
    class EntityBuilder
    {
        #region Singleton Stuff
        private static EntityBuilder _instance;

        public static EntityBuilder Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new Exception("Begin must be called before the EntityBuilder can be accessed.");
                }
                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// All Entities created are kept reference of until end is called.
        /// This is done to be able to link RoamingRectangles and such items to the Entities.
        /// </summary>
        private List<ZoneVeil> _zoneContainer;
        private List<Entity> _jumpPoints;
        public Area CurrentArea { get; private set; }

        private EntityBuilder()
        {
            _zoneContainer = new List<ZoneVeil>();
            _jumpPoints = new List<Entity>();
        }

        public static void Begin()
        {
            _instance = new EntityBuilder();
        }

        public static void BeginArea(Area area)
        {
            _instance.CurrentArea = area;
        }

        public static void EndArea()
        {
            _instance.CurrentArea = null;
            _instance._zoneContainer.Clear();
        }

        public static void End()
        {
            _instance.ConnectJumpPoints();

            _instance = null;
        }

        /// <summary>
        /// Creates an Entity from a given XML DataSet.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        private Entity CreateEntity(DataSet data)
        {
            string typeID = data.Tables["TypeInfo"].Rows[0]["ID"].ToString();
            string typeCategorie = data.Tables["TypeInfo"].Rows[0]["Categorie"].ToString();

            List<string> errors = new List<string>();

            #region Read BaseInfo
            DataRow row = data.Tables["BaseInfo"].Rows[0];

            String name = row["Name"].ToString();
            name.Trim();
            if (name.Equals(""))
                errors.Add("Name");

            int health = int.Parse(row["Health"].ToString());
            int bodyHeight = int.Parse(row["BodyHeight"].ToString());
            #endregion

            Entity ent = new Entity(Vector2.Zero, name, health, bodyHeight);
            ent.Categorie = typeCategorie;

            #region Read Modules

            foreach (DataRow r in data.Tables["Module"].Rows)
            {
                ent.AddModule(BaseModule.GetModule(r));
            }

            #endregion

            return ent;
        }
        /// <summary>
        /// Creates an Entity of the given Type.
        /// </summary>
        /// <param name="area">The area.</param>
        /// <param name="entityType">The EntityType.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">EntityType +entityType+ not found.</exception>
        private Entity CreateEntity(string entityType)
        {
            foreach (DataSet r in Game1.Instance.Level.EntityTypeData)
            {
                if (r.Tables["TypeInfo"].Rows[0]["ID"].ToString().Equals(entityType))
                    return CreateEntity(r);
            }
            throw new Exception("EntityType " + entityType + " not found.");
        }

        public Entity CreateEntity(TileSetBox tilesetBox, DataRow data)
        {
            Entity entity = CreateEntity(
                    tilesetBox.TypeOf(
                        int.Parse(
                            data["gid"].ToString()
                               )));

            entity.Position = new Vector2(int.Parse(data["x"].ToString()) + tilesetBox.DimensionsOf(int.Parse(data["gid"].ToString())).X * .5f,
                                       int.Parse(data["y"].ToString()));


            // Handle special types
            #region Chest
            //Creates a standard Dialog providing the item specified in the Tiled editor

            if (entity.Categorie.Equals("Chest"))
            {
                Conversation con = new Conversation("default");

                //try
                //{
                    con.Add(new ItemContainer(
                        SwitchItemType(data.GetChildRows("object_properties")[0])));
                //}
                //catch { throw new Exception("A Chest is missing an Item."); }
                con.Add(new KillSpeaker());

                List<Conversation> cons = new List<Conversation>();
                cons.Add(con);

                entity.AddModule(new Dialog(cons));
            }
            #endregion
            #region Door
            if (entity.Categorie.Equals("Door"))
            {
                entity.AddModule(new OpenDoorOnCollide(CurrentArea.AreaID));
            }
            #endregion
            #region JumpPoint
            if (entity.Categorie.Equals("JumpPoint"))
            {
                DataRow jumpProps = data.GetChildRows("object_properties")[0];

                entity.AddModule(new JumpPoint(
                    XMLParser.ValueOfProperty(jumpProps, "ID"),
                    XMLParser.ValueOfProperty(jumpProps, "Target")));

                _jumpPoints.Add(entity);
            }
            #endregion
            #region Check for demanded zone
            try
            {
                string zoneID = XMLParser.ValueOfProperty(data.GetChildRows("object_properties")[0], "ZoneID");
                // Check if the entity relates to a Zone
                if (zoneID != null)
                {
                    entity.SetZone(FindMatch(int.Parse(zoneID)));
                }
            }
            catch { }
            #endregion

            return entity;
        }

        private void ConnectJumpPoints()
        {
            foreach (Entity j in _jumpPoints)
            {
                foreach(Entity t in _jumpPoints)
                {
                    if ((t.CollideModule as JumpPoint).JumpID.Equals(
                        (j.CollideModule as JumpPoint).TargetID))
                    {
                        (j.CollideModule as JumpPoint).Connect(t);
                    }
                }
            }
        }

        private Item SwitchItemType(DataRow data)
        {
            // Register new Itemtypes here
            switch(XMLParser.ValueOfProperty(data, "Item"))
            {
                case "DoorKey":
                    return new DoorKey(CurrentArea.AreaID);
                case "MasterKey":
                    return new MasterKey(CurrentArea.AreaID);
            }

            return Item.Nothing;
        }

        public void DepositZone(int id, Zone zone)
        {
            ZoneVeil veil = new ZoneVeil();
            veil.ID = id;
            veil.Zone = zone;

            _zoneContainer.Add(veil);
        }

        private Zone FindMatch(int id)
        {
            foreach (ZoneVeil veil in _zoneContainer)
            {
                if (id == veil.ID)
                    return veil.Zone;
            }

            throw new Exception("No Zone with ID "+id+" found.");
        }
    }

    struct ZoneVeil
    {
        public int ID;
        public Zone Zone;
    }
}

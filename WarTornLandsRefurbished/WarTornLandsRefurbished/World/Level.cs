using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.World.Layers;
using Microsoft.Xna.Framework;
using WarTornLands;
using WarTornLands.Entities;
using System.Xml;
using WarTornLands.Entities.Modules.Draw;
using Microsoft.Xna.Framework.Graphics;
using WarTornLands.Entities.Modules.Die;
using WarTornLands.PlayerClasses;
using WarTornLands.Entities.Modules.Interact;
using WarTornLands.Infrastructure.Systems.DialogSystem;
using System.Xml.Linq;
using WarTornLands.Infrastructure;
using WarTornLands.Entities.Modules.Collide;
using WarTornLands.Entities.Modules.Draw.ParticleSystem;
using WarTornLands.Infrastructure.Systems.SkyLight;
using WarTornLandsRefurbished.Entities.Modules.Think;
using WarTornLands.PlayerClasses.Items;

namespace WarTornLands.World
{
    public class Level
    {
        private Dictionary<string, Area> _areas;
        private Game _game;

        private Random r = new Random();
        public Level(Game game)
        {
            _areas = new Dictionary<string, Area>();
            _game = game;
        }

        public bool AddArea(string name, Area area)
        {
            if (!_areas.ContainsKey(name))
            {
                _areas.Add(name, area);
                area.Add();
                return true;
            }

            return false;
        }

        public bool RemoveArea(string name)
        {
            if (_areas.ContainsKey(name))
            {
                _areas[name].Remove();
                _areas.Remove(name);
                return true;
            }

            return false;
        }

        public static Entity Ute;

        /// <summary>
        /// Queries the Tile Map whether a given pixel position is accessible.
        /// Entities are not considered in the collision check. This is done
        /// via GetEntitiesAt in the CollisionManager.
        /// </summary>
        /// <param name="position">Pixel position to check.</param>
        /// <returns></returns>
        public bool IsPositionAccessible(Vector2 position)
        {
            // TODO only check areas near the player
            foreach (KeyValuePair<string, Area> pair in _areas)
            {
                if (pair.Value.IsPositionAccessible(position) == false)
                    return false;
            }

            return true;
        }

        public List<Entity> GetEntitiesAt(Vector2 position)
        {
            List<Entity> result = new List<Entity>();

            // TODO only check areas near the player
            foreach (KeyValuePair<string, Area> pair in _areas)
            {
                result.AddRange(pair.Value.GetEntitiesAt(position));
            }

            if ((Player.Instance.Position - position).Length() < 40)
                result.Add(Player.Instance);

            return result;
        }

        public List<Entity> GetEntitiesAt(Vector2 position, float radius)
        {
            List<Entity> result = new List<Entity>();

            // TODO only check areas near the player
            foreach (KeyValuePair<string, Area> pair in _areas)
            {
                result.AddRange(pair.Value.GetEntitiesAt(position, radius));
            }

            return result;
        }

        /// <summary>
        /// Loads a test level.
        /// </summary>
        public void LoadTestLevel()
        {
            Area area1 = new Area(new Rectangle(0, 0, 10, 10));

            TileLayer layer1 = new TileLayer(0);
            Tile[,] grid1 = new Tile[10, 10];
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    if (x == 3 || y == 5)
                        grid1[x, y].TileNum = 3;
                    else
                        grid1[x, y].TileNum = 2;
                }
            }
            grid1[0, 0].TileNum = 0;
            layer1.LoadGrid(grid1, false, "grass", false);
            area1.AddLayer(layer1);

            TileLayer layer2 = new TileLayer(90);
            Tile[,] grid2 = new Tile[10, 10];
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    grid2[x, y].TileNum = 0;
                }
            }
            grid2[3, 3].TileNum = 6;
            grid2[4, 3].TileNum = 6;
            grid2[5, 3].TileNum = 6;
            grid2[5, 4].TileNum = 6;
            grid2[5, 5].TileNum = 6;
            grid2[4, 5].TileNum = 6;
            grid2[3, 5].TileNum = 6;

            layer2.LoadGrid(grid2, false, "grass", true);
            area1.AddLayer(layer2);

            EntityLayer layer3 = new EntityLayer(99);
            StaticDrawer sd = new StaticDrawer();
            sd.Texture = Game1.Instance.Content.Load<Texture2D>("Schatztruhe");
            Entity staticTest = new Entity(Game1.Instance, new Vector2(50, 50), "loch");
            staticTest.AddModule(sd);
            //staticTest.AddModule(new ExplodeAndLoot(Item.Potion));
            staticTest.Health = 100;
            layer3.AddEntity(staticTest);
            area1.AddLayer(layer3);

            StaticDrawer dialogTestDrawer = new StaticDrawer();
            dialogTestDrawer.Texture = Game1.Instance.Content.Load<Texture2D>("Schatztruhe");
            Entity dialogTest = new Entity(Game1.Instance, new Vector2(-20, 150), "chest");
            dialogTest.AddModule(dialogTestDrawer);

            List<Conversation> cons = new List<Conversation>();
            Conversation con = new Conversation("1");
            con.Add(new TextLine("Once upon a time there was a man named Jason"));
            con.Add(new TextLine("He lived in a camp near his place of death, Crystal~Lake"));
            con.Add(new TextLine("and he wore a Hockey~Mask."));
            con.Add(new TextLine("From time to time Jason went out to kill random people with his machete"));
            con.Add(new TextLine("The end."));
            List<Option> ops = new List<Option>();
            ops.Add(new Option("option1", "#this"));
            ops.Add(new Option("option2", "#this"));
            ops.Add(new Option("option3", "#this"));
            ops.Add(new Option("option4", "#this"));
            Options op = new Options(ops);
            con.Add(op);
            cons.Add(con);
            dialogTest.AddModule(new Dialog(cons, dialogTest));
            layer3.AddEntity(dialogTest);

            //torch
            List<Texture2D> pL = new List<Texture2D>();
            pL.Add(Game1.Instance.Content.Load<Texture2D>("flame3"));
            Entity torch = new Entity(Game1.Instance, new Vector2(50, 150), "torch");
            ParticleSystem pSystem = new ParticleSystem(
                new EmitterSetting()
                {
                    DirectionX = new Range() { Min = -1, Max = 1 },
                    DirectionY = new Range() { Min = -1, Max = -3 },
                    AnglePermutation = new Range() { Min = -1, Max = 1 },
                    Lifetime = new Range() { Min = 1000, Max = 1500 },
                    MaxParticles = new Range(150),
                    Size = new Range() { Min = 0.1f, Max = 0.3f },
                    SpeedX = new Range() { Min = -1, Max = 1 },
                    SpeedY = new Range() { Min = -0.5f, Max = -1.5f },
                    Alpha = new Range(1),
                    AlphaDecay = new Range(0.01f, 0.1f)

                },
        pL);
            StaticDrawer torchlight = new StaticDrawer();
            torchlight.IsLight = true;

            torchlight.Texture = Game1.Instance.Content.Load<Texture2D>("flame3");

            torch.AddModule(new DualDraw(torchlight, pSystem));
            //       torch.AddModule(pSystem);
            layer3.AddEntity(torch);
            Lightmanager.AddLight(torch);
            //endtorch

            AddArea("Entenhausen", area1);
        }

        /// <summary>
        /// Loads the test level for the X-Mas Party on December 6th, 2012.
        /// </summary>
        public void LoadChristmasCaverns()
        {
            SoundManager.Instance.PlaySong("69_spirit_temple");
            // Floor tiles
            const int STONE_FLOOR = 33; //was 49
            const int BOSS_FLOOR = 43;
            // Wall tiles
            const int STONE_WALL = 28;//was 28

            // Create an empty area in the right size
            Area cavernsArea = new Area(new Rectangle(0, 0, 56, 46));

            #region Add a floor layer
            TileLayer floorLayer = new TileLayer(0);
            Tile[,] floorGrid = new Tile[56, 46];

            // Set normal floor
            for (int x = 0; x < 56; ++x)
                for (int y = 0; y < 46; ++y)
                    floorGrid[x, y].TileNum = STONE_FLOOR;

            // Set boss floor
            for (int x = 34; x <= 43; ++x)
                for (int y = 10; y <= 17; ++y)
                    floorGrid[x, y].TileNum = BOSS_FLOOR;

            floorLayer.LoadGrid(floorGrid, false, "dg_grounds32", false);
            cavernsArea.AddLayer(floorLayer);
            #endregion

            #region Add a wall layer
            TileLayer wallLayer = new TileLayer(50);
            Tile[,] wallGrid = new Tile[56, 46];

            // Set walls EVWERYWHERE!
            for (int x = 0; x < 56; ++x)
                for (int y = 0; y < 46; ++y)
                    wallGrid[x, y].TileNum = STONE_WALL;

            // Scoop out rooms and shit
            for (int x = 12; x <= 21; ++x)
                for (int y = 14; y <= 21; ++y)
                    wallGrid[x, y].TileNum = 0;
            for (int x = 20; x <= 21; ++x)
                for (int y = 22; y <= 29; ++y)
                    wallGrid[x, y].TileNum = 0;
            for (int x = 22; x <= 39; ++x)
                for (int y = 28; y <= 29; ++y)
                    wallGrid[x, y].TileNum = 0;
            for (int x = 30; x <= 31; ++x)
                for (int y = 30; y <= 31; ++y)
                    wallGrid[x, y].TileNum = 0;
            for (int x = 28; x <= 33; ++x)
                for (int y = 32; y <= 35; ++y)
                    wallGrid[x, y].TileNum = 0;
            for (int x = 38; x <= 39; ++x)
                for (int y = 18; y <= 27; ++y)
                    wallGrid[x, y].TileNum = 0;
            for (int x = 38; x <= 39; ++x)
                for (int y = 18; y <= 27; ++y)
                    wallGrid[x, y].TileNum = 0;
            for (int x = 34; x <= 43; ++x)
                for (int y = 10; y <= 17; ++y)
                    wallGrid[x, y].TileNum = 0;

            wallLayer.LoadGrid(wallGrid, false, "dg_dungeon32", true);
            cavernsArea.AddLayer(wallLayer);
            #endregion

             #region Beautify level
            // I want to black out every wall which is not attached to a floor. we will assume a wall height of 2 tiles
            int wallheight = 2;
            TileLayer overlayer = new TileLayer(52);
            Tile[,] mastergrid = new Tile[56, 46];
            for (int x = 0; x < 56; ++x)
            {
                for (int y = 0; y < 46; ++y)
                {
                    if (wallGrid[x, y].TileNum != 0)//there shall be a wall
                    {
                        if (x == 0 || x == 55 || y == 0 || y == 45)
                        {
                            mastergrid[x, y].TileNum = 9;
                            continue;
                        }
                        if (wallGrid[x + 1, y + 1].TileNum != 0 &&
                            wallGrid[x + 1, y].TileNum != 0 &&
                            wallGrid[x + 1, y - 1].TileNum != 0 &&
                            wallGrid[x, y + 1].TileNum != 0 &&
                            wallGrid[x, y - 1].TileNum != 0 &&
                            wallGrid[x - 1, y + 1].TileNum != 0 &&
                            wallGrid[x - 1, y].TileNum != 0 &&
                            wallGrid[x - 1, y - 1].TileNum != 0)//we are surrounded by walls
                        {
                            if (x == 13 && y == 12)
                                Console.WriteLine("derp");
                            bool isWallPiece = false;
                            for (int i = 0; i <= wallheight; ++i)
                            {
                                if (y >= 45 - wallheight)
                                {
                                    continue;
                                }
                                isWallPiece |= wallGrid[x - 1, y + i].TileNum == 0 || wallGrid[x, y + i].TileNum == 0 || wallGrid[x + 1, y + i].TileNum == 0;
                                if (isWallPiece)
                                    break;
                            }
                            if (!isWallPiece)
                                mastergrid[x, y].TileNum = 9;
                        }
                    }
                }
            }
            for (int x = 0; x < 56; ++x)
            {
                for (int y = 0; y < 46; ++y)
                {

                    if (mastergrid[x, y].TileNum != 0)//there shall be blackout
                    {
                        if (x == 0 || x == 55 || y == 0 || y == 45)
                        {
                            continue;
                        }

                        if (mastergrid[x, y + 1].TileNum == 0 || mastergrid[x, y + 1].TileNum > 9)
                        {
                            mastergrid[x, y].TileNum = 1;
                            continue;
                        }
                        if (mastergrid[x - 1, y].TileNum == 0 || mastergrid[x-1, y ].TileNum >9)
                        {
                            mastergrid[x, y].TileNum = 2;
                            continue;
                        }
                        if (mastergrid[x, y - 1].TileNum == 0 || mastergrid[x, y - 1].TileNum >9)
                        {
                            mastergrid[x, y ].TileNum = 3;
                            continue;
                        }
                        if (mastergrid[x + 1, y].TileNum == 0 || mastergrid[x + 1, y].TileNum >9)
                        {
                            mastergrid[x, y].TileNum = 4;
                            continue;
                        }
                        if ((mastergrid[x - 1, y].TileNum == 0 && mastergrid[x, y + 1].TileNum == 0 && mastergrid[x-1, y + 1].TileNum == 0)||
                            (mastergrid[x - 1, y].TileNum >9 && mastergrid[x, y + 1].TileNum >9 && mastergrid[x-1, y + 1].TileNum >9))
                        {
                            mastergrid[x, y].TileNum = 5;
                            continue;
                        }
                        if ((mastergrid[x - 1, y].TileNum == 0 && mastergrid[x, y - 1].TileNum == 0 && mastergrid[x - 1, y - 1].TileNum == 0)||
                        (mastergrid[x - 1, y].TileNum >9 && mastergrid[x, y - 1].TileNum >9 && mastergrid[x - 1, y - 1].TileNum >9))
                        {
                            mastergrid[x, y].TileNum = 6;
                            continue;
                        }
                        if ((mastergrid[x + 1, y ].TileNum == 0 && mastergrid[x , y - 1].TileNum == 0 && mastergrid[x + 1, y - 1].TileNum == 0)||
                            (mastergrid[x + 1, y ].TileNum >9 && mastergrid[x , y - 1].TileNum >9 && mastergrid[x + 1, y - 1].TileNum >9))
                        {
                            mastergrid[x, y].TileNum = 7;
                            continue;
                        }
                        if ((mastergrid[x , y + 1].TileNum == 0 && mastergrid[x + 1, y].TileNum == 0 && mastergrid[x + 1, y + 1].TileNum == 0)||
                            (mastergrid[x , y + 1].TileNum >9 && mastergrid[x + 1, y].TileNum >9 && mastergrid[x + 1, y + 1].TileNum >9))
                        {
                            mastergrid[x, y].TileNum = 8;
                            continue;
                        }

                    }
                    else //adding some magic
                    {
                        if (wallGrid[x, y].TileNum != 0)//we are talking about a wall
                        {
                            if (r.Next(100) > 60)
                                //3rd row overlay
                                mastergrid[x, y].TileNum = 19 + r.Next(9);
                        }
                        else//floor is here to come
                        {
                            if (r.Next(100) > 85)
                            {
                                //2nd row                         
                                mastergrid[x, y].TileNum = 10 + r.Next(9);
                            }
                        }
                    }
                }
            }


            overlayer.LoadGrid(mastergrid, false, "overlay", false);
            cavernsArea.AddLayer(overlayer);
            #endregion
            // Add entities
            EntityLayer entityLayer = new EntityLayer(90);

            // Normal door
            Entity door1 = new Entity(Game1.Instance, new Vector2(31, 31) * Constants.TileSize);
            List<Conversation> doorConList = new List<Conversation>();
            Conversation doorCon = new Conversation("1");
           // doorCon.Add(new TextLine("Ich brauche einen kleinen Schluessel."));
            doorCon.Add(new TextLineAndEvent("Ich brauche einen kleinen Schluessel.", new Action(delegate()
            {
                List<Vector2> points = new List<Vector2>();
                points.Add(Game1.Instance.Player.Position);
                points.Add(new Vector2(574, 500));
                points.Add(new Vector2(574, 500));
                points.Add(Game1.Instance.Player.Position);
                Game1.Instance.Camera.PlayCinematic(points);
            })));
            doorConList.Add(doorCon);
            door1.AddModule(new Dialog(doorConList, door1));            
            StaticDrawer sd1 = new StaticDrawer();
            sd1.Texture = Game1.Instance.Content.Load<Texture2D>("doorClosed");
            door1.AddModule(sd1);
            OpenDoorOnCollide d1coll = new OpenDoorOnCollide((int)ItemTypes.SmallKey);
            door1.AddModule(d1coll);
            entityLayer.AddEntity(door1);

            // Boss door
            Entity door2 = new Entity(Game1.Instance, new Vector2(39, 27) * Constants.TileSize);
            doorConList = new List<Conversation>();
            doorCon = new Conversation("1");
            //doorCon.Add(new TextLine("Ich brauche einen Riesenschluessel!"));
            doorCon.Add(new TextLineAndEvent("Ich brauche einen Riesenschluessel!", new Action(delegate()
            {
                List<Vector2> points = new List<Vector2>();
                points.Add(Game1.Instance.Player.Position);
                points.Add(new Vector2(994, 1099));
                points.Add(new Vector2(994, 1099));
                points.Add(Game1.Instance.Player.Position);
                Game1.Instance.Camera.PlayCinematic(points);
            })));
            doorConList.Add(doorCon);
            door2.AddModule(new Dialog(doorConList, door2));       
            StaticDrawer sd2 = new StaticDrawer();
            sd2.Texture = Game1.Instance.Content.Load<Texture2D>("doorClosedBoss");
            door2.AddModule(sd2);
            OpenDoorOnCollide d2coll = new OpenDoorOnCollide((int)ItemTypes.MasterKey);
            door2.AddModule(d2coll);
            entityLayer.AddEntity(door2);

            // Add chest
            Entity chest = new Entity(Game1.Instance, new Vector2(31, 35) * Constants.TileSize);
            chest.AddModule(new Obstacle());
            chest.AddModule(new ReplaceByStatic("treasureChestLooted"));
            StaticDrawer sd3 = new StaticDrawer();
            sd3.Texture = Game1.Instance.Content.Load<Texture2D>("treasureChest");
            chest.AddModule(sd3);
            List<Conversation> cons = new List<Conversation>();
            Conversation con = new Conversation("1");
            Item key = new Item(ItemTypes.MasterKey);
            List<Item> items = new List<Item>();
            items.Add(key);
            con.Add(new ItemContainer(items));
            con.Add(new KillSpeaker());
            cons.Add(con);
            chest.AddModule(new Dialog(cons, chest));
            entityLayer.AddEntity(chest);

            // Add crazy dude
            Entity crazyDude = new Entity(Game1.Instance, new Vector2(18 * Constants.TileSize, 15 * Constants.TileSize + 10));
            crazyDude.AddModule(new Obstacle());
            StaticDrawer sd4 = new StaticDrawer();
            sd4.Texture = Game1.Instance.Content.Load<Texture2D>("frederik");
            crazyDude.AddModule(sd4);
            crazyDude.AddModule(new Obstacle());
            cons = new List<Conversation>();
            // First conversation
            con = new Conversation("1");
            con.Add(new TextLine("Piep Piep!! Hast du Kaese?"));
            con.Add(new TextLine("Ich bin's, Frederik! Piep! Die boese Gruselute hat mich verhext."));
            con.Add(new TextLine("Hier, nimm diesen Schluessel und hau ihr eins vor'n Koffer!"));
            key = new Item(ItemTypes.SmallKey);
            items = new List<Item>();
            items.Add(key);
            con.Add(new ItemContainer(items));
            con.Add(new ComboBreaker("2"));
            cons.Add(con);
            // Second conversation
            con = new Conversation("2");
            con.Add(new TextLine("Die alte Gruselute schaffst du mit links!"));
            cons.Add(con);

            crazyDude.AddModule(new Dialog(cons, crazyDude));
            entityLayer.AddEntity(crazyDude);

            cavernsArea.AddLayer(entityLayer);

            // Boss
            Entity boss = new Entity(Game1.Instance, new Vector2(39, 15) * Constants.TileSize, "GruselUte");
            boss.AddModule(new ThinkRoamAround(new Vector2(39, 11) * Constants.TileSize, 170));
            StaticDrawer bossDrawer = new StaticDrawer();
            bossDrawer.Texture = Game1.Instance.Content.Load<Texture2D>("gruselute");
            boss.AddModule(bossDrawer);
            boss.AddModule(new Obstacle());
            boss.AddModule(new ExplodeAndLoot(null));
            entityLayer.AddEntity(boss);
            Ute = boss;

            //burp torch

            for (int i = 0; i < 5; ++i)
            {
                AnimatedDrawer body = new AnimatedDrawer(Game1.Instance.Content.Load<Texture2D>("torch_model"));
                Animation simpleflicker = new Animation("flicker");
                simpleflicker.AddFrame(new Rectangle(0, 0, 32, 32));
                simpleflicker.AddFrame(new Rectangle(32, 0, 32, 32));
                body.AddAnimation(simpleflicker);
                body.SetCurrentAnimation("flicker");
                AnimatedDrawer light = new AnimatedDrawer(Game1.Instance.Content.Load<Texture2D>("torch_light"));
                light.AddAnimation(simpleflicker);
                light.SetCurrentAnimation("flicker");
                light.IsLight = true;
                Entity newTorch = new Entity(Game1.Instance, new Vector2(24, 27) * Constants.TileSize + new Vector2(100 * i, 0), "torchi");
                newTorch.AddModule(new DualDraw(body, light));
                Lightmanager.AddLight(newTorch);
                entityLayer.AddEntity(newTorch);
            }


            //fire
            List<Texture2D> pL = new List<Texture2D>();
            pL.Add(Game1.Instance.Content.Load<Texture2D>("flame3"));
            Entity torch = new Entity(Game1.Instance, new Vector2(29, 32) * Constants.TileSize, "torch");
            Entity torch2 = new Entity(Game1.Instance, new Vector2(33, 32) * Constants.TileSize, "torch");
            ParticleSystem pSystem = new ParticleSystem(
                new EmitterSetting()
                {
                    DirectionX = new Range() { Min = -1, Max = 1 },
                    DirectionY = new Range() { Min = -1, Max = -3 },
                    AnglePermutation = new Range() { Min = -1, Max = 1 },
                    Lifetime = new Range() { Min = 1000, Max = 1500 },
                    MaxParticles = new Range(150),
                    Size = new Range() { Min = 0.1f, Max = 0.3f },
                    SpeedX = new Range() { Min = -1, Max = 1 },
                    SpeedY = new Range() { Min = -0.5f, Max = -1.5f },
                    Alpha = new Range(1),
                    AlphaDecay = new Range(0.01f, 0.1f)

                },
            pL);
            StaticDrawer torchlight = new StaticDrawer();
            torchlight.IsLight = true;

            torchlight.Texture = Game1.Instance.Content.Load<Texture2D>("flame3");

            torch.AddModule(new DualDraw(torchlight, pSystem));
            torch2.AddModule(new DualDraw(torchlight, pSystem));
            //       torch.AddModule(pSystem);
            entityLayer.AddEntity(torch);
            Lightmanager.AddLight(torch);
            entityLayer.AddEntity(torch2);
            Lightmanager.AddLight(torch2);
            //endtorch
            //fungus
            StaticDrawer fungusS = new StaticDrawer();
            fungusS.Texture = Game1.Instance.Content.Load<Texture2D>("fungus");
            
          

            for (int i = 0; i < 3; ++i)
            {
                Entity fungus = new Entity(Game1.Instance, new Vector2(13 + r.Next(9), 15 + r.Next(7)) * Constants.TileSize, "fungus");
                AnimatedDrawer fungusGlow = new AnimatedDrawer(Game1.Instance.Content.Load<Texture2D>("fungus_light"));
                Animation glow = new Animation("glow");
                glow.AddFrame(new Rectangle(64, 0, 64, 64), r.Next(1000));
                glow.AddFrame(new Rectangle(64, 0, 64, 64));
                glow.AddFrame(new Rectangle(64, 0, 64, 64));
                glow.AddFrame(new Rectangle(64, 0, 64, 64));
                glow.AddFrame(new Rectangle(64, 0, 64, 64));
                glow.AddFrame(new Rectangle(0, 0, 64, 64), r.Next(1000));
                glow.AddFrame(new Rectangle(0, 0, 64, 64));
                fungusGlow.AddAnimation(glow,r.Next(1000));
                fungusGlow.SetCurrentAnimation("glow");
                fungusGlow.IsLight = true;
                fungus.AddModule(new DualDraw(fungusS, fungusGlow));
                entityLayer.AddEntity(fungus);
                Lightmanager.AddLight(fungus);
            }

       
            //endfungus
            // Add area to level
            AddArea("ChristmasCaverns", cavernsArea);
        }

        public void LoadLevel(string fileName)
        {
            // TODO TODO TODO TODO TODO TODO TODO TODO
            // TODO use XDocument because XmlTextReader sucks balls

            XmlTextReader reader = new XmlTextReader(fileName);
            reader.ReadToFollowing("world");
            reader = (XmlTextReader)reader.ReadSubtree();

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element &&
                    reader.Name == "area")
                {
                    string isbn = reader.GetAttribute("ISBN");
                }
            }
        }


    }
}

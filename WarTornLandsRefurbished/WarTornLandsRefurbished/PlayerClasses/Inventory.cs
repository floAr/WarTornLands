using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using WarTornLands.Infrastructure;
using Microsoft.Xna.Framework;
using WarTornLands.PlayerClasses.Items;
using WarTornLands.Infrastructure.Systems.DrawSystem;
using WarTornLands.World;

namespace WarTornLands.PlayerClasses
{
    public class Inventory
    {
        #region Drawvariablen

        private bool _inventoryIsOpen;
        private double _standardHeight = 480;
        private double _standardWidth = 800;
        private double _deltaHeight;
        private double _deltaWidth;
        private int _radius;


        private Texture2D _chestPicture;
        private Texture2D _potionPicture;
        private Texture2D _keyPicture;
        private Texture2D _bosskeyPicture;
        private Texture2D _itemPicture;


        #endregion

        #region Itemvariables

        private PotionBag _potionBag;
        private IceHammer _iceHammer;
        private Gun _gun;
        private Telekinesis _telekinesis;
        
        private KeyStash _keys;
        public HammerStash Hammer { get; private set; }

        #endregion

        private Item _activeSlot = null;

        public IDrawProvider Drawer
        {
            get;
            private set;
        }

        public short GetNormalKeys
        {
            get
            {
                bool dungeonFound = false;
                int count = 0;

                foreach (Area a in Game1.Instance.Level.GetCurrentAreas())
                {
                    if (a.IsDungeon)
                    {
                        //if (dungeonFound)
                        //    throw new Exception("Two Dungeons should not be overlapping.");
                        
                        count = _keys.NormalCount(a.AreaID);
                        dungeonFound = true;
                    }
                }

                return (short)count;
            }
        }

        public short GetMasterKeys
        {
            get
            {
                bool dungeonFound = false;  // Used to check if two dungeons are overlapping in the world
                int count = 0;

                foreach (Area a in Game1.Instance.Level.GetCurrentAreas())
                {
                    if (a.IsDungeon)
                    {
                        //if (dungeonFound)
                        //    throw new Exception("Two Dungeons should not be overlapping.");

                        count = _keys.MasterCount(a.AreaID);
                        dungeonFound = true;
                    }
                }

                return (short)count;
            }
        }

        public bool HasPotionbag
        {
            get;
            set;
        }

        public bool HasTelekinesis
        {
            get;
            set;
        }

        public bool HasIceHammer
        {
            get;
            set;
        }

        public bool HasGun
        {
            get;
            set;
        }

        public Inventory(WarTornLands.Entities.Modules.Think.Parts.SwingHitAbility swing)
        {
            _deltaWidth = (Game1.Instance.Window.ClientBounds.Width / _standardWidth);
            _deltaHeight = (Game1.Instance.Window.ClientBounds.Height / _standardHeight);
            _chestPicture = Game1.Instance.Content.Load<Texture2D>("sprite/treasureChest");
            _potionPicture = Game1.Instance.Content.Load<Texture2D>("sprite/potion");
            _keyPicture = Game1.Instance.Content.Load<Texture2D>("sprite/key");
            _bosskeyPicture = Game1.Instance.Content.Load<Texture2D>("sprite/bosskey");
            _radius = 100;
            _inventoryIsOpen = false;
            _inventoryIsOpen = false;
            _keys = new KeyStash();
            Hammer = new HammerStash(swing);

            Drawer = new InventoryDrawer(this);
        }

        public bool Insert(Item item)
        {
            #region Key
            if (item is DoorKey)
            {
                _keys.AddKey((item as DoorKey).AreaID);
                return true;
            }
            #endregion
            #region MasterKey
            if (item is MasterKey)
            {
                _keys.AddMasterKey((item as MasterKey).AreaID);
                return true;
            }
            #endregion
            #region NoneHammer
            if (item is NoneHammer)
                return Hammer.SetNone();
            #endregion
            #region NormalHammer
            if (item is NormalHammer)
            {
                Hammer.Add(item as NormalHammer);
                return true;
            }
            #endregion
            #region ChainHammer
            if (item is ChainHammer)
            {
                Hammer.Add(item as ChainHammer);
                return true;
            }
            #endregion
            
            return false;
        }

        public void UseItem()
        {
            if (_activeSlot != null)
            {
                _activeSlot.Use();
            }
        }

        public void AddKeyShelf(string areaID)
        {
            _keys.AddShelf(areaID);
            _keys.AddMasterShelf(areaID);
        }

        public bool HasKey(string areaID)
        {
            return _keys.UseKey(areaID);
        }

        public bool HasMasterKey(string areaID)
        {
            return _keys.UseMaster(areaID);
        }
    }
}
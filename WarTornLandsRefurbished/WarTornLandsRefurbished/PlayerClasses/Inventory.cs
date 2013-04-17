using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using WarTornLands.Infrastructure;
using Microsoft.Xna.Framework;
using WarTornLands.PlayerClasses.Items;
using WarTornLands.Infrastructure.Systems.DrawSystem;

namespace WarTornLands.PlayerClasses
{
    public class Inventory : IDrawProvider
    {
        #region Drawvariablen

        private bool _inventoryIsOpen;
        private bool _previouskeystate;
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

        private short _totalItemCount;

        #endregion

        #region Itemvariablen

        private short _countPotions;
        private short _maxPotions;

        private short _countKeys;
        private short _maxKeys;

        private bool _hasNormalHammer;
        private bool _useNormalHammer;

        private bool _hasChainHammer;
        private bool _useChainHammer;

        private bool _hasWoodenShield;
        private bool _useWoodenShield;

        private KeyStash _keys;
        public HammerStash Hammer { get; private set; }

        #endregion

        #region GetterundSetter

        public short GetPotions
        {
            get { return _countPotions; }
            set { _countPotions = value; }
        }

        public short GetSchluessel
        {
            get { return _countKeys; }
            set { _countKeys = value; }
        }

        public bool GetNormalhammer
        {
            get { return _hasNormalHammer; }
            set { _hasNormalHammer = value; }
        }

        public bool GetHolzschild
        {
            get { return _hasWoodenShield; }
            set { _hasWoodenShield = value; }
        }

        public bool UseHolzschild
        {
            get { return _useWoodenShield; }
            set { _useWoodenShield = value; }
        }

        public bool UseNormalhammer
        {
            get { return _useNormalHammer; }
            set { _useNormalHammer = value; }
        }

        public bool UseKettenhammer
        {
            get { return _useChainHammer; }
            set { _useChainHammer = value; }
        }

        public bool GetKettenhammer
        {
            get { return _hasChainHammer; }
            set { _hasChainHammer = value; }
        }

        #endregion

        public Inventory(WarTornLands.Entities.Modules.Think.Parts.SwingHitAbility swing)
        {
            _deltaWidth = (Game1.Instance.Window.ClientBounds.Width / _standardWidth);
            _deltaHeight = (Game1.Instance.Window.ClientBounds.Height / _standardHeight);
            _chestPicture = Game1.Instance.Content.Load<Texture2D>("sprite/treasureChest");
            _potionPicture = Game1.Instance.Content.Load<Texture2D>("sprite/potion");
            _keyPicture = Game1.Instance.Content.Load<Texture2D>("sprite/key");
            _bosskeyPicture = Game1.Instance.Content.Load<Texture2D>("sprite/bosskey");
            _radius = 100;
            _totalItemCount = 8;
            _inventoryIsOpen = false;
            _maxPotions = 5;
            _maxKeys = 2;
            _previouskeystate = false;
            _inventoryIsOpen = false;
            _keys = new KeyStash();
            Hammer = new HammerStash(swing);
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
                _keys.AddKey((item as MasterKey).AreaID);
                return true;
            }
            #endregion
            #region NoneHammer
            if (item is NoneHammer)
                return Hammer.SetNone();
            #endregion
            #region NormalHammer
            if(item is NormalHammer)
                return Hammer.SetNormal();
            #endregion
            #region ChainHammer
            if(item is ChainHammer)
                return Hammer.SetChain();
            #endregion
            
            return false;
        }

        public void AddKeyShelf(string areaID)
        {
            _keys.AddShelf(areaID);
            _keys.AddMasterShelf(areaID);
        }

        public void Draw(GameTime gameTime)
        {
            double currentangle = MathHelper.PiOver2;
            double incrementangle = MathHelper.TwoPi / _totalItemCount;
            for (double i = 0; i < _totalItemCount; i++)
            {
                switch ((int)i)
                {
                    case 0:
                        if (_countPotions > 0)
                        {
                            _itemPicture = _potionPicture;
                        }
                        else
                        {
                            _itemPicture = _chestPicture;
                        }
                        break;
                    default:
                        _itemPicture = _chestPicture;
                        break;
                }

                Game1.Instance.SpriteBatch.Draw(_itemPicture, new Microsoft.Xna.Framework.Rectangle((int)(((Game1.Instance.Window.ClientBounds.Width * 0.5f) - (Player.Instance.DrawModule.Size.X * 0.5f)) + _radius * Math.Cos(currentangle)), (int)(((Game1.Instance.Window.ClientBounds.Height * 0.5f) - (Player.Instance.DrawModule.Size.Y * 0.25f)) + _radius * Math.Sin(currentangle)), (int)(60 * _deltaWidth), (int)(60 * _deltaHeight)), Color.White);
                currentangle -= incrementangle;

                if (i == 0 || i == _totalItemCount - 1)
                {

                }
                else
                {
                    Game1.Instance.SpriteBatch.Draw(_chestPicture, new Microsoft.Xna.Framework.Rectangle((int)(Game1.Instance.Window.ClientBounds.Width * 0.125), (int)(Game1.Instance.Window.ClientBounds.Height * (i / _totalItemCount)), (int)(60 * _deltaWidth), (int)(60 * _deltaHeight)), Color.White);
                    Game1.Instance.SpriteBatch.Draw(_chestPicture, new Microsoft.Xna.Framework.Rectangle((int)(Game1.Instance.Window.ClientBounds.Width * (2 * 0.125)), (int)(Game1.Instance.Window.ClientBounds.Height * (i / _totalItemCount)), (int)(60 * _deltaWidth), (int)(60 * _deltaHeight)), Color.White);

                }
            }
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
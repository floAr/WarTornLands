using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using WarTornLands.Infrastructure;
using Microsoft.Xna.Framework;
using WarTornLands.PlayerClasses.Items;

namespace WarTornLands.PlayerClasses
{


    public class Inventory : DrawableGameComponent
    {
        #region Drawvariablen

        private bool _inventoryIsOpen;
        private bool _previouskeystate;
        private double _standardHeight = 480;
        private double _standardWidth = 800;
        private double _deltaHeight;
        private double _deltaWidth;
        private int _radius;
         

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

        private List<Item> _items;

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

        public Inventory()
            : base(Game1.Instance as Game)
        {
            _deltaWidth = (Game1.Instance.Window.ClientBounds.Width / _standardWidth);
            _deltaHeight = (Game1.Instance.Window.ClientBounds.Height / _standardHeight);
            _itemPicture = Game1.Instance.Content.Load<Texture2D>("treasureChest");
            _radius = 100;
            _totalItemCount = 8;
            _inventoryIsOpen = false;
            _maxPotions = 5;
            _maxKeys = 2;
            _previouskeystate = false;
            _inventoryIsOpen = false;
            _items = new List<Item>();
            this.DrawOrder = 100000;
        }


        public bool Insert(Items.Item item)
        {
            _items.Add(item);

            switch (item.ItemType)
            {
                case Items.ItemTypes.Potion:
                    if (_countPotions < _maxPotions)
                    {
                        _countPotions++;
                        return true;
                    }
                    else return false;
                case Items.ItemTypes.Hammer:
                    _hasNormalHammer = true;
                    return true;
                case Items.ItemTypes.ChainHammer:
                    _hasChainHammer = true;
                    return true;
                case Items.ItemTypes.WoodenShield:
                    _hasWoodenShield = true;
                    return true;
                /*   case Items.ItemTypes.Schluessel:
                       if (_anzahlschluessel < _maxschluessel)
                       {
                           _anzahlschluessel++;
                           return true;
                       }
                       else return false;   */
                default:
                    return false;
            }

        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            if (InputManager.Instance.Inventory.Value && !_previouskeystate)
            {
                if (_inventoryIsOpen)
                {
                    _inventoryIsOpen = false;
                }
                else
                {
                    _inventoryIsOpen = true;
                }
            }
            _previouskeystate = InputManager.Instance.Inventory.Value;

            if (_inventoryIsOpen)
            {
                double currentangle = MathHelper.PiOver2;
                double incrementangle = MathHelper.TwoPi / _totalItemCount;
                for (double i = 0; i < _totalItemCount; i++)
                {
                    Game1.Instance.SpriteBatch.Draw(_itemPicture, new Microsoft.Xna.Framework.Rectangle((int)(((Game1.Instance.Window.ClientBounds.Width * 0.5f) - (Game1.Instance.Player.MDrawModule.Size.X * 0.5f)) + _radius * Math.Cos(currentangle)), (int)(((Game1.Instance.Window.ClientBounds.Height * 0.5f) - (Game1.Instance.Player.MDrawModule.Size.Y * 0.25f)) + _radius * Math.Sin(currentangle)), (int)(60 * _deltaWidth), (int)(60 * _deltaHeight)), Color.White);
                    currentangle -= incrementangle;

                    if (i == 0 || i == _totalItemCount - 1)
                    {

                    }
                    else
                    {
                        Game1.Instance.SpriteBatch.Draw(_itemPicture, new Microsoft.Xna.Framework.Rectangle((int)(Game1.Instance.Window.ClientBounds.Width * 0.125), (int)(Game1.Instance.Window.ClientBounds.Height * (i / _totalItemCount)), (int)(60 * _deltaWidth), (int)(60 * _deltaHeight)), Color.White);
                        Game1.Instance.SpriteBatch.Draw(_itemPicture, new Microsoft.Xna.Framework.Rectangle((int)(Game1.Instance.Window.ClientBounds.Width * (2 * 0.125)), (int)(Game1.Instance.Window.ClientBounds.Height * (i / _totalItemCount)), (int)(60 * _deltaWidth), (int)(60 * _deltaHeight)), Color.White);

                    }

                }
            }
       }

        internal bool HasKey(int _id)
        {
            // TODO change :-)

            foreach (Item i in _items)
            {
                if ((int)i.ItemType == _id)
                    return true;
            }

            return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using WarTornLands.Infrastructure;
using Microsoft.Xna.Framework;

namespace WarTornLands.PlayerClasses
{
    

   public class Inventory
    {
       private static Inventory _inventory;

       #region Drawvariablen

       private double _standardheight = 480;
       private double _standardwidth = 800;
       private double _deltaheight;
       private double _deltawidth;

       private Texture2D _itempicture; 

       #endregion

       #region Itemvariablen

       private short _anzahlaktuellerItemsimSpiel = 2;

       private short _anzahlpotions;
       private short _maxpotions = 2;

       private short _anzahlschluessel;
       private short _maxschluessel = 2;

       private bool _getnormalhammer;       
       private bool _usenormalhammer;

       private bool _getkettenhammer;
       private bool _usekettenhammer;

       private bool _getholzschild;
       private bool _useholzschild;

       #endregion

       #region GetterundSetter

        public short GetPotions
        {
            get { return _anzahlpotions; }
            set { _anzahlpotions = value; }
        }

        public short GetSchluessel
        {
            get { return _anzahlschluessel; }
            set { _anzahlschluessel = value; }
        }

        public bool GetNormalhammer
        {
            get { return _getnormalhammer; }
            set { _getnormalhammer = value; }
        }

        public bool GetHolzschild
        {
            get { return _getholzschild; }
            set { _getholzschild = value; }
        }

        public bool UseHolzschild
        {
            get { return _useholzschild; }
            set { _useholzschild = value; }
        }

        public bool UseNormalhammer
        {
            get { return _usenormalhammer; }
            set { _usenormalhammer = value; }
        }

        public bool UseKettenhammer
        {
            get { return _usekettenhammer; }
            set { _usekettenhammer = value; }
        }

        public bool GetKettenhammer
        {
            get { return _getkettenhammer; }
            set { _getkettenhammer = value; }
        }

       #endregion
       
       private Inventory()
       { 
           _deltawidth = (Game1.Instance.Window.ClientBounds.Width / _standardwidth);
           _deltaheight = (Game1.Instance.Window.ClientBounds.Height / _standardheight);
           _itempicture = Game1.Instance.Content.Load<Texture2D>("Schatztruhe");
       }

       public static Inventory GetInstance()
       {
           if (_inventory == null)
           {
               _inventory = new Inventory();               
           }

           return _inventory;
       }

       public bool Insert(Items.Item item)
       {
           switch (item.Itemtyp)
           {
               case Items.ItemTypes.Potion:
                   if (_anzahlpotions < _maxpotions)
                   {
                       _anzahlpotions++;
                       return true;
                   }
                   else return false;
               case Items.ItemTypes.Hammer:
                   _getnormalhammer = true;
                   return true;
               case Items.ItemTypes.Kettenhammer:
                   _getkettenhammer = true;
                   return true;
               case Items.ItemTypes.Holzschild:
                   _getholzschild = true;
                   return true;
               case Items.ItemTypes.Schluessel:
                   if (_anzahlschluessel < _maxschluessel)
                   {
                       _anzahlschluessel++;
                       return true;
                   }
                   else return false;                
               default: 
                   return false;
           }

        }


       public void AktivMenue()
       {

       }

       public void DrawMenue()
       {
           // Norden
           Game1.Instance.SpriteBatch.Draw(_itempicture, new Microsoft.Xna.Framework.Rectangle((int)((400 * _deltawidth) - 30), (int)((240 * _deltaheight) - 30) - 120, 60, 60), Color.White);
           //Nordosten
           Game1.Instance.SpriteBatch.Draw(_itempicture, new Microsoft.Xna.Framework.Rectangle((int)((400 * _deltawidth) - 30) + 120, (int)((240 * _deltaheight) - 30) - 120, 60, 60), Color.White);
           // Osten
           Game1.Instance.SpriteBatch.Draw(_itempicture, new Microsoft.Xna.Framework.Rectangle((int)((400 * _deltawidth) - 30) + 120, (int)((240 * _deltaheight) - 30), 60, 60), Color.White);
           //Südosten
           Game1.Instance.SpriteBatch.Draw(_itempicture, new Microsoft.Xna.Framework.Rectangle((int)((400 * _deltawidth) - 30) + 120, (int)((240 * _deltaheight) - 30) + 120, 60, 60), Color.White);
           // Süden4
           Game1.Instance.SpriteBatch.Draw(_itempicture, new Microsoft.Xna.Framework.Rectangle((int)((400 * _deltawidth) - 30), (int)((240 * _deltaheight) - 30) + 120, 60, 60), Color.White);
           //Südwesten
           Game1.Instance.SpriteBatch.Draw(_itempicture, new Microsoft.Xna.Framework.Rectangle((int)((400 * _deltawidth) - 30) - 120, (int)((240 * _deltaheight) - 30) + 120, 60, 60), Color.White);
           //Westen
           Game1.Instance.SpriteBatch.Draw(_itempicture, new Microsoft.Xna.Framework.Rectangle((int)((400 * _deltawidth) - 30) - 120, (int)((240 * _deltaheight) - 30), 60, 60), Color.White);
           //Nordwesten
           Game1.Instance.SpriteBatch.Draw(_itempicture, new Microsoft.Xna.Framework.Rectangle((int)((400 * _deltawidth) - 30) - 120, (int)((240 * _deltaheight) - 30) - 120, 60, 60), Color.White);
       }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Infrastructure;

namespace WarTornLands.PlayerClasses
{
    

   public class Inventory
    {
       private static Inventory _inventory;

       #region Itemvariablen

       private short _anzahlaktuellerItemsimSpiel = 2;

       private short _anzahlpotions;
       private short _maxpotions;

       private short _anzahlschluessel;
       private short _maxschluessel;

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
       { }

       public static Inventory GetInstance()
       {
           if (_inventory == null)
               _inventory = new Inventory();

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
    }
}

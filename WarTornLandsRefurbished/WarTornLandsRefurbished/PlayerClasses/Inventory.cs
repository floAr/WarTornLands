using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarTornLands.PlayerClasses
{
    public enum Items 
    { 
        Potion = 0
    }

   public class Inventory
    {
        private static Inventory _inventory;

        public int Potions { get; set; }

        private Inventory()
        { }

        public static Inventory GetInstance()
        {
            if (_inventory == null)
                _inventory = new Inventory();

            return _inventory;
        }

        public bool Insert(Items item)
        {
            switch (item)
            {
                case Items.Potion:
                    if (Potions < Constants.Inventory_MaxPotions)
                    {
                        Potions++;
                        return true;
                    }
                    else return false;

                default:
                    return false;
            }
        }
    }
}

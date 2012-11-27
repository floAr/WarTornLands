using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Infrastructure;

namespace WarTornLands.PlayerClasses
{
    public enum ItemTypes 
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

        public bool Insert(ItemTypes item)
        {
            switch (item)
            {
                case ItemTypes.Potion:
                    if (Potions < 1 /*Maxpotions*/)
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

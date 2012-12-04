using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarTornLands.PlayerClasses.Items
{
    public enum ItemTypes 
    { 
        /*
         * Verbrauchsitems
         * 0 - 100
         * 
         * Hämmer und andere Waffen
         * 101 - 200
         * 
         * Schilde
         * 201 - 300
         * 
         * Gadgets
         * 301 - 400
         * 
         * Schlüsselgegenstände
         * 401 - 500
         * 
         * sonstiges
         * 501 - 600
         */

        // Restore a fixed number of health points. TODO specify number
        Potion = 0,

        // Standard hammer the player has from the beginning
        Hammer = 101,

        // Hammer with a chain. Ranged, but less damage
        // Can be used to get over chasms
        ChainHammer = 102,

        // Standard wooden shield, can block attacks
        WoodenShield = 201,

        // Key to open standard dungeon doors
        SmallKey = 401,

        // Key to open the dungeon boss door
        MasterKey = 402
    }

    public class Item
    {
        public string Name { get; private set; }
        private ItemTypes _itemType;

        public ItemTypes ItemType
        {
            get { return _itemType; }
            set { _itemType = value; }
        }

        public Item(ItemTypes itemType)
        {
            _itemType = itemType;

            if (ItemTypes.SmallKey == itemType)
            {
                Name = "Kleiner~Schluessel";
            }

            if (ItemTypes.MasterKey == itemType)
            {
                Name = "Riesenschluessel";
            }
        }

        public Item(ItemTypes itemtyp, string name)
        {
            _itemType = itemtyp;
            Name = name;
        }
    }

}

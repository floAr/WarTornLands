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
        // Stellt eine bestimmte menge an Lebenspunkten wieder her. Noch zu spezifizieren
        Potion = 0,

        // Standard hammer der von begin an verfügbar ist.
        Hammer = 101,

        // Hammer der mit einer Kette verbunden ist, hat Reichweite aber weniger schaden als Hammer. Kann benutzt werden um sich über abgründe zu ziehen.
        Kettenhammer = 102,

        // Standardholzschild, kann angriffe ablenken
        Holzschild = 201,

        // Schlüssel zum öffnen von Dungeontüren
        KleinerSchluessel = 401,

        MasterKey = 402
    }

    public class Item
    {
        public string Name { get; private set; }
        private ItemTypes _itemtyp;

        public ItemTypes Itemtyp
        {
            get { return _itemtyp; }
            set { _itemtyp = value; }
        }

        public Item(ItemTypes itemtyp)
        {
            _itemtyp = itemtyp;

            if (ItemTypes.KleinerSchluessel == itemtyp)
            {
                Name = "Kleiner Schluessel";
            }
        }

        public Item(ItemTypes itemtyp, string name)
        {
            _itemtyp = itemtyp;
            Name = name;
        }
    }

}

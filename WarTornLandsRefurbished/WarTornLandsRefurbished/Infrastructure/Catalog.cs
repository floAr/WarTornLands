using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WarTornLands.Infrastructure
{
    /// <summary>
    /// Class containing lists of item and NPC names.
    /// Used by the DialogManager to display strings in the color of the type they represent.
    /// </summary>
    class Catalog
    {
        public static Color BaseColor = Color.White;
        public static Color SpeakerColor;

        public static Color WeaponColor = Color.Red;
        private List<string> _weapons;

        public static Color ConsumableColor = Color.Wheat;
        private List<string> _consumables;

        public static Color ArmorColor = Color.LightGreen;
        private List<string> _armor;

        public static Color NpcColor = Color.Violet;
        private List<string> _npcs;

        public static Color LocationColor = Color.Blue;
        private List<string> _locations;

        #region Singleton Stuff
        private static Catalog _instance;

        public static Catalog Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Catalog();
                }
                return _instance;
            }
        }
        #endregion

        private Catalog()
        {
            _weapons = new List<string>();
            _consumables = new List<string>();
            _armor = new List<string>();
            _npcs = new List<string>();
            _locations = new List<string>();
        }

        public Color CheckString(string check)
        {
            if (CheckList(_weapons, check)) return WeaponColor;
            if (CheckList(_consumables, check)) return ConsumableColor;
            if (CheckList(_armor, check)) return ArmorColor;
            if (CheckList(_npcs, check)) return NpcColor;
            if (CheckList(_locations, check)) return LocationColor;

            return BaseColor;
        }

        private bool CheckList(List<string> list, string check)
        {
            foreach (string item in list)
            {
                if (item.Equals(check))
                    return true;
            }

            return false;
        }

        public void SetupTestCatalog()
        {
            _npcs.Add("Jason");
            _npcs.Add("Gruselute");
            _npcs.Add("Frederik");
            _locations.Add("Crystal~Lake");
            _weapons.Add("machete");
            _weapons.Add("Kleiner~Schluessel");
            _weapons.Add("Riesenschluessel");
            _weapons.Add("Schluessel");
            _armor.Add("Hockey~Mask");
            _armor.Add("Koffer");
            _armor.Add("Kaese");
        }

        public void SetColors(
            Color basecolor,
            Color weapons,
            Color consumables,
            Color armor,
            Color npcs,
            Color locations
            )
        {
            BaseColor = basecolor;
            WeaponColor = weapons;
            ConsumableColor = consumables;
            ArmorColor = armor;
            NpcColor = npcs;
            LocationColor = locations;
            SpeakerColor = NpcColor;
        }
    }
}

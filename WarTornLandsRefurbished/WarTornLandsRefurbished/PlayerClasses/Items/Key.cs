using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarTornLands.PlayerClasses.Items
{
    public class KeyStash
    {
        private List<KeyShelf> _shelfes;
        private List<MasterShelf> _masters;

        public KeyStash()
        {
            _shelfes = new List<KeyShelf>();
            _masters = new List<MasterShelf>();
        }

        public void AddShelf(string areaID)
        {
            KeyShelf shelf = new KeyShelf();
            shelf.AreaID = areaID;
            shelf.Count = 0;

            _shelfes.Add(shelf);
        }

        public void AddMasterShelf(string areaID)
        {
            MasterShelf master = new MasterShelf();
            master.AreaID = areaID;
            master.Owned = false;

            _masters.Add(master);
        }

        /// <summary>
        /// Removes a key for the current area from its respective shelf.
        /// Returns true if there is a key available.
        /// </summary>
        /// <param name="areaID">The area ID.</param>
        /// <returns></returns>
        public bool UseKey(string areaID)
        {
            foreach (KeyShelf s in _shelfes)
            {
                if (s.AreaID.Equals(areaID))
                {
                    if (s.Count > 0)
                    {
                        s.Remove();
                        return true;
                    }
                    else return false;
                }
            }
            throw new Exception("There is no KeyShelf for area "+areaID+". However that happened.");
        }

        public bool UseMaster(string areaID)
        {
            foreach (MasterShelf m in _masters)
            {
                if (m.AreaID.Equals(areaID))
                {
                    if (m.Owned)
                    {
                        m.Remove();
                        return true;
                    }
                    else return false;
                }
            }
            throw new Exception("There is no MasterShelf for area " + areaID + ". However that happened.");
        }

        /// <summary>
        /// Adds a key for the current area.
        /// </summary>
        /// <param name="areaID">The area ID.</param>
        /// <exception cref="System.Exception">There is no KeyShelf for area  + areaID + . However that happened.</exception>
        public void AddKey(string areaID)
        {
            foreach (KeyShelf s in _shelfes)
            {
                if (s.AreaID.Equals(areaID))
                {
                    s.Add();
                    return;
                }
            }
            throw new Exception("There is no KeyShelf for area " + areaID + ". However that happened.");
        }

        public void AddMasterKey(string areaID)
        {
            foreach (MasterShelf m in _masters)
            {
                if (m.AreaID.Equals(areaID))
                {
                    if (!m.Owned)
                    {
                        m.Place();
                        return;
                    }
                    else
                    {
                        throw new Exception("You already ot a MasterKey for the "+areaID+" dungeon.");
                    }
                }
            }

            throw new Exception("There is no MasterShelf for area " + areaID + ". However that happened.");
        }

        /// <summary>
        /// Contains the amount of Keys for a specific dungeon.
        /// </summary>
        private struct KeyShelf
        {
            public string AreaID;
            public short Count;

            public void Add()
            {
                Count++;
            }

            public void Remove()
            {
                Count--;
            }
        }

        private struct MasterShelf
        {
            public string AreaID;
            public bool Owned;

            public void Place()
            {
                Owned = true;
            }

            public void Remove()
            {
                Owned = false;
            }
        }
    }

    public class DoorKey : Item
    {
        public string AreaID { get; private set; }

        public DoorKey(string areaID)
            : base("Kleiner Schlüssel")
        {
            AreaID = areaID;
        }
    }

    public class MasterKey : Item
    {
        public string AreaID { get; private set; }

        public MasterKey(string areaID)
            : base("Boss Schlüssel")
        {
            AreaID = areaID;
        }
    }
}

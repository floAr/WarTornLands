using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarTornLands.PlayerClasses.Items
{
    public class HammerStash
    {
        private Hammer _hammer;
        private bool _hasNormal;
        private bool _hasChain;
        private bool _hasPick;

        public bool HasNone
        {
            get { return _hammer == null; }
        }
        public bool HasNormal
        {
            get { return _hasNormal; }
            private set { _hasNormal = value; }
        }
        public bool HasChain
        {
            get { return _hasChain; }
            private set { _hasChain = value; }
        }
        public bool HasPick
        {
            get { return _hasPick; }
            private set { _hasPick = value; }
        }

        public EventHandler Use;

        public HammerStash(WarTornLands.Entities.Modules.Think.Parts.SwingHitAbility swing)
        {
            Use = new EventHandler(OnUse);
            swing.Use += Use;
        }

        public bool SetNone()
        {
            _hammer = null;

            return true;
        }
        public bool SetNormal()
        {
            if (HasNormal && !(_hammer is NormalHammer))
            {
                _hammer = new NormalHammer();
                return true;
            }

            return false;
        }
        public bool SetChain()
        {
            if (HasChain && !(_hammer is ChainHammer))
            {
                _hammer = new ChainHammer();
                return true;
            }

            return false;
        }
        public bool SetPick()
        {
            if (HasPick && !(_hammer is Pick))
            {
                _hammer = new Pick();
                return true;
            }

            return false;
        }

        public void Add(Hammer hammer)
        {
            if (hammer is NormalHammer)
            {
                HasNormal = true;
                SetNormal();
            }
            if (hammer is ChainHammer)
            {
                HasChain = true;
                SetChain();
            }
            if (hammer is Pick)
            {
                HasPick = true;
                SetPick();
            }
        }

        /// <summary>
        /// Removes the specified hammer.
        /// In case the removed hammer was equipped, the next best hammer is equipped (quality ascending: none-normal-pick-chain).
        /// Returns false if the hammer was not in possession.
        /// </summary>
        /// <param name="hammer">The hammer.</param>
        public bool Remove(Hammer hammer)
        {
            if (hammer is NormalHammer && HasNormal)
            {
                HasNormal = false;

                if (_hammer is NormalHammer)
                {
                    if (!SetChain()) { }
                    else if (!SetPick()) { }
                    else
                        return SetNone();
                }

                return true;
            }

            if (hammer is ChainHammer && HasChain)
            {
                HasChain = false;

                if (_hammer is ChainHammer)
                {
                    if (!SetPick()) { }
                    else if (!SetNormal()) { }
                    else
                        SetNone();
                }

                return true;
            }

            if (hammer is Pick && HasPick)
            {
                HasPick = false;

                if (_hammer is Pick)
                {
                    if (!SetChain()) { }
                    else if (!SetNormal()) { }
                    else
                        SetNone();
                }

                return true;
            }

            return false;
        }

        private void OnUse(object sender, EventArgs e)
        {
            _hammer.Use();
        }
    }

    public interface Hammer
    {
        void Use();
    }

    public class NormalHammer : Item, Hammer
    {

        public NormalHammer()
            : base("Hammer")
        {
        }

        public void Use()
        {
        }
    }

    public class ChainHammer : Item, Hammer
    {

        public ChainHammer()
            : base("Kettenhammer")
        {
        }

        public void Use()
        {
            throw new Exception();
        }
    }

    public class Pick : Item, Hammer
    {
        public Pick()
            : base("Krähenschnabel")
        { 
        }
    }

    /// <summary>
    /// Used to remove the hammer from the Players Inventory
    /// </summary>
    public class NoneHammer : Item
    {
        public NoneHammer()
            : base("")
        { }
    }
}

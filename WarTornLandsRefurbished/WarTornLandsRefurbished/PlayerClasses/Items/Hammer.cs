using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarTornLands.PlayerClasses.Items
{
    public class HammerStash
    {
        private Hammer _hammer;
        public bool HasNone
        {
            get { return _hammer == null; }
        }
        public bool HasNormal
        {
            get { return _hammer is NormalHammer; }
        }
        public bool HasChain
        {
            get { return _hammer is ChainHammer; }
        }

        public EventHandler Use;

        public HammerStash(WarTornLands.Entities.Modules.Think.Parts.SwingHitAbility swing)
        {
            Use = new EventHandler(OnUse);
            swing.Use += Use;
        }

        /// <summary>
        /// Empties the hammer slot.
        /// </summary>
        public bool SetNone()
        {
            if (_hammer != null)
            {
                _hammer = null;
                return true;
            }

            return false;
        }
        public bool SetNormal()
        {
            if (!(_hammer is NormalHammer))
            {
                _hammer = new NormalHammer();
                return true;
            }

            return false;
        }
        public bool SetChain()
        {
            if (!(_hammer is ChainHammer))
            {
                _hammer = new ChainHammer();
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

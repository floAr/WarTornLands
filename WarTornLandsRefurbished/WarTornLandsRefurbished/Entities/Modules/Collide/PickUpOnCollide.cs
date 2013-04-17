using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.PlayerClasses;
using System.Data;

namespace WarTornLands.Entities.Modules.Collide
{
    public class PickUpOnCollide : BaseModule, ICollideModule
    {
        private WarTornLands.PlayerClasses.Items.Item _loot;

        private bool _oneTime = true;

        private Counter.CounterManager _pushItemCooldown;
        private int _charges;
        private bool _isOnCD;


        public PickUpOnCollide(WarTornLands.PlayerClasses.Items.Item item)
        {
            _loot = item;
        }
        public PickUpOnCollide(WarTornLands.PlayerClasses.Items.Item item, int charges, int waitingTimeBetweenDrop)
        {
            _oneTime = false;
            _pushItemCooldown = new Counter.CounterManager();
            _pushItemCooldown.AddCounter("cdDrop", waitingTimeBetweenDrop);
            _pushItemCooldown.Bang += new EventHandler<Counter.BangEventArgs>(_pushItemCooldown_Bang);
            _loot = item;
        }
        public PickUpOnCollide(DataRow data)
        { }

        void _pushItemCooldown_Bang(object sender, Counter.BangEventArgs e)
        {
            if (e.ID == "cdDrop")
                _isOnCD = false;
        }

        public void OnCollide(CollideInformation info)
        {
            if (_charges == 0)
                _owner.ToBeRemoved = true;

            ((Player)info.Collider).GiveItem(_loot);

            if (_oneTime)
            {
                _owner.ToBeRemoved = true;
            }
            else
            {
                _charges = Math.Max(-1, _charges--);
                _pushItemCooldown.StartCounter("cdDrop");
                _isOnCD = true;
            }
        }

        public bool IsPassable(CollideInformation info)
        {
            // Pickup item will never block
            return true;
        }

        public Microsoft.Xna.Framework.Rectangle BodyShape
        {
            get;
            set;
        }

        public Microsoft.Xna.Framework.Rectangle MovingShape
        {
            get;
            set;
        }
    }
}

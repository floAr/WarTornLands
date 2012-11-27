using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.PlayerClasses;

namespace WarTornLands.Entities.Modules.Collide
{
    public class PickUpOnCollide : BaseModule, ICollideModule
    {
        private WarTornLands.PlayerClasses.ItemTypes _loot;

        private bool _oneTime = true;

        private Counter.CounterManager _pushItemCooldown;
        private int _charges;
        private bool _isOnCD;


        public PickUpOnCollide(WarTornLands.PlayerClasses.ItemTypes item)
        {
            _loot = item;
        }
        public PickUpOnCollide(WarTornLands.PlayerClasses.ItemTypes item, int charges, int waitingTimeBetweenDrop)
        {
            _oneTime = false;
            _pushItemCooldown = new Counter.CounterManager();
            _pushItemCooldown.AddCounter("cdDrop", waitingTimeBetweenDrop);
            _pushItemCooldown.Bang += new EventHandler<Counter.BangEventArgs>(_pushItemCooldown_Bang);
            _loot = item;
        }

        void _pushItemCooldown_Bang(object sender, Counter.BangEventArgs e)
        {
            if (e.ID == "cdDrop")
                _isOnCD = false;
        }

        public bool OnCollide(CollideInformation info)
        {
            if (_charges == 0)
            {
                _owner.IsDead = true;
                return true;
            }
            if (!info.IsPlayer || _isOnCD || _owner.IsDead)
                return true;

            ((Player)info.Collider).GiveItem(_loot);

            if (_oneTime)
            {
                _owner.IsDead = true;
            }
            else
            {
                _charges = Math.Max(-1, _charges--);
                _pushItemCooldown.StartCounter("cdDrop");
                _isOnCD = true;
            }
            return true;


        }
    }
}

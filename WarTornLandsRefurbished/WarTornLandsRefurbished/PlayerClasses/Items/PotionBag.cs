using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarTornLands.PlayerClasses.Items
{
    class PotionBag : Item
    {
        private const int HealAmount = 50;
        private const int MaxPotions = 10;
        private int _count = 0;

        public PotionBag()
            : base("Potion Bag")
        { }

        /// <summary>
        /// Adds the specified amount of potions to the bag.
        /// In case it is attempted to store more potions than can be contained (specified by MaxPotions constant)
        /// the amount that could not be stored is returned as the functions value.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <returns>Amount of potions that could not be contained.</returns>
        public int Add(int amount)
        {
            _count += amount;

            if (_count > MaxPotions)
                return amount - _count;

            return 0;
        }

        public bool Use()
        {
            if(_count > 0)
            {
                Player.Instance.Health += HealAmount;
                if (Player.Instance.Health > Player.Instance.MaxHealth)
                    Player.Instance.Health = Player.Instance.MaxHealth;
                _count -= 1;

                return true;
            }

            return false;
        }
    }
}

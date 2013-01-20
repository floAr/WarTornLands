using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.PlayerClasses.Items;

namespace WarTornLands.Infrastructure.Systems.DialogSystem
{
    class ItemContainer : ConversationItem
    {
        private List<Item> _items;

        public ItemContainer(List<Item> items)
            : base(ComposeDescription(items))
        {
            if (items.Count == 0)
                throw new Exception("The itemlist for an ItemContainer should'nt be empty.");

            _items = items;
        }

        public ItemContainer(Item item)
            : base(ComposeDescription(item))
        {
            _items = new List<Item>();
            _items.Add(item);
        }

        private static string ComposeDescription(List<Item> items)
        {
            if(items.Count == 1 && items[0] is NoneHammer)
                    return "Dein Hammer wurde geklaut!";

            string message = "Item";
            if (items.Count > 1)
                message += 's';
            message += " erhalten: ";

            for (int i = 0; i < items.Count; ++i)
            {
                message += items[i].Name;
                message += i < items.Count - 1 ? ", " : ".";
            }

            return message;
        }

        private static string ComposeDescription(Item item)
        {
            if (item is NoneHammer)
                return "Dein Hammer wurde geklaut!";

            string message = "Item erhalten: ";
            message += item.Name;

            return message;
        }

        public override void Trigger()
        {
            foreach (Item i in _items)
            {
                Game1.Instance.Player.GiveItem(i);
            }

            // Show message
            base.Trigger();
        }
    }
}

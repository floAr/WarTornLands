using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarTornLands.PlayerClasses.Items
{
    public abstract class Item
    {
        /// <summary>
        /// The Name of the Item as displayed in the game.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; protected set; }

        public Item(string name)
        {
            Name = name;
        }

        public static Item Nothing
        { get { return null; } }
    }
}

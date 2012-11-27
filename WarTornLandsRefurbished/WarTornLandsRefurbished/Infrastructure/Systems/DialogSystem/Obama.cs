using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarTornLands.Infrastructure.Systems.DialogSystem
{
    /// <summary>
    /// Obama changes the world
    /// (Set global variables)
    /// </summary>
    abstract class Obama : ConversationItem
    {
        public Obama()
            : base("Options lol")
        { 
        }

        public void Trigger()
        { }
    }
}

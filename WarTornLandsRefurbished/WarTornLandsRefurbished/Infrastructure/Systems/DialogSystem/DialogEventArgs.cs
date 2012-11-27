using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarTornLands.Infrastructure.Systems.DialogSystem
{
    public class DialogEventArgs : EventArgs
    {
        public string Speaker { get; private set; }
        public ConversationItem Statement { get; private set; }

        internal DialogEventArgs(string speaker, ConversationItem statement)
        {
            Speaker = speaker;
            Statement = statement;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarTornLands.Infrastructure.Systems.DialogSystem
{
    class TextLineAndEvent: ConversationItem
    {
        Action _action;
        public TextLineAndEvent(string text,Action action)
            : base(text)
        {
            _action = action;
        }

        public override void Trigger()
        {
            DisplayText();
            _action.Invoke();
        }



    }
}

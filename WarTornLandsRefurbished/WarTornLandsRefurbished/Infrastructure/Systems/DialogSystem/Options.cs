using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarTornLands.Infrastructure.Systems.DialogSystem
{
    /// <summary>
    /// Shows in the dialog as a list of options from which the player can chose.
    /// (Yes/No scenarios)
    /// </summary>
    class Options : ConversationItem
    {
        private List<string> _conIDs;

        public Options(List<string> conIDs)
            : base("Options placeholder")
        {
            _conIDs = conIDs;
        }

        public override void Trigger()
        {
            
        }
    }
}

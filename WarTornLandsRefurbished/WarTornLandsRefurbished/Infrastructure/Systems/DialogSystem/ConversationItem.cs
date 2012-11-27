using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarTornLands.Infrastructure.Systems.DialogSystem
{
    /// <summary>
    /// A single screen in a conversation.
    /// Contains a text message to tell the player what is happening (item received, world changed) 
    /// or simply to communicate story informations.
    /// </summary>
    public abstract class ConversationItem
    {
        public string Text { get; private set; }

        public ConversationItem(string text)
        {
            Text = text;
        }

        /// <summary>
        /// Determines what happens if this line is triggered/shown (give item etc.).
        /// </summary>
        public virtual void Trigger()
        {
            // Show text message.
        }
    }

    /// <summary>
    /// Place this at the end of a conversation and the respective NPC will shut up until spoken to again.
    /// </summary>
    class ComboBreaker : ConversationItem
    {
        public ComboBreaker()
            : base("If you can read this something went wrong")
        { }

        public override void Trigger()
        {
            base.Trigger();

            // end conversation
        }
    }
}

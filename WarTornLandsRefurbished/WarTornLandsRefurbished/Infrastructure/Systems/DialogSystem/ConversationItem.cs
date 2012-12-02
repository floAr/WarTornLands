using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Entities;
using WarTornLands.Entities.Modules.Interact;

namespace WarTornLands.Infrastructure.Systems.DialogSystem
{
    /// <summary>
    /// A single screen in a conversation.
    /// Contains a text message to tell the player what is happening (item received, world changed)
    /// or simply to communicate story informations.
    /// </summary>
    public abstract class ConversationItem
    {
        /// <summary>
        /// Gets the text message displayed if this item is called.
        /// </summary>
        /// <value>
        /// The text as string.
        /// </value>
        public string Text { get; protected set; }
        ///// <summary>
        ///// Gets the successor of the item.
        ///// #next means the conversation will continue with the next item in the list
        ///// #end means the conversation will end
        ///// [ex_id] means the conversation with the ID ex_id will start (do not include [])
        ///// </summary>
        ///// <value>
        ///// The successor.
        ///// </value>
        //public string Successor { get; private set; }
        /// <summary>
        /// Gets the name of the Entity which utters this item.
        /// </summary>
        /// <value>
        /// The name of the speaker as string.
        /// </value>
        public string Speaker
        {
            get { return _owner.Name; }
        }
        /// <summary>
        /// Gets the ID of the conversation this item belongs to.
        /// </summary>
        /// <value>
        /// The conversation ID as string.
        /// </value>
        public string ConversationID { get; private set; }

        protected Entity _owner;

        public ConversationItem(string text)
        {
            Text = text;
            //Successor = succ;
        }

        public void SetOwner(Entity owner)
        {
            _owner = owner;
        }

        public void SetID(string conID)
        {
            ConversationID = conID;
        }

        /// <summary>
        /// Determines what happens if this line is triggered/shown (give item etc.).
        /// </summary>
        public virtual void Trigger()
        {
            DisplayText();
        }

        protected void DisplayText()
        {
            DialogManager.Instance.CallDialog(this);
        }
    }

    /// <summary>
    /// Place this at the end of a conversation and the respective NPC will shut up until spoken to again.
    /// </summary>
    class ComboBreaker : ConversationItem
    {
        /// <summary>
        /// The ID of the conversation that will start the next time the owning Entity is spoken to.
        /// #this means the conversation that this ComboBreaker belongs to is the new default
        /// #mute means no conversation will be set, resulting in the Entity remaining silent upon interaction
        /// </summary>
        private string _newDefault;

        public ComboBreaker(string newDefault = "#this")
            : base("If you can read this something went wrong")
        {
            _newDefault = newDefault;
        }

        public override void Trigger()
        {
            DialogManager.Instance.CallDialog(null);
            (_owner.GetInteractModule() as Dialog).ShutDown();

            if(_newDefault.Equals("#this"))
                (_owner.GetInteractModule() as Dialog).SetNewDefault(this.ConversationID, this.ConversationID);
            else
                (_owner.GetInteractModule() as Dialog).SetNewDefault(_newDefault, this.ConversationID);
        }
    }
}

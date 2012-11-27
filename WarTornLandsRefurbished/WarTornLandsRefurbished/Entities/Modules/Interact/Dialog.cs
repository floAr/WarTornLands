using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Infrastructure.Systems.DialogSystem;
using WarTornLands.PlayerClasses;
using Microsoft.Xna.Framework;

namespace WarTornLands.Entities.Modules.Interact
{
    class Dialog : BaseModule, IInteractModule
    {
        private List<Conversation> _conversations;
        private ConversationIterator _currentCon;


        public Dialog(List<Conversation> conversations, Entity owner)
            : base() 
        {
            _conversations = conversations;
            _currentCon = _conversations.First().GetIterator();
            this._owner = owner;

            // Subscribe Enter event to continue conversations
        }

        public void Interact(Entity user)
        {
            if (user is Player)
            {
                (user as Player).CommunicateConversation(_owner.Name, _currentCon.First());
            }
            else
            {
                throw new Exception("NPCs calling to one another or other strange shit is happening.");
            }
        }

        public void Update(GameTime gameTime)
        {

        }

        #region Subscribed events

        private void OnEnter(object sender, EventArgs e)
        {
            _currentCon.Next().Trigger();
        }

        #endregion
    }
}

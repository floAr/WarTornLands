using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Infrastructure.Systems.DialogSystem;
using WarTornLands.PlayerClasses;
using Microsoft.Xna.Framework;
using WarTornLands.Infrastructure;

namespace WarTornLands.Entities.Modules.Interact
{
    class Dialog : BaseModule, IInteractModule
    {
        public bool Active { get; private set; }

        private List<Conversation> _conversations;
        private ConversationIterator _currentCon;
        private DialogManager _dm;

        public Dialog(List<Conversation> conversations, Entity owner)
            : base() 
        {
            _conversations = conversations;
            _dm = DialogManager.Instance;
            _currentCon = _conversations.First().GetIterator();
            this._owner = owner;

            // Subscribe Enter event to continue conversations
            InputManager.Instance.Interact.Pressed += new EventHandler(OnEnter);
            _dm.ConversationEnded += new EventHandler(OnConversationEnded);
        }

        public void Interact(Entity user)
        {
            if (!Active)
            {
                if (user is Player)
                {
                    CallDialog(_owner.Name, _currentCon.First());
                }
                else
                {
                    throw new Exception("NPCs calling to one another or other strange shit is happening.");
                }
            }
        }

        private void CallDialog(string speaker, ConversationItem statement)
        {
            Active = true;

            _dm.CallDialog(speaker, statement);
        }

        public void Update(GameTime gameTime)
        {

        }

        #region Subscribed events

        private void OnEnter(object sender, EventArgs e)
        {
            if(Active)
                CallDialog(_owner.Name, _currentCon.Next());
        }

        private void OnConversationEnded(object sender, EventArgs e)
        {
            Active = false;
            _currentCon = _conversations.First().GetIterator();
        }

        #endregion
    }
}

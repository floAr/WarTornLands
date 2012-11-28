using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Infrastructure.Systems.DialogSystem;
using WarTornLands.PlayerClasses;
using Microsoft.Xna.Framework;
using WarTornLands.Infrastructure;
using WarTornLands.Counter;

namespace WarTornLands.Entities.Modules.Interact
{
    class Dialog : BaseModule, IInteractModule
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="Dialog" /> is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if active; otherwise, <c>false</c>.
        /// </value>
        public bool Active { get; private set; }
        /// <summary>
        /// Gets a value indicating whether the module is on cooldown.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [on cooldown]; otherwise, <c>false</c>.
        /// </value>
        public bool OnCooldown { get; private set; }
        /// <summary>
        /// Gets or sets the cooldown time. A DialogModule on cooldown will remain silent when spoken to.
        /// </summary>
        /// <value>
        /// The cooldown in milliseconds. Default value is 2000.
        /// </value>
        public int Cooldown { get; set; }

        private List<Conversation> _conversations;
        private ConversationIterator _currentCon;
        private DialogManager _dm;
        private CounterManager _cm;

        private readonly string _cShutdown = "shutdownCounter";

        public Dialog(List<Conversation> conversations, Entity owner, int cooldown = 2000)
            : base() 
        {
            _conversations = conversations;
            _dm = DialogManager.Instance;
            _cm = owner.CM;
            _cm.AddCounter(_cShutdown);
            _cm.Bang += new EventHandler<BangEventArgs>(OnBang);
            _currentCon = _conversations.First().GetIterator();
            this._owner = owner;
            Cooldown = cooldown;

            foreach (Conversation con in _conversations)
            {
                con.SetOwner(owner);
            }

            // Subscribe Enter event to continue conversations
            InputManager.Instance.Interact.Pressed += new EventHandler(OnEnter);
        }

        public void Interact(Entity user)
        {
            if (!Active && !OnCooldown)
            {
                if (user is Player)
                {
                    Active = true;
                    OnEnter(null, EventArgs.Empty);
                }
                else
                {
                    throw new Exception("NPCs calling to one another or other strange shit is happening.");
                }
            }
        }

        /// <summary>
        /// Sets the new default conversation. See @class ComboBreaker for more information.
        /// This method should only be called by ConversationItems
        /// </summary>
        /// <param name="newDefaultID">The new default ID.</param>
        public void SetNewDefault(string newDefaultID)
        {
            foreach (Conversation con in _conversations)
            {
                if (con.ID.Equals(newDefaultID))
                    _currentCon = con.GetIterator();
            }
        }

        /// <summary>
        /// Deactivates this instance.
        /// This method should only be called by ConversationItems
        /// </summary>
        public void ShutDown()
        {
            Active = false;
            _cm.StartCounter(_cShutdown, Cooldown);
            OnCooldown = true;
        }

        public void Update(GameTime gameTime)
        {

        }

        #region Subscribed events

        private void OnEnter(object sender, EventArgs e)
        {
            if(Active)
                _currentCon.Next().Trigger();
        }

        private void OnBang(object sender, BangEventArgs e)
        {
            if (e.IsDesiredCounter(_cShutdown))
            {
                OnCooldown = false;
            }
        }

        #endregion
    }
}

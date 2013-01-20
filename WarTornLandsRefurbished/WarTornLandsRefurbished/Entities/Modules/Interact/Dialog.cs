using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Infrastructure.Systems.DialogSystem;
using WarTornLands.PlayerClasses;
using Microsoft.Xna.Framework;
using WarTornLands.Infrastructure;
using WarTornLands.Counter;
using WarTornLands.Infrastructure.Systems.InputSystem;
using System.Data;
using System.IO;
using WarTornLands.PlayerClasses.Items;

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
        private EventHandler _enter;


        private readonly string _cShutdown = "shutdownCounter";

        public Dialog(List<Conversation> conversations, int cooldown = 2000)
            : base() 
        {
            _enter = new EventHandler(OnEnter);

            _conversations = new List<Conversation>();
            foreach(Conversation con in conversations)
            {
                _conversations.Add(con.Clone());
            }
            _dm = DialogManager.Instance;

            Cooldown = cooldown;
        }
        public Dialog(DataRow data)
        {
            string dialogID = data["Dialog"].ToString() + "_Dialog.xml";
            string path = Game1.Instance.Content.RootDirectory + "/Data";

            IEnumerable<string> files = Directory.EnumerateFiles(path, dialogID, SearchOption.AllDirectories);

            DataSet dialogData = null;
            foreach (string file in files)
            {
                if (dialogData == null)
                {
                    dialogData = new DataSet();
                    dialogData.ReadXml(file);
                }
                else
                {
                    throw new Exception("More than one dialog file found for ID " + data["Dialog"].ToString());
                }
            }

            List<Conversation> conversations = ReadConversations(dialogData);

            _enter = new EventHandler(OnEnter);

            _conversations = new List<Conversation>();
            foreach (Conversation con in conversations)
            {
                _conversations.Add(con.Clone());
            }
            _dm = DialogManager.Instance;

            _currentCon = _conversations.First().GetIterator();

            Cooldown = 2000;
        }

        public override void SetOwner(Entity owner)
        {
            _cm = owner.CM;
            _cm.AddCounter(_cShutdown);
            _cm.Bang += new EventHandler<BangEventArgs>(OnBang);
            _currentCon = _conversations.First().GetIterator();
            this._owner = owner;

            foreach (Conversation con in _conversations)
            {
                try
                {
                    con.Add(new ComboBreaker());
                }
                catch (ConversationAlreadyFinalisedException e)
                { }

                con.SetOwner(owner);
            }
        }

        private List<Conversation> ReadConversations(DataSet data)
        {
            List<Conversation> cons = new List<Conversation>();

            foreach (DataRow conData in data.Tables["Conversation"].Rows)
            {
                Conversation con = new Conversation(conData["ID"].ToString());
                foreach (DataRow lineData in conData.GetChildRows("Conversation_Line"))
                {
                    switch (lineData["Type"].ToString())
                    {
                        case "Text":
                            con.Add(TextLineFromData(lineData));
                            break;
                        case "ItemContainer":
                            con.Add(ItemContainerFromData(lineData));
                            break;
                        case "ComboBreaker":
                            con.Add(ComboBreakerFromData(lineData));
                            break;
                        case "Event":
                            con.Add(TextLineAndEventFromData(lineData));
                            break;
                    }
                }
                cons.Add(con);
            }

            return cons;
        }

        #region Linetypes

        private TextLine TextLineFromData(DataRow data)
        {
            return new TextLine(data["Text"].ToString());
        }
        private ItemContainer ItemContainerFromData(DataRow data)
        {
            List<PlayerClasses.Items.Item> items = new List<PlayerClasses.Items.Item>();
            foreach (DataRow itemData in data.GetChildRows("Line_Item"))
            {
                // TODO switch for item types
                items.Add(new DoorKey(EntityBuilder.Instance.CurrentArea.AreaID));
            }
            return new ItemContainer(items);
        }
        private ComboBreaker ComboBreakerFromData(DataRow data)
        {
            return new ComboBreaker(data["NewCon"].ToString());
        }
        private TextLineAndEvent TextLineAndEventFromData(DataRow data)
        {
            return new TextLineAndEvent("", null);
        }

        #endregion

        public void Interact(Entity user)
        {
            if (!Active && !OnCooldown)
            {
                if (user is Player)
                {
                    Active = true;
                    OnEnter(null, EventArgs.Empty);

                    // Subscribe Enter event to continue conversations
                    InputManager.Instance.Subscribe((InputManager.Instance["Interact"] as Key), ref _enter);
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
        public void SetNewDefault(string newDefaultID, string calledByID)
        {
            Active = false;
            foreach (Conversation con in _conversations)
            {
                if (con.ID.Equals(newDefaultID))
                {
                    _currentCon = con.GetIterator();
                    return;
                }
            }
            throw new ConversationNotFoundException(newDefaultID, calledByID);
        }

        /// <summary>
        /// Deactivates this instance.
        /// This method should only be called by ConversationItems
        /// </summary>
        public void ShutDown()
        {
            Active = false;
            InputManager.Instance.Unsubscribe((InputManager.Instance["Interact"] as Key), ref _enter);
            _cm.StartCounter(_cShutdown, Cooldown);
            OnCooldown = true;
        }

        public void Update(GameTime gameTime)
        {

        }

        #region Subscribed events

        private void OnEnter(object sender, EventArgs e)
        {
            try
            {
                if (Active)
                    _currentCon.Next().Trigger();
            }
            catch (ConversationHasEndedException ex)
            {
                _currentCon.Reset();
            }
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

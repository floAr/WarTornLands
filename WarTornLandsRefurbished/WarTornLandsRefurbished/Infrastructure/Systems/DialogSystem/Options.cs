using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Entities.Modules.Interact;
using WarTornLands.Infrastructure.Systems.InputSystem;

namespace WarTornLands.Infrastructure.Systems.DialogSystem
{
    /// <summary>
    /// An Option is added to an Options ConversationItem.
    /// </summary>
    public struct Option
    {
        /// <summary>
        /// The ID of the conversation that is called by this Option.
        /// </summary>
        /// <value>
        /// The conversation ID as string.
        /// </value>
        public string ConID { get; private set; }
        /// <summary>
        /// The Label that is displayed to the user in the list of the respective Options item.
        /// </summary>
        /// <value>
        /// The label as string.
        /// </value>
        public string Label { get; private set; }

        public Option(string label, string conID)
            : this()
        {
            ConID = conID;
            Label = label;
        }
    }

    /// <summary>
    /// Shows in the dialog as a list of options from which the player can chose.
    /// (Yes/No scenarios)
    /// </summary>
    class Options : ConversationItem
    {
        private List<Option> _options;
        private EventHandler _scrollUp;
        private EventHandler _scrollDown;
        private EventHandler _enter;
        private int _current;


        /// <summary>
        /// The symbol or string that is displayed in front of the currently selected option.
        /// </summary>
        private static readonly string _choiceMarker = " >  ";
        private static readonly string _whatever = "    ";

        public Options(List<Option> options)
            : base("An error occured in the options class.")
        {
            _options = options;
            _current = 0;

            _scrollUp = new EventHandler(OnScrollUp);
            _scrollDown = new EventHandler(OnScrollDown);
            _enter = new EventHandler(OnEnter);
        }

        public override void Trigger()
        {
            base.Text = DecryptOptionsToString();

            (Game1.Instance.CurrentState.InputSheet["Move"] as DirectionInput).Up += _scrollUp;
            (Game1.Instance.CurrentState.InputSheet["Move"] as DirectionInput).Down += _scrollDown;
            (Game1.Instance.CurrentState.InputSheet["Interact"] as Key).Pressed += _enter;

            base.Trigger();
        }

        private string DecryptOptionsToString()
        {
            string res = "";
            int i = 0;

            foreach(Option opt in _options)
            {
                if (i == _current)
                    res += _choiceMarker;
                else
                    res  += _whatever;
                res += opt.Label;
                res += "/n";
                ++i;
            }

            return res;
        }

        #region Subscribed events

        private void OnEnter(object sender, EventArgs e)
        {
            (Game1.Instance.CurrentState.InputSheet["Move"] as DirectionInput).Up -= _scrollUp;
            (Game1.Instance.CurrentState.InputSheet["Move"] as DirectionInput).Down -= _scrollDown;
            (Game1.Instance.CurrentState.InputSheet["Interact"] as Key).Pressed -= _enter;

            if (_options[_current].ConID.Equals("#this"))
                (_owner.MInteractModule as Dialog).SetNewDefault(this.ConversationID, this.ConversationID);
            else
                (_owner.MInteractModule as Dialog).SetNewDefault(_options[_current].ConID, this.ConversationID);

            (_owner.MInteractModule as Dialog).Interact(PlayerClasses.Player.Instance);
        }

        private void OnScrollUp(object sender, EventArgs e)
        {
            --_current;
            if (_current < 0)
                _current = 0;

            Text = DecryptOptionsToString();
            base.Trigger();
        }

        private void OnScrollDown(object sender, EventArgs e)
        {
            ++_current;
            if (_current >= _options.Count - 1)
                _current = _options.Count - 1;

            Text = DecryptOptionsToString();
            base.Trigger();
        }

        #endregion
    }
}

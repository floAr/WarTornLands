using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Entities;

namespace WarTornLands.Infrastructure.Systems.DialogSystem
{
    class Conversation
    {
        /// <summary>
        /// The ID of this conversation. Can be interpreted as the topic of the conversation.
        /// The ID will be shown to the player in the dialog window.
        /// Must be unique within the list of conversations in the repsective DialogModule.
        /// </summary>
        /// <value>
        /// The ID as string.
        /// </value>
        public string ID { get; private set; }
        public int Count
        {
            get { return _lines.Count; }
        }

        private List<ConversationItem> _lines;

        public Conversation(string id)
        {
            ID = id;
            _lines = new List<ConversationItem>();
        }

        private Conversation(string id, List<ConversationItem> lines)
        {
            ID = id;
            _lines = lines;
        }

        public Conversation Clone()
        {
            return new Conversation(ID, new List<ConversationItem>(_lines));
        }

        public ConversationIterator GetIterator()
        {
            return new ConversationIterator(this);
        }

        public void Add(ConversationItem item)
        {
            if (_lines.Count > 0)
            {
                if(_lines[_lines.Count - 1] is Options)
                    throw new ConversationAlreadyFinalisedException(this.ID, true);
                if(_lines[_lines.Count - 1] is IEndItem)
                    throw new ConversationAlreadyFinalisedException(this.ID, false);
            }

            item.SetID(ID);
            _lines.Add(item);
        }

        public ConversationItem this[int index]
        {
            get { return _lines[index]; }
        }

        public void SetOwner(Entity owner)
        {
            foreach (ConversationItem item in _lines)
            {
                item.SetOwner(owner);
            }
        }
    }

    public class ConversationAlreadyFinalisedException : Exception
    {
        public ConversationAlreadyFinalisedException(string conID, bool options)
            : base("The conversation " + conID + " already has an endpoint of type " + (options ? "Options." : "ComboBreaker.")) { }
    }

    public class ConversationNotFoundException : Exception
    {
        public ConversationNotFoundException(string calledID, string calledByID)
            : base("The conversation " + calledID + " was not found. It got called by the conversation " + calledByID) { }
    }

    public class ConversationHasEndedException : Exception
    {
        public ConversationHasEndedException(string calledID)
            : base("The conversation " + calledID + " has ended but a continuation was attempted.") { }
    }

    /// <summary>
    /// Iterator to access the next item in a conversation aggregate.
    /// Iterator Patter [GoF 19].
    /// </summary>
    class ConversationIterator
    {
        private Conversation _conversation;
        private int current = -1;

        public ConversationIterator(Conversation conversation)
        {
            _conversation = conversation;
        }

        public void Reset()
        {
            current = -1;
        }

        public ConversationItem First()
        {
            return _conversation[0];
        }

        public ConversationItem Next()
        {
            ConversationItem ret = null;
            if (current < _conversation.Count - 1)
                ret = _conversation[++current];
            else
                throw new ConversationHasEndedException(_conversation.ID);

            return ret;
        }

        public ConversationItem CurrentItem()
        {
            return _conversation[current];
        }

        public bool IsDone()
        {
            return current >= _conversation.Count ? true : false;
        }
    }
}

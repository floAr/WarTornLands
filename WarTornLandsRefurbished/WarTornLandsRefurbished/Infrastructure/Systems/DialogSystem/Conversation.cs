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

        public Conversation(string id, List<ConversationItem> lines)
        {
            // Gatekeeper method to report faulty conversation builds
            for (int i = 0; i < lines.Count; ++i)
            {
                if ((lines[i] is Options) && i != lines.Count - 1)
                    throw new Exception("Options can only stand at the end of a conversation.");
                if ((lines[i] is ComboBreaker) && i != lines.Count - 1)
                    throw new Exception("ComboBreakers can only stand at the end of a conversation.");
            }

            ID = id;
            _lines = lines;

            foreach (ConversationItem line in _lines)
            {
                line.SetID(ID);
            }
        }

        public ConversationIterator GetIterator()
        {
            return new ConversationIterator(this);
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
                throw new Exception("Conversation should'nt be continued. Something went wrong.");

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

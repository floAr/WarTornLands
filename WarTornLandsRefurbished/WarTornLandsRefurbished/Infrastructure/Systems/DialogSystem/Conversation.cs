using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarTornLands.Infrastructure.Systems.DialogSystem
{
    class Conversation
    {
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
        }

        public ConversationIterator GetIterator()
        {
            return new ConversationIterator(this);
        }

        public ConversationItem this[int index]
        {
            get { return _lines[index]; }
        }
    }

    /// <summary>
    /// Iterator to access the next item in a conversation aggregate.
    /// Iterator Patter [GoF 19].
    /// </summary>
    class ConversationIterator
    {
        private Conversation _conversation;
        private int current = 0;

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

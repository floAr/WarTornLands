using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarTornLands.World
{
    static class GlobalState
    {
        private static List<String> _triggers = new List<string>();
        private static Dictionary<string, object> _values = new Dictionary<string, object>();

        public static void RegisterValue<T>(string id, T defaultValue = default(T))
        {
            if (_values.ContainsKey(id))
                throw new Exception("ValueKey " + id + " is already registered");
            _values.Add(id, defaultValue);
        }
        public static void SetValue<T>(String id, T value)
        {
            if (_values.ContainsKey(id))
            {
                _values[id] = value;
            }
            else
                RegisterValue<T>(id, value);
        }
        public static T GetValue<T>(string id)
        {
            if (_values.ContainsKey(id))
            {
                return (T)_values[id];
            }
            else
                throw new Exception("Value "+id+" is not set");
        }
        public static void SetTrigger(string trigger)
        {
            _triggers.Add(trigger);
        }
        public static bool IsTriggered(string trigger)
        {
            return _triggers.Contains(trigger);
        }
    }
}

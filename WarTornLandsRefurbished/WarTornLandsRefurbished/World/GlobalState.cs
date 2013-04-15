using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WarTornLands.Infrastructure.Systems.SaveLoad;

namespace WarTornLands.World
{
    static class GlobalState
    {
        private static Vector2 _checkpointLocation = Vector2.Zero;
        private static List<String> _triggers = new List<string>();
        private static Dictionary<string, object> _values = new Dictionary<string, object>();

        public static void ApplySaveGame(SaveGameData data)
        {
            _checkpointLocation = data.CheckpointPosition;
            _triggers = data.Triggers;
            _values = new Dictionary<string, object>();
            for (int i = 0; i < data.ValuesS.Count; i++)
            {
                _values.Add(data.ValuesS[i], data.ValuesO[i]);
            }
        }
        public static void RegisterCheckpoint(Vector2 playerPosition)
        {
            _checkpointLocation = playerPosition;
        }

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

        public static List<string> Triggers { get{return _triggers;} }

        public static Dictionary<string, object> Values { get { return _values; } }

        public static Vector2 CheckpointLocation { get { return _checkpointLocation; } }
    }
}

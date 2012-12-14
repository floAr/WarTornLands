using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace WarTornLands.Infrastructure.Systems.InputSystem
{

    public class ControlSheet
    {
        private Dictionary<String, InputKey> _keys;

        public InputKey[] Keys { get { return _keys.Values.ToArray(); } }

        public Dictionary<String, InputKey> InputSheme { get { return _keys; } }

        public ControlSheet()
        {
            _keys = new Dictionary<string, InputKey>();
        }

        public void RegisterKey(String name, Keys binding)
        {
            Key k = new Key();
            k.SetActivator(binding);
            _keys.Add(name, k);
        }
        public void RegisterKey(String name, Buttons binding)
        {
            Key k = new Key();
            k.SetActivator(binding);
            _keys.Add(name, k);
        }
        public void RegisterKey(String name, Keys[] binding)
        {
            DirectionInput k = new DirectionInput();
            k.SetActivator(binding);
            _keys.Add(name, k);
        }
        public void RegisterKey(String name, TStick binding)
        {
            DirectionInput k = new DirectionInput();
            k.SetActivator(binding);
            _keys.Add(name, k);
        }

        public InputKey this[String keyName]
        {
            get
            {
                if (!_keys.ContainsKey(keyName))
                    throw new Exception("Key " + keyName + " is not registered yet.\nRegister key in Initialize of current GameState");
                return _keys[keyName];
            }
        }





    }
}

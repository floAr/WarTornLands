using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace WarTornLands
{
    class Key : InputKey
    {
        private int _held;
        private bool _value;
        private Keys _key;
        private Buttons _button;
        private int _mode;

        public void SetActivator(Keys key)
        {
            _key = key;
        }
        public void SetActivator(Buttons button)
        {
            _button = button;
        }

        public bool Value
        {
            get { return _value; }
        }

        public void Update(int mode)
        {
            if (mode == 0)
            {
                KeyboardState state = Keyboard.GetState();

                _value = state.IsKeyDown(_key);
            }

            if (mode == 1)
            {
                _value = GamePad.GetState(PlayerIndex.One).IsButtonDown(_button);
            }
        }
    }
}

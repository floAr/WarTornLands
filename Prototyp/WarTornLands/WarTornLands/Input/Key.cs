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
        public event EventHandler Pressed;

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
        public override int Held()
        {
            return _held;
        }

        public Key()
            : base()
        { }

        public override void Update(GameTime gt, KeyboardState oldKeys)
        {
            if (_mode == 0)
            {
                KeyboardState state = Keyboard.GetState();

                if (state.IsKeyDown(_key))
                {
                    _value = true;
                    _held += gt.ElapsedGameTime.Milliseconds;

                    if (oldKeys.IsKeyUp(_key))
                    {
                        if (Pressed != null)
                            Pressed(null, EventArgs.Empty);
                    }
                }
                else 
                {
                    _value = false;
                    _held = 0;
                }
            }

            if (_mode == 1)
            {
                if (GamePad.GetState(PlayerIndex.One).IsButtonDown(_button))
                {
                    _value = true;
                    _held += gt.ElapsedGameTime.Milliseconds;
                }
                else
                {
                    _value = false;
                    _held = 0;
                }
            }
        }

        public override void SetMode(int mode)
        {
            _mode = mode;
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace WarTornLands.Infrastructure.Systems.InputSystem
{
    public class Key : InputKey
    {
        public bool Value { get; private set; }
        
        private Keys _key;
        private Buttons _button;
        private InputMode _mode;
        
        public event EventHandler Pressed;


        public override void  SetMode(InputMode mode)
{
            _mode = mode;
        }
        public void SetActivator(Keys key)
        {
            _key = key;
        }
        public void SetActivator(Buttons button)
        {
            _button = button;
        }

        public Key()
            : base()
        { }

        public override void Update(GameTime gt, KeyboardState oldKeys)
        {
            // Keyboard ///
            if ((int)_mode == 0)
            {
                if(_key == null)
                    throw new Exception("Input keys not set.");
                
                KeyboardState state = Keyboard.GetState();

                if (state.IsKeyDown(_key))
                {
                    Value = true;
                    Held += gt.ElapsedGameTime.Milliseconds;

                    if (oldKeys.IsKeyUp(_key))
                    {
                        if (Pressed != null)
                            Pressed(null, EventArgs.Empty);
                    }
                }
                else
                {
                    Value = false;
                    Held = 0;
                }
            }
            /////////////

            // GamePad ///
            if ((int)_mode == 1)
            {
                if (GamePad.GetState(PlayerIndex.One).IsButtonDown(_button))
                {
                    Value = true;
                    Held += gt.ElapsedGameTime.Milliseconds;
                }
                else
                {
                    Value = false;
                    Held = 0;
                }
            }
            /////////////
        }

       
    }
}

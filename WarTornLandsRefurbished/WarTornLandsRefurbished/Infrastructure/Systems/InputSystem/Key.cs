using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace WarTornLands.Infrastructure.Systems.InputSystem
{
    public class Key 
    {
       /* public override int Held()
        {
            throw new NotImplementedException();
        }
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
            try
            {
                // Keyboard ///
                if (_mode == 0)
                {
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
                if (_mode == 1)
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
            catch { throw new Exception("Input keys not set."); }
        }

       */

       
    }
}

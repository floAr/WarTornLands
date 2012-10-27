using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace WarTornLands
{
    class Direction : InputKey
    {
        enum TStick { LEFT = 0, RIGHT = 1 }

        private Vector2 _value;

        private static Vector2 _up = new Vector2(0, -1);
        private static Vector2 _down = new Vector2(0, 1);
        private static Vector2 _left = new Vector2(-1, 0);
        private static Vector2 _right = new Vector2(1, 0);

        private Keys _keyUp = Keys.W;
        private Keys _keyDown = Keys.S;
        private Keys _keyLeft = Keys.A;
        private Keys _keyRight = Keys.D;

        private TStick _stick = 0;

        public Vector2 Value
        {
            get { return _value; }
        }

        public void Update(int mode)
        {
            if (mode == 0)
            {
                _value = CalcKeyboard();
            }

            if (mode == 1)
            {
                _value = CalcGamePad();
            }
        }

        private Vector2 CalcKeyboard()
        {
            KeyboardState state = Keyboard.GetState();

            Vector2 output = Vector2.Zero;

            if(state.IsKeyDown(_keyUp)) { output += _up; }
            if(state.IsKeyDown(_keyDown)) { output += _down; }
            if(state.IsKeyDown(_keyLeft)) { output += _left; }
            if(state.IsKeyDown(_keyRight)) { output += _right; }

            return output;
        }

        private Vector2 CalcGamePad()
        {
            Vector2 output = Vector2.Zero;

            if(_stick == TStick.LEFT)
            {
                output = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left;
            }

            if(_stick == TStick.RIGHT)
            {
                output = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right;
            }

            return output;
        }
    }
}

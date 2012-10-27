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

        private float _speed;
        private Vector2 _value;
        private int _held;
        private int _mode;
        private float _threshold = .2f;

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
        public float Speed
        {
            get { return _speed; }
        }
        public int Held()
        {
            return _held;
        }

        public void Update(GameTime gt)
        {
            if (_mode == 0)
            {
                _value = CalcKeyboard(gt);
            }

            if (_mode == 1)
            {
                _value = CalcGamePad(gt);
            }

            _value.Normalize();
        }

        private Vector2 CalcKeyboard(GameTime gt)
        {
            KeyboardState state = Keyboard.GetState();

            Vector2 output = Vector2.Zero;

            if (state.IsKeyDown(_keyUp) ||
                state.IsKeyDown(_keyDown) ||
                state.IsKeyDown(_keyLeft) ||
                state.IsKeyDown(_keyRight))
            {
                if (state.IsKeyDown(_keyUp)) { output += _up; }
                if (state.IsKeyDown(_keyDown)) { output += _down; }
                if (state.IsKeyDown(_keyLeft)) { output += _left; }
                if (state.IsKeyDown(_keyRight)) { output += _right; }

                _held += gt.ElapsedGameTime.Milliseconds;
            }
            else
            {
                _held = 0;
            }

            return output;
        }

        private Vector2 CalcGamePad(GameTime gt)
        {
            Vector2 output = Vector2.Zero;

            if(_stick == TStick.LEFT)
            {
                Vector2 temp = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left;
                if (temp.LengthSquared() >= _threshold * _threshold)
                {
                    output = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left;
                    _held += gt.ElapsedGameTime.Milliseconds;
                }
                else
                {
                    output = Vector2.Zero;
                    _held = 0;
                }
            }

            if(_stick == TStick.RIGHT)
            {
                Vector2 temp = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right;
                if (temp.LengthSquared() >= _threshold * _threshold)
                {
                    output = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right;
                    _held += gt.ElapsedGameTime.Milliseconds;
                }
                else
                {
                    output = Vector2.Zero;
                    _held = 0;
                }
            }

            return output;
        }

        public void SetMode(int mode)
        {
            _mode = mode;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace WarTornLands.Infrastructure.Systems.InputSystem
{
    public enum TStick 
    { 
        LEFT = 0, 
        RIGHT = 1 
    }

    public class DirectionInput : InputKey
    {
        public Vector2 Value { get; private set; }
        public event EventHandler Up;
        public event EventHandler Down;
        public event EventHandler Left;
        public event EventHandler Right;

        // Directions ///
        private readonly static Vector2 _up = new Vector2(0, -1);
        private readonly static Vector2 _down = new Vector2(0, 1);
        private readonly static Vector2 _left = new Vector2(-1, 0);
        private readonly static Vector2 _right = new Vector2(1, 0);
        /////////////////

        private InputMode _mode;
        private float _threshold = InputManager.GamePadTStickThreshold;

        private Keys _keyUp;
        private Keys _keyDown;
        private Keys _keyLeft;
        private Keys _keyRight;

        private TStick _stick = 0;

        public override void SetMode(InputMode mode)
        {
            _mode = mode;
        }
        public void SetActivator(Keys[] keySet)
        {
            if (keySet.Count() != 4)
                throw new Exception("Key-Set for DirectionInput is invalid.");

            _keyUp = keySet[0];
            _keyLeft = keySet[1];
            _keyDown = keySet[2];
            _keyRight = keySet[3];
        }
        public void SetActivator(TStick tStick)
        {
            _stick = tStick;
        }

        public DirectionInput()
            : base()
        {
            Value = Vector2.Zero;
        }

        public override void Update(GameTime gt, KeyboardState oldKeys)
        {
            try
            {
                if (_mode == InputMode.KEYBOARD)
                {
                    Value = CalcKeyboard(gt, oldKeys);
                }

                if (_mode == InputMode.GAMEPAD)
                {
                    Value = CalcGamePad(gt);
                }

                if (!Value.Equals(Vector2.Zero))
                    Value.Normalize();

            }
            catch { throw new Exception("Input keys not set."); }
        }

        private Vector2 CalcKeyboard(GameTime gt, KeyboardState oldKeys)
        {
            KeyboardState state = Keyboard.GetState();

            Vector2 output = Vector2.Zero;

            if (state.IsKeyDown(_keyUp) ||
                state.IsKeyDown(_keyDown) ||
                state.IsKeyDown(_keyLeft) ||
                state.IsKeyDown(_keyRight))
            {
                if (state.IsKeyDown(_keyUp)) { output += _up; 
                    if (oldKeys.IsKeyUp(_keyUp) && Up != null) Up(null, EventArgs.Empty); }
                if (state.IsKeyDown(_keyDown)) { output += _down; 
                    if (oldKeys.IsKeyUp(_keyDown) && Down != null) Down(null, EventArgs.Empty); }
                if (state.IsKeyDown(_keyLeft)) { output += _left; 
                    if (oldKeys.IsKeyUp(_keyLeft) && Left != null) Left(null, EventArgs.Empty); }
                if (state.IsKeyDown(_keyRight)) { output += _right; 
                    if (oldKeys.IsKeyUp(_keyRight) && Right != null) Right(null, EventArgs.Empty); }

                Held += gt.ElapsedGameTime.Milliseconds;
            }
            else
            {
                Held = 0;
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
                    Held += gt.ElapsedGameTime.Milliseconds;
                }
                else
                {
                    output = Vector2.Zero;
                    Held = 0;
                }
            }

            if(_stick == TStick.RIGHT)
            {
                Vector2 temp = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right;
                if (temp.LengthSquared() >= _threshold * _threshold)
                {
                    output = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right;
                    Held += gt.ElapsedGameTime.Milliseconds;
                }
                else
                {
                    output = Vector2.Zero;
                    Held = 0;
                }
            }

            return output;
        }
    }
}

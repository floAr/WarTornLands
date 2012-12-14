using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WarTornLands.Infrastructure.Systems.InputSystem;

namespace WarTornLands.Infrastructure
{
    public enum InputMode
    {
        KEYBOARD = 0,
        GAMEPAD = 1
    }

    public class InputManager : GameComponent
    {
        private static InputManager _instance;

        #region Default controls

        // Keyboard
        internal readonly static Keys KeyboardDefault_Hit = Keys.Enter;
        internal readonly static Keys KeyboardDefault_Jump = Keys.Space;
        internal readonly static Keys KeyboardDefault_Interact = Keys.T;
        internal readonly static Keys KeyboardDefault_UsePotion = Keys.P;
        internal readonly static Keys KeyboardDefault_Inventory = Keys.I;
        internal readonly static Keys KeyboardDefault_Quit = Keys.Escape;
        internal readonly static Keys[] KeyboardDefault_Move = { Keys.W, Keys.A, Keys.S, Keys.D };

        // GamePad

        internal readonly static float GamePadTStickThreshold = .1f;

        #endregion



        private readonly static InputMode _defaultMode = InputMode.KEYBOARD;
        private readonly InputMode _mode = _defaultMode;
        private readonly List<InputKey> _inputList = new List<InputKey>();
        private KeyboardState _oldKeys = Keyboard.GetState();

        private InputManager(Game game)
            : base(game)
        {

        }


        public static InputManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new InputManager(Game1.Instance);

                return _instance;
            }
        }

        public override void Update(GameTime gt)
        {
            foreach (InputKey ik in new List<InputKey>(_inputList))
            {
                ik.Update(gt, _oldKeys);
            }

            _oldKeys = Keyboard.GetState();
        }

        public void Subscribe(Key key, ref EventHandler handler)
        {
            key.Pressed += handler;
        }

        public void Unsubscribe(Key key, ref EventHandler handler)
        {
            key.Pressed -= handler;
        }

        public bool SetMapping(Microsoft.Xna.Framework.Input.Keys key, InputKey inputKey)
        {
            throw new System.NotImplementedException();
        }

        internal void RegisterControlSheet(ControlSheet _inputSheet)
        {
            _inputList.Clear();
            _inputList.AddRange(_inputSheet.Keys);
        }
    }
}

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

        // Keys /////////
        public Key KHit { get; private set; }
        public Key KJump { get; private set; }
        public Key KInteract { get; private set; }
        public Key KUsePotion { get; private set; }
        public Key KInventory { get; private set; }
        public Key KQuit { get; private set; }

        public DirectionInput KMove { get; private set; }
        /////////////////

        private readonly static InputMode _defaultMode = InputMode.KEYBOARD;
        private readonly InputMode _mode = _defaultMode;
        private readonly List<InputKey> _inputList = new List<InputKey>();
        private KeyboardState _oldKeys = Keyboard.GetState();

        private InputManager(Game game)
            : base(game)
        {
            KHit = new Key();
            KHit.SetActivator(KeyboardDefault_Hit);
            KJump = new Key();
            KJump.SetActivator(KeyboardDefault_Jump);
            KInteract = new Key();
            KInteract.SetActivator(KeyboardDefault_Interact);
            KUsePotion = new Key();
            KUsePotion.SetActivator(KeyboardDefault_UsePotion);
            KInventory = new Key();
            KInventory.SetActivator(KeyboardDefault_Inventory);
            KQuit = new Key();
            KQuit.SetActivator(KeyboardDefault_Quit);

            KMove = new DirectionInput();
            KMove.SetActivator(KeyboardDefault_Move);


            _inputList.Add(KHit);
            _inputList.Add(KJump);
            _inputList.Add(KInteract);
            _inputList.Add(KUsePotion);
            _inputList.Add(KMove);
            _inputList.Add(KInventory);
            _inputList.Add(KQuit);

            foreach (InputKey ik in _inputList)
            {
                ik.SetMode(_defaultMode);
            }
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
            foreach (InputKey ik in _inputList)
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
    }
}

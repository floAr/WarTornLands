using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WarTornLands
{
    public class InputManager : GameComponent
    {
        //public enum Inputmode
        //{
        //    KEYBOARD = 0,
        //    GAMEPAD = 1
        //};

        private Key _hit;
        private Key _jump;
        private Key _interact;
        private Key _potion;
        private Direction _move;
        private int _mode = 0;       // 0 = Keyboard, 1 = GamePad
        private List<InputKey> _inputList;

        KeyboardState _oldKeys = Keyboard.GetState();

        public InputManager(Game game)
            : base(game)
        {
            _inputList = new List<InputKey>();

            _hit = new Key();
            _hit.SetActivator(Keys.O);

            _jump = new Key();
            _jump.SetActivator(Keys.Space); ;

            _move = new Direction();

            _interact = new Key();
            _interact.SetActivator(Keys.T);

            _potion = new Key();
            _potion.SetActivator(Keys.P);

            _inputList.Add(_hit);
            _inputList.Add(_jump);
            _inputList.Add(_move);
            _inputList.Add(_interact);
            _inputList.Add(_potion);

            foreach(InputKey ik in _inputList)
            {
                int defaultMode = 0;     // 0 = Keyboard, 1 = GamePad
                ik.SetMode(defaultMode);
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

        internal Key Hit
        {
            get { return _hit; }
        }
        internal Key Jump
        {
            get { return _jump; }
        }
        internal Key Potion
        {
            get { return _potion; }
        }

        internal Direction Move
        {
            get { return _move; }
        }

        internal bool Speak
        {
            get { return _interact.Value;}
        }

        public Type GetService()
        {
            return null;
        }

        public bool SetMapping(Microsoft.Xna.Framework.Input.Keys key, InputKey inputKey)
        {
            throw new System.NotImplementedException();
        }

    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WarTornLands
{
    public class Inputmanager : GameComponent
    {
        //public enum Inputmode
        //{
        //    KEYBOARD = 0,
        //    GAMEPAD = 1
        //};

        private Key _hit;
        private Key _jump;
        private Direction _move;
        private int _mode = 0;       // 0 = Keyboard, 1 = GamePad
        private List<InputKey> _inputList;

        public Inputmanager(Game game)
            : base(game)
        {
            _inputList = new List<InputKey>();

            _hit = new Key();
            _hit.SetActivator(Keys.O);

            _jump = new Key();
            _jump.SetActivator(Keys.Space); ;

            _move = new Direction();

            _inputList.Add(_hit);
            _inputList.Add(_jump);
            _inputList.Add(_move);
        }

        public override void Update(GameTime gt)
        {
            foreach (InputKey ik in _inputList)
            {
                ik.Update(_mode);
            }
        }

        internal bool Hit
        {
            get { return _hit.Value; }
        }
        internal bool Jump
        {
            get { return _jump.Value; }
        }
        internal Vector2 Move
        {
            get { return _move.Value; }
        }

        public Type GetService()
        {
            return null;
        }

    }
}

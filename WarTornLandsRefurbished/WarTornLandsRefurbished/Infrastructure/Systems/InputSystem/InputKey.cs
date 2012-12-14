﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WarTornLands.Infrastructure.Systems.InputSystem
{
    public abstract class InputKey
    {
        public enum KeyType
        {
            Key,
            DirectionalInput
        }
        public InputKey()
        {
        }

        public abstract void Update(GameTime gt, KeyboardState oldKeys);
        public abstract void SetMode(InputMode mode);
        public int Held { get; protected set; }
    }
}

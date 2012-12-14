using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WarTornLands.Infrastructure.Systems.DrawSystem;
using WarTornLands.Infrastructure.Systems.InputSystem;

namespace WarTornLands.Infrastructure.Systems.GameState
{
    public abstract class BaseGameState
    {

        public bool IsInitializedAndLoaded = false;
        public virtual void Initialize()
        {
            InputManager.Instance.RegisterControlSheet(_inputSheet);
        }
        public virtual void LoadContent()
        {
            IsInitializedAndLoaded = true;
        }

        public abstract void Pause();
        public virtual void Resume()
        {
            InputManager.Instance.RegisterControlSheet(_inputSheet);
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);

        public ControlSheet InputSheet { get { return _inputSheet; } }

        public bool DrawingLights;

        protected DrawManager _drawManager=new DrawManager();

        protected ControlSheet _inputSheet = new ControlSheet();


    }
}

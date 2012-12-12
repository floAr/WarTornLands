using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WarTornLands.Infrastructure.Systems.GameState
{
    public abstract class BaseGameState
    {
        public abstract void Initialize();
        public abstract void LoadContent();

        public abstract void Pause();
        public abstract void Resume();

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);

        public bool DrawingLights;
    }
}

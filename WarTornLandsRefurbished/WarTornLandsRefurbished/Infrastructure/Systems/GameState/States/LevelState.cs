using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.World;

namespace WarTornLands.Infrastructure.Systems.GameState.States
{
    public class LevelState:BaseGameState
    {
        private GameSessionState _session;
        private Level _level;

        public override void Pause()
        {
            throw new NotImplementedException();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            _level.Update(gameTime);
            _session.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
           //Draw logic here
        }
    }
}

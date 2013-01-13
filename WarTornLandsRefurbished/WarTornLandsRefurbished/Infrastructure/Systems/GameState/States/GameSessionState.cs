using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.PlayerClasses;

namespace WarTornLands.Infrastructure.Systems.GameState.States
{
    class GameSessionState:BaseGameState
    {
        private Player _player;
        public Player Player
        {
            get { return _player; }
        }
        public override void Pause()
        {
           
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            throw new Exception("Draw should happen in the current LevelState, as this is just a container for global data");
        }
    }
}

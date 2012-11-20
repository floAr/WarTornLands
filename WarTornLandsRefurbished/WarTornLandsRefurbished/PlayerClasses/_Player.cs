using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WarTornLands.Counter;
using Microsoft.Xna.Framework.Graphics;
using WarTornLands.Infrastructure;
using WarTornLands.Entities;
using WarTornLands.Entities.Modules.Draw;
using Microsoft.Xna.Framework.Content;
using WarTornLands.Entities.Modules.Think;

namespace WarTornLands.PlayerClasses
{
    public class Player : Entity
    {
        private static Player _player;

        private Player(Game1 game)
            : base(game, Vector2.Zero, null, "Player")
        {
            this.LoadContent(game.Content);
            CM = new CounterManager();
            CM.Bang += new EventHandler<BangEventArgs>(OnBang);

            base._mThinkModule = new ThinkInputGuided(this);
        }

        public static Player GetInstance(Game1 game)
        {
            if (_player == null)
                _player = new Player(game);

            return _player;
        }

        private void LoadContent(ContentManager content)
        {
 
        }

        #region Subscribed events

        private void OnBang(object sender, BangEventArgs e)
        {

        }

        #endregion
    }
}

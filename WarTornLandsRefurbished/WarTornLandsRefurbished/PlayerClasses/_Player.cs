﻿using System;
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
            : base(game, new Vector2(0, 0),  "Player")
        {
            CM = new CounterManager();
            CM.Bang += new EventHandler<BangEventArgs>(OnBang);

            this.AddModule(new ThinkInputGuided(this));
            this.DrawOrder = 100;
        }

        public static Player GetInstance(Game1 game)
        {
            if (_player == null)
                _player = new Player(game);

            return _player;
        }

        protected override void LoadContent()
        {
            AnimatedDrawer animS = new AnimatedDrawer(Game.Content.Load<Texture2D>("character_64x128"));

            Animation anim = new Animation("walkDown");

            for (int i = 0; i < 4; i++)
                anim.AddFrame(new Rectangle(64 * i, 0, 64, 128));
            animS.AddAnimation(anim);

            animS.SetCurrentAnimation("walkDown");

            this.AddModule(animS);
        }

        /*
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            #if DEBUG
            (this.Game as Game1).SpriteBatch.Draw(
                WarTornLands.Entities.Modules.Think.Parts.SwingHitAbility.DweaponMarker,
                WarTornLands.Entities.Modules.Think.Parts.SwingHitAbility.WeaponPos - new Vector2(40,40),
                Color.White);
            #endif
        }
         * */

        #region Subscribed events

        private void OnBang(object sender, BangEventArgs e)
        {

        }

        #endregion

        internal void GiveItem(Items _loot)
        {
            throw new NotImplementedException();
        }
    }
}

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
using WarTornLands.Infrastructure.Systems.DialogSystem;

namespace WarTornLands.PlayerClasses
{
    public class Player : Entity
    {
        private static Player _instance;
        private Inventory _inventory;
        public static Player Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Player(Game1.Instance);

                return _instance;
            }
        }

        public Inventory Inventory
        {
            get { return _inventory; }
        }

        private Player(Game1 game)
            : base(game, new Vector2(0, 0),  "Player")
        {
            CM.Bang += new EventHandler<BangEventArgs>(OnBang);

           this.AddModule(new ThinkInputGuided(this));
        }

        protected override void LoadContent()
        {
            AnimatedDrawer animS = new AnimatedDrawer(Game.Content.Load<Texture2D>("character_64x128"));
            
            Animation anim = new Animation("walkDown");
            anim.AddFrame(new Rectangle(64 * 1, 0, 64, 128));
            anim.AddFrame(new Rectangle(64 * 2, 0, 64, 128));
            anim.AddFrame(new Rectangle(64 * 1, 0, 64, 128));
            anim.AddFrame(new Rectangle(64 * 3, 0, 64, 128));
            animS.AddAnimation(anim);
            animS.SetCurrentAnimation("walkDown");

            anim = new Animation("walkLeft");
            anim.AddFrame(new Rectangle(64 * 5, 0, 64, 128));
            anim.AddFrame(new Rectangle(64 * 6, 0, 64, 128));
            anim.AddFrame(new Rectangle(64 * 5, 0, 64, 128));
            anim.AddFrame(new Rectangle(64 * 7, 0, 64, 128));
            animS.AddAnimation(anim);

            anim = new Animation("walkRight");
            anim.AddFrame(new Rectangle(64 * 1, 128, 64, 128));
            anim.AddFrame(new Rectangle(64 * 2, 128, 64, 128));
            anim.AddFrame(new Rectangle(64 * 1, 128, 64, 128));
            anim.AddFrame(new Rectangle(64 * 3, 128, 64, 128));
            animS.AddAnimation(anim);

            anim = new Animation("walkUp");
            anim.AddFrame(new Rectangle(64 * 5, 128, 64, 128));
            anim.AddFrame(new Rectangle(64 * 6, 128, 64, 128));
            anim.AddFrame(new Rectangle(64 * 5, 128, 64, 128));
            anim.AddFrame(new Rectangle(64 * 7, 128, 64, 128));
            animS.AddAnimation(anim);

            anim=new Animation("standDown");
            anim.AddFrame(new Rectangle(0,0,64,128));
            animS.AddAnimation(anim);

            anim = new Animation("standLeft");
            anim.AddFrame(new Rectangle(4*64, 0, 64, 128));
            animS.AddAnimation(anim);

            anim = new Animation("standRight");
            anim.AddFrame(new Rectangle(0, 128, 64, 128));
            animS.AddAnimation(anim);

            anim = new Animation("standUp");
            anim.AddFrame(new Rectangle(4*64, 128, 64, 128));
            animS.AddAnimation(anim);

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

        internal void GiveItem(Items.Item _loot)
        {
            throw new NotImplementedException();
        }
    }
}

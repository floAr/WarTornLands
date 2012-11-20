using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Counter;
using WarTornLands;
using Microsoft.Xna.Framework;
using WarTornLands.Entities;
using WarTornLandsRefurbished.Entities.Modules.Think.Parts;

namespace WarTornLandsRefurbished.Entities.Modules.Think
{
    class ThinkInputGuided : IThinkModule
    {
        private CounterManager _cm;
        private Entity _owner;

        // Parts
        private JumpAbility _jump;
        private SwingHitAbility _swing;

        public ThinkInputGuided(Entity owner)
        {
            _cm = owner.CM;
            _cm.Bang += new EventHandler<BangEventArgs>(OnBang);

            _jump = new JumpAbility(_owner);

            _owner = owner;
            Game1 game = _owner.Game as Game1;

            // Subscribe to Input events
            game.Input.UsePotion.Pressed += new EventHandler(OnUsePotion);
            game.Input.ExecuteHit.Pressed += new EventHandler(OnExecuteHit);
            game.Input.Interact.Pressed += new EventHandler(OnInteract);
            game.Input.Jump.Pressed += new EventHandler(OnJump);
        }

        public void Update(GameTime gameTime)
        {
            _cm.Update(gameTime);
        }

        #region Subscribed events

        private void OnBang(object sender, BangEventArgs e)
        {

        }

        private void OnUsePotion(object sender, EventArgs e)
        {

        }

        private void OnExecuteHit(object sender, EventArgs e)
        {

        }

        private void OnInteract(object sender, EventArgs e)
        {
        }

        private void OnJump(object sender, EventArgs e)
        {
            _jump.TryExecute();
        }

        #endregion
    }
}

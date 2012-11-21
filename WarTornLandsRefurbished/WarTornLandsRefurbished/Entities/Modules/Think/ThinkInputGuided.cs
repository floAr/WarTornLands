using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Counter;
using WarTornLands;
using Microsoft.Xna.Framework;
using WarTornLands.Entities;
using WarTornLands.Entities.Modules.Think.Parts;
using WarTornLands.Infrastructure;

namespace WarTornLands.Entities.Modules.Think
{
    class ThinkInputGuided :BaseModule, IThinkModule
    {
        public float Speed { get; set; }
        public float Radius { get; set; }

        private CounterManager _cm;
        private InputManager _input;

        // Parts
        private JumpAbility _jump;
        private SwingHitAbility _swing;

        public ThinkInputGuided(Entity owner, float speed = .125f)
        {
            _owner = owner;
            Speed = speed;
            Game1 game = _owner.Game as Game1;

            _cm = owner.CM;
            _cm.Bang += new EventHandler<BangEventArgs>(OnBang);
            _input = game.Input;

            _jump = new JumpAbility(owner);

            // Subscribe to Input events
            _input.UsePotion.Pressed += new EventHandler(OnUsePotion);
            _input.ExecuteHit.Pressed += new EventHandler(OnExecuteHit);
            _input.Interact.Pressed += new EventHandler(OnInteract);
            _input.Jump.Pressed += new EventHandler(OnJump);
        }

        public void Update(GameTime gameTime)
        {
            _cm.Update(gameTime);

            Vector2 oldPos = _owner.Position;

            _owner.Position = CollisionManager.Instance.TryMove(_owner.Position,
                                                      _input.Move.Value * Speed * gameTime.ElapsedGameTime.Milliseconds,
                                                      Radius, _owner);
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

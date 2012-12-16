using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Counter;
using Microsoft.Xna.Framework;

namespace WarTornLands.Entities.Modules.Hit
{
    class DamageFlash : BaseModule, IHitModule
    {
        private bool _invulnerableAfterHit;

        // Counters //
        private CounterManager _cm;
        protected readonly string _cInvulnerable = "InvulnerableCounter";
        protected readonly int _invulnerableDuration = 1000;
        //////////////

        public override void SetOwner(Entity owner)
        {
            base.SetOwner(owner);

            _cm = owner.CM;
            _cm.AddCounter(_cInvulnerable, _invulnerableDuration);
            _cm.Bang += new EventHandler<BangEventArgs>(OnBang);
        }

        public int Damage(int damage)
        {
            // No damage if invulnerable
            if (_invulnerableAfterHit)
                return 0;

            _invulnerableAfterHit = true;
            _cm.StartCounter(_cInvulnerable);
            return damage;
        }

        private void OnBang(object sender, BangEventArgs e)
        {
            if (e.IsDesiredCounter(_cInvulnerable))
            {
                _invulnerableAfterHit = false;
            }
        }

        public bool IsFlashing()
        {
            return _invulnerableAfterHit;
        }
    }
}

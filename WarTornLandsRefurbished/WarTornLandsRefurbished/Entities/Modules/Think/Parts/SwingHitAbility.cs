using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Counter;
using WarTornLands.Entities;

namespace WarTornLandsRefurbished.Entities.Modules.Think.Parts
{
    class SwingHitAbility
    {
        /// <summary>
        /// Gets or sets the duration of a swing.
        /// </summary>
        /// <value>
        /// The duration in milliseconds.
        /// </value>
        public int Duration { get; set; }
        /// <summary>
        /// Gets or sets the expanse angle of the swing cone.
        /// </summary>
        /// <value>
        /// The angle in radians.
        /// </value>
        public float Angle { get; set; }
        /// <summary>
        /// Gets a value indicating whether this <see cref="SwingHitAbility" /> is executing a hit.
        /// </summary>
        /// <value>
        ///   <c>true</c> if executing; otherwise, <c>false</c>.
        /// </value>
        public bool Active
        {
            get { return _cm.GetPercentage(_cJump) != 0; }
        }

        private CounterManager _cm;
        private Entity _owner;

        // Counters
        private readonly string _cJump = "SwingHitCounter";

        /// <summary>
        /// Initializes a new instance of the <see cref="SwingHitAbility" /> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="duration">The duration of a swing.</param>
        /// <param name="angle">The angle of the swing cone.</param>
        public SwingHitAbility(Entity owner, int duration = 700, float angle = 20)
        {
            Duration = duration;
            Angle = angle;

            _cm = owner.CM;
            _cm.Bang += new EventHandler<BangEventArgs>(OnBang);

            _owner = owner;
        }

        #region Subscribed events

        private void OnBang(object sender, BangEventArgs e)
        {

        }

        #endregion
    }
}

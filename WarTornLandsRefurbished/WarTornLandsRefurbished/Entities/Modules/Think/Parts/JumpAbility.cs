using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Counter;
using WarTornLands.Entities;
using Microsoft.Xna.Framework;

namespace WarTornLands.Entities.Modules.Think.Parts
{
    /// <summary>
    ///     A part for a ThinkModule to give the respective entity a jump ability on TryExecute().
    ///     Parameters:
    ///     Duration (duration of a complete jump)
    ///     Zenit (max jump height in pixels)
    /// </summary>
    class JumpAbility : BaseAbility
    {
        /// <summary>
        /// Gets or sets the duration of a jump.
        /// </summary>
        /// <value>
        /// The duration in milliseconds.
        /// </value>
        public int Duration { get; set; }
        /// <summary>
        /// Gets or sets the max height of a jump.
        /// </summary>
        /// <value>
        /// The zenit in pixels.
        /// </value>
        public float Zenit { get; set; }
        /// <summary>
        /// Gets a value indicating whether this <see cref="JumpAbility" /> is executing a jump.
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
        private readonly string _cJump = "JumpCounter";

        /// <summary>
        /// Initializes a new instance of the <see cref="JumpAbility" /> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="duration">The duration of a jump.</param>
        /// <param name="zenit">The zenit of a jump.</param>
        public JumpAbility(Entity owner, int duration = 700, float zenit = 20)
        {
            Duration = duration;
            Zenit = zenit;

            _cm = owner.CM;
            _cm.Bang += new EventHandler<BangEventArgs>(OnBang);
            _cm.AddCounter(_cJump);

            _owner = owner;
        }

        /// <summary>
        /// Updates the part.
        /// May manipulate the Height value of respective owner.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public void Update(GameTime gameTime)
        {
            _cm.Update(gameTime);

            float perc = _cm.GetPercentage(_cJump);
            _owner.Height = 
                _owner.BaseHeight + 
                perc < .5f ? 2 * perc : 2 * (1 - perc) 
                * Zenit;
        }

        /// <summary>
        /// Tries to execute a jump.
        /// </summary>
        /// <returns>
        /// Returns true if the execution was successfull.
        /// </returns>
        public bool TryExecute()
        {
            if (_cm.GetPercentage(_cJump) == 0)
            {
                _cm.StartCounter(Duration, false, _cJump);
                return true;
            }
            else return false;
        }

        public bool TryCancel()
        {
            return false;
        }

        #region Subscribed events

        private void OnBang(object sender, BangEventArgs e)
        {

        }

        #endregion
    }
}

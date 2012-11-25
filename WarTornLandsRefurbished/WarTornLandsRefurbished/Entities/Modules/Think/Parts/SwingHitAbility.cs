using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Counter;
using WarTornLands.Entities;
using WarTornLands.World;
using Microsoft.Xna.Framework;

namespace WarTornLands.Entities.Modules.Think.Parts
{
    class SwingHitAbility : BaseAbility
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
        public double Angle
        {
            get { return 2 * _startAngle; }
            set { 
                    _startAngle = (value * .5);
                    _goalAngle = Math.PI * 2 - _startAngle * 2;
                }
        }
        /// <summary>
        /// Gets or sets the weapon range.
        /// </summary>
        /// <value>
        /// The weapon range (roughly in pixels).
        /// </value>
        public float Range { get; set; }
        /// <summary>
        /// Gets or sets the onhit damage.
        /// </summary>
        /// <value>
        /// The damage.
        /// </value>
        public float Damage { get; set; }
        /// <summary>
        /// Gets a value indicating whether this <see cref="SwingHitAbility" /> is executing a hit.
        /// </summary>
        /// <value>
        ///   <c>true</c> if executing; otherwise, <c>false</c>.
        /// </value>
        public bool Active
        {
            get { return _cm.GetPercentage(_cSwingHit) != 0; }
        }


        private CounterManager _cm;
        private Entity _owner;
        private Level _level;
        private double _startAngle;
        private double _goalAngle;

        // Counters
        private readonly string _cSwingHit = "SwingHitCounter";

        #if DEBUG
        static public Vector2 WeaponPos;
        static public Microsoft.Xna.Framework.Graphics.Texture2D DweaponMarker;
        #endif

        /// <summary>
        /// Initializes a new instance of the <see cref="SwingHitAbility" /> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="duration">The duration of a swing.</param>
        /// <param name="angle">The angle of the swing cone.</param>
        public SwingHitAbility(Entity owner, int duration = 700, float angle = 2, float range = 50, float damage = 5)
        {
            Duration = duration;
            Angle = angle;
            Range = range;
            Damage = damage;

            _cm = owner.CM;
            _cm.AddCounter(_cSwingHit, Duration, true);
            _cm.Bang += new EventHandler<BangEventArgs>(OnBang);
            _cm.Step += new EventHandler<BangEventArgs>(OnStep);

            _owner = owner;

            #if DEBUG
            DweaponMarker = owner.Game.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("weapontest");
            #endif

        }

        public bool TryExecute()
        {
            if (this.Active)
                return false;

            _cm.StartCounter(_cSwingHit, false);

            return true;
        }

        private void Hit()
        {
            if (_cm.GetPercentage(_cSwingHit) != 0)
            {
                float baseAngle = Constants.Player_WeaponStartAngle + (float)GetRoundedAngle();

                double maxAddition = _goalAngle - _startAngle;
                double finalAngle = _cm.GetPercentage(_cSwingHit) * maxAddition + baseAngle;
                Vector2 hitPos = _owner.Position + new Vector2(Range * (float)Math.Cos(finalAngle),
                                                Range * (float)Math.Sin(finalAngle));

                #if DEBUG
                WeaponPos = hitPos;
                #endif

                if (_owner is PlayerClasses.Player)
                {
                    /*
                    ENTITYAT(hitPos).Damage(Damage);
                    */
                }
                else
                {
                    /*
                    if (PLAYERAT(hitPos))
                    {
                        Player.Damage(Damage);
                    }
                    */
                }
            }
        }

        private double GetRoundedAngle()
        {
            switch (_owner.Face)
            {
                case Facing.LEFT: return 0;
                case Facing.UP: return 0.5 * Math.PI;
                case Facing.RIGHT: return Math.PI;
                case Facing.DOWN: return 1.5 * Math.PI;
            }
            return 0;
        }

        #region Subscribed events

        private void OnBang(object sender, BangEventArgs e)
        {
            
        }

        private void OnStep(object sender, BangEventArgs e)
        {
            if (e.IsDesiredCounter(_cSwingHit))
            {
                Hit();
            }
        }

        #endregion
    }
}

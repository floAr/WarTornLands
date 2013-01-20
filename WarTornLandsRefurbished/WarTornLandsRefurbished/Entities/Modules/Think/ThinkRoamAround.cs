using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Entities.Modules.Think;
using Microsoft.Xna.Framework;
using WarTornLands.Entities;
using WarTornLands.Infrastructure;
using WarTornLands.Entities.Modules;
using WarTornLands.Entities.Modules.Think.Parts;
using WarTornLands.PlayerClasses;
using System.Data;
using WarTornLands.Entities.AI;

namespace WarTornLands.Entities.Modules.Think
{
    public class ThinkRoamAround : BaseModule, IThinkModule
    {
        public float SightRange { get; set; }
        public float AttackRange 
        {
            get { return _swing.Range; }
            set { _swing.Range = value; } 
        }
        public int AttackDamage
        {
            get { return _swing.Damage; }
            set { _swing.Damage = value; }
        }

        private enum RoamState 
        {
            Idle = 0,
            Aggro = 1
        }

        private GoToPosition _goTo;
        private SwingHitAbility _swing;
        private ShooterAbility _shooter;
        private JumpAbility _jump;

        private Vector2 _anchor;
        private float _radius;
        private Random _rand;
        private RoamState _state;
        private RoamState _lastState;
        private Zone _zone;

        public ThinkRoamAround(Vector2 anchor, float roamingRadius, float attackRange = 60, float sightRange = 110, int damage = 10, bool canBeAttacked = true)
            : base()
        {
            _goTo = new GoToPosition(.5f);
            _swing = new SwingHitAbility(1500, 0f, attackRange, damage);
            _shooter = new ShooterAbility();
            _jump = new JumpAbility();
            _rand = new Random();
            _anchor = anchor;
            _radius = roamingRadius;
            _state = RoamState.Idle;
            SightRange = sightRange;
        }

        public ThinkRoamAround(DataRow data)
            : this(Vector2.Zero, float.Parse(data["RoamingRadius"].ToString()))
        {
            //TODO: Read data
        }

        public void SetZone(Zone zone)
        {
            _zone = zone;
        }

        public override void SetOwner(Entity owner)
        {
            base.SetOwner(owner);
            _anchor = owner.Position;
            _goTo.SetOwner(owner);
            _swing.SetOwner(owner);
            _jump.SetOwner(owner);
            _shooter.SetOwner(owner);
        }

        public void Update(GameTime gameTime)
        {
            _goTo.Update(gameTime);
            _swing.Update(gameTime);
            _jump.Update(gameTime);

            // TODO fix shooter :)
            /*_shooter.TryExecute();
            _shooter.Update(gameTime);*/

            CheckForTarget();

            switch (_state)
            {
                case RoamState.Idle:
                    IdleActions(gameTime);
                    break;
                case RoamState.Aggro:
                    AggroActions(gameTime);
                    break;
                default:
                    break;
            }
        }

        private void CheckForTarget()
        {
            _lastState = _state;

            if ((Player.Instance.Position - _owner.Position).LengthSquared() < SightRange * SightRange)
            {
                _state = RoamState.Aggro;
            }
            else
            {
                _state = RoamState.Idle;
            }
        }

        private void IdleActions(GameTime gameTime)
        {
            if (_lastState != RoamState.Idle)
                _goTo.Reset();

            if (!_goTo.Active)
            {
                _goTo.Thaw();
                _goTo.TryExecute();
                _goTo.TargetPosition = CreateTargetPosition();
            }
        }

        private void AggroActions(GameTime gameTime)
        {

            if (_lastState != RoamState.Aggro)
                _goTo.Reset();

            if (!_goTo.Active)
            {
                _goTo.TryExecute();
                _goTo.Bait = Player.Instance;
            }

            Vector2 rangeCheck = new Vector2(float.PositiveInfinity);
            if(_goTo.Bait != null)
                rangeCheck = _goTo.Bait.Position - _owner.Position;
            if (/*rangeCheck.Equals(Vector2.Zero) || */rangeCheck.LengthSquared() < AttackRange * AttackRange)
            {
                _goTo.Freeze();
                _swing.TryExecute();
            }
            else
            {
                // TODO remove
                _jump.TryExecute();
                Vector2 awayFromHome = _owner.Position - _anchor;
                if (awayFromHome.LengthSquared() >= _radius * _radius)
                    _goTo.Freeze();
                else
                    _goTo.Thaw();
            }
        }

        private Vector2 CreateTargetPosition()
        {
            Vector2 res = new Vector2((float)_rand.NextDouble(), (float)_rand.NextDouble());
            res.Normalize();

            if (_rand.NextDouble() > .5)
                res.X *= 1;
            if (_rand.NextDouble() > .5)
                res.Y *= -1;

            res *= (float)(_radius * (_rand.NextDouble() * .5 + .5));
            res += _anchor;

            return res;
        }
    }
}

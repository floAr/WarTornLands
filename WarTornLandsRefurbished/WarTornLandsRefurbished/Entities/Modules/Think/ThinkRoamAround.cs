﻿using System;
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

namespace WarTornLandsRefurbished.Entities.Modules.Think
{
    public class ThinkRoamAround : BaseModule, IThinkModule
    {
        public float SightRange { get; set; }

        private enum RoamState 
        {
            Idle = 0,
            Aggro = 1
        }

        private GoToPosition _goTo;

        private Vector2 _anchor;
        private float _radius;
        private Random _rand;
        private RoamState _state;
        private RoamState _lastState;

        public ThinkRoamAround(Entity owner, Vector2 anchor, float roamingRadius, float sightRange = 50)
            : base()
        {
            _goTo = new GoToPosition(.9f);
            _rand = new Random();
            _anchor = anchor;
            _radius = roamingRadius;
            _state = RoamState.Idle;
            SightRange = sightRange;
        }

        public override void SetOwner(Entity owner)
        {
            base.SetOwner(owner);
            _goTo.SetOwner(owner);
        }

        public void Update(GameTime gameTime)
        {
            _goTo.Update(gameTime);
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
                _goTo.Unfreeze();
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

            Vector2 awayFromHome = _owner.Position - _anchor;
            if (awayFromHome.LengthSquared() > _radius * _radius)
                _goTo.Freeze();
            else
                _goTo.Unfreeze();
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

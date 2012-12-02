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

        public ThinkRoamAround(Entity owner, Vector2 anchor, float radius)
            : base()
        {
            _goTo = new GoToPosition();
            _rand = new Random();
            _anchor = anchor;
            _radius = radius;
        }

        public void Update(GameTime gameTime)
        {
            _goTo.Update(gameTime);

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
            if (!_goTo.Active)
            {
                _goTo.TargetPosition = CreateTargetPosition();
            }
        }

        private void AggroActions(GameTime gameTime)
        {

        }

        private Vector2 CreateTargetPosition()
        {
            Vector2 res = new Vector2((float)_rand.NextDouble(), (float)_rand.NextDouble());
            res.Normalize();
            res *= _radius;

            if ((res - _anchor).LengthSquared() > _radius)
                res = CreateTargetPosition();

            return res;
        }
    }
}

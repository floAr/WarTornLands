using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WarTornLands.Counter;
using WarTornLands.Entities.Modules.Draw;
using Microsoft.Xna.Framework.Graphics;

namespace WarTornLands.Entities.Modules.Think.Parts
{
    class ShooterAbility : BaseAbility
    {
        private Entity _owner;
        private bool _cooldown;
        public int CooldownTime;
        public int Range;
        public float Speed;

        private List<Entity> _projectiles;

        // Counters
        private CounterManager _cm;
        private readonly string _cShooter = "ShooterCooldownCounter";

        public ShooterAbility(int cooldownTime = 1000, int range = 100, float speed = 1f)
        {
            _projectiles = new List<Entity>();
            CooldownTime = cooldownTime;
            Range = range;
            Speed = speed;
            _cooldown = false;
        }

        public bool TryExecute()
        {
            if (_cooldown)
                return false;

            // Start cooldown
            _cooldown = true;
            _cm.StartCounter(_cShooter);

            // Create projectile entity
            Entity p = new Entity(_owner.Position);
            StaticDrawer sd = new StaticDrawer();
            sd.Texture = Game1.Instance.Content.Load<Texture2D>("sprite/deadtree");
            p.AddModule(sd);
            p.Face = _owner.Face;
            _projectiles.Add(p);
            Game1.Instance.Level.AreaIndependentEntities.Add(p);

            return true;
        }

        public bool TryCancel()
        {
            return true;
        }

        public void Update(GameTime gameTime)
        {
            foreach (Entity p in _projectiles)
            {
                switch (p.Face)
                {
                    case Facing.DOWN:
                        p.Position += new Vector2(0, Speed * gameTime.ElapsedGameTime.Milliseconds);
                        break;
                    case Facing.UP:
                        p.Position -= new Vector2(0, Speed * gameTime.ElapsedGameTime.Milliseconds);
                        break;
                    case Facing.LEFT:
                        p.Position -= new Vector2(Speed * gameTime.ElapsedGameTime.Milliseconds, 0);
                        break;
                    case Facing.RIGHT:
                        p.Position += new Vector2(Speed * gameTime.ElapsedGameTime.Milliseconds, 0);
                        break;
                }

                // TODO remove entities that are too far away from THEIR starting point
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (Entity p in _projectiles)
            {
                p.Draw(gameTime);
            }
        }

        public void SetOwner(Entity owner)
        {
            _owner = owner;
            _cm = owner.CM;
            _cm.AddCounter(_cShooter, CooldownTime, true);
            _cm.Bang += new EventHandler<BangEventArgs>(OnBang);
        }

        #region Subscribed events

        private void OnBang(object sender, BangEventArgs e)
        {
            // Shooter is not on cooldown anymore
            if (e.ID.Equals(_cShooter))
                _cooldown = false;
        }

        #endregion
    }
}

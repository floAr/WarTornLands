using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WarTornLands.Counter;
using WarTornLands.Entities.Modules.Draw;
using Microsoft.Xna.Framework.Graphics;
using WarTornLands.Infrastructure.Systems;

namespace WarTornLands.Entities.Modules.Think.Parts
{
    class ShooterAbility : BaseAbility
    {
        private Entity _owner;
        private bool _cooldown;
        public int CooldownTime;
        public int Damage;
        public int Range;
        public float Speed;

        // Counters
        private CounterManager _cm;
        private readonly string _cShooter = "ShooterCooldownCounter";

        // Projectile pool
        Pool<Entity> _projectilePool;

        public ShooterAbility(int cooldownTime = 800, int damage = 1, int range = 400, float speed = 400f)
        {
            CooldownTime = cooldownTime;
            Range = range;
            Speed = speed;
            Damage = damage;
            _cooldown = false;
            _projectilePool = new Pool<Entity>(delegate() { return new Entity(Vector2.Zero); });
        }

        public bool TryExecute()
        {
            if (_cooldown)
                return false;

            // Start cooldown
            _cooldown = true;
            _cm.StartCounter(_cShooter);

            // Create projectile entity
            Entity p = _projectilePool.GetFreeItem();

            // Projectile specific setting
            p.Position = _owner.Position;
            p.Face = _owner.Face;
            p.Altitude = _owner.Altitude + _owner.BodyHeight * 0.5f; // place entity in center of the shooting weapon or guy
            p.BodyHeight = 0.03f; // 3 cm projectile height

            // Create draw module if it has none
            if (p.DrawModule == null)
            {
                StaticDrawer sd = new StaticDrawer();
                sd.Texture = Game1.Instance.Content.Load<Texture2D>("sprite/deadtree"); // TODO add custom textures
                p.AddModule(sd);
            }

            // Create think module if it has none
            if (p.ThinkModule == null)
            {
                // Add flying behaviour
                p.AddModule(new ThinkProjectileFly(_owner, Damage, Range, Speed));
            }

            // Add projectile to level
            Game1.Instance.Level.AreaIndependentEntities.Add(p);

            return true;
        }

        public bool TryCancel()
        {
            return true;
        }

        public void Update(GameTime gameTime)
        {
            // do nothing
        }

        public void Draw(GameTime gameTime)
        {
            // do nothing
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

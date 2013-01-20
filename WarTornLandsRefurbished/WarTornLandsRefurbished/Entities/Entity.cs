using System;
using Microsoft.Xna.Framework;
using WarTornLands.Counter;
using WarTornLands.Entities.Modules;
using WarTornLands.Entities.Modules.Collide;
using WarTornLands.Entities.Modules.Die;
using WarTornLands.Entities.Modules.Draw;
using WarTornLands.Entities.Modules.Draw.ParticleSystem;
using WarTornLands.Entities.Modules.Interact;
using WarTornLands.Entities.Modules.Think;
using WarTornLands.Infrastructure;
using WarTornLands.PlayerClasses;
using WarTornLands.Infrastructure.Systems.DrawSystem;
using WarTornLands.Entities.Modules.Hit;
using WarTornLands.Infrastructure.Systems.Camera2D;
using WarTornLands.Entities.AI;

namespace WarTornLands.Entities
{
    public enum Facing
    {
        LEFT,
        RIGHT,
        UP,
        DOWN
    }


    public class Entity : IComparable<Entity>, IDrawProvider, ISpatial
    {
        /* TODO
         * Interface als Events oder Methoden?
         * Events:
         *  Andere Klassen (Implementationen) definieren Verhalten und binden sich an 
         *  die Hooks der Entity Klasse. Events werden von außerhalb (z.B. onCollide) oder intern 
         *  (z.B. onDie) aufgerufen. 
         *      Probleme: Wie werden Daten durchgereicht? Große EventArgs?
         * 
         * Methoden:
         *  Andere Klassen sprechen direkt Methoden an, Entity entält die abstrakten Methoden
         *  (ist praktisch ein Inferface) und Implementation füllen Logik.
         *      Probleme: Wie verknpüfen wir in beide Richtungen (Spieler kennt Item-Entity
         *      aus Collision Manager, aber wie weiß das Item in wessen Inv. es soll)
         */


        // Flags ////////
        public bool ToBeRemoved { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsStatic { get; set; } // set to true if entity can not move or change altitude for performance
        public bool DropShadow { get; set; }
        /////////////////

        // Counters ///
        protected readonly string _cInvulnerable = "InvulnerableCounter";
        protected readonly int _invulnerableDuration = 1000;
        public readonly string _cHit = "HitCounter";
        ///////////////

        public string Categorie;

        public Vector2 Position {
            get { return _position; }
            set
            {
                _prevPosition = _position;
                _position = value;
            }
        }

        public Rectangle BoundingRect
        {
            get
            {
                // TODO new every time?
                return new Rectangle((int)(Position.X - Size.X / 2.0f), (int)(Position.Y - Size.Y), (int)Size.X, (int)Size.Y);
            }
        }

        public Vector2 BoundingRectCenter
        {
            get
            {
                // TODO new every time?
                return new Vector2(BoundingRect.Center.X, BoundingRect.Center.Y);
            }
        }

        public Vector2 InitialPosition { get; internal set; }
        public Point TilePosition { get; set; }

        /// <summary>
        /// Current altitude of the entity, e.g. 0 while standing or greater than 0 while jumping or flying.
        /// Value in meter.
        /// </summary>
        public float Altitude { get; internal set; }
        /// <summary>
        /// Body height of the entity. Value in meter.
        /// </summary>
        public float BodyHeight { get; internal set; }

        public int Health { get; internal set; }
        public int MaxHealth { get; internal set; }
        public string Name { get; internal set; }

        private Vector2 _prevPosition = Vector2.Zero;
        private Vector2 _position = Vector2.Zero;
        private bool _moving = false;
        public Facing Face
        {
            get { return _face; }
            set
            {
                _prevFace = _face;
                if (!FaceLock)
                {
                    _face = value;
                }
            }
        }
        private Facing _face;
        private Facing _prevFace;
        public bool FaceLock; // set to true if player Facing should not change

        internal CounterManager CM;

        #region CollideModule
        protected ICollideModule _collideModule;
        #endregion

        #region DrawModule
        protected IDrawExecuter _drawModule;

        private float _rotation;
        private float _scale;
        #endregion

        #region InteractModule
        protected IInteractModule _interactModule;
        #endregion

        #region ThinkModule
        protected IThinkModule _thinkModule;
        #endregion

        #region DieModule
        protected IDieModule _dieModule;
        #endregion

        #region HitModule
        protected IHitModule _hitModule;
        #endregion

        public Entity(Vector2 position, String name = "Entity")
        {
            Position = position;
            Health = 5;
            BodyHeight = 1.7f;
            Name = name;
            Face = Facing.DOWN;
            FaceLock = false;
            IsEnabled = true;
            IsStatic = false;
            DropShadow = false;

            CM = new CounterManager();
        }

        public void AddModule(BaseModule module)
        {
            if (module == null)
                return;

            module.SetOwner(this);
            if (module is IThinkModule)
                _thinkModule = module as IThinkModule;
            if (module is IDrawExecuter)
                _drawModule = module as IDrawExecuter;
            if (module is IInteractModule)
                _interactModule = module as IInteractModule;
            if (module is IDieModule)
                _dieModule = module as IDieModule;
            if (module is ICollideModule)
                _collideModule = module as ICollideModule;
            if (module is IHitModule)
                _hitModule = module as IHitModule;
        }

        public void SetZone(Zone zone)
        {
            if (_thinkModule != null)
                _thinkModule.SetZone(zone);
            else
                throw new Exception("Tried to provide an Entity without a ThinkModule with a roamzone.");
        }

        public IInteractModule InteractModule
        {
            get{return  _interactModule;}
        }

        public IDrawExecuter DrawModule
        {
            get{return  _drawModule;}
        }

        public IDieModule DieModule
        {
            get{return  _dieModule;}
        }

        public IThinkModule ThinkModule
        {
            get{return  _thinkModule;}
        }

        public ICollideModule CollideModule
        {
            get{return  _collideModule;}
        }

        public IHitModule HitModule
        {
            get { return _hitModule; }
        }

        public float Damage(int damage)
        {
            if (_hitModule == null)
                return 0;

            int dmgDone = Math.Min(_hitModule.Damage(damage), Health);
            this.Health -= dmgDone;
            return dmgDone;
        }

     /*   public void SetPosition(Vector2 position)
        {
            _prevPosition = Position;
            Position = position;
        }*/

        public void Reset(int health)
        {
            Position = InitialPosition;
            Health = health;
        }

        public Vector2 Size
        {
            get
            {
                if (_drawModule == null)
                {
                    return new Vector2(0, 0);
                }
                else
                {
                    return _drawModule.Size;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            // Return if single entity is not enabled, but someone calls update anyway
            if (!IsEnabled)
                return;

            // If animated entity, and we changed our moving state / facing
            if (this.DrawModule is AnimatedDrawer &&
                ((_prevPosition != _position) != _moving || _prevFace != Face))
            {
                // Update moving flag
                _moving = (_prevPosition != _position);

                if (!_moving) // stop
                {
                    switch (Face)
                    {
                        case Facing.DOWN:
                            (this.DrawModule as AnimatedDrawer).SetCurrentAnimation("standDown");
                            break;
                        case Facing.LEFT:
                            (this.DrawModule as AnimatedDrawer).SetCurrentAnimation("standLeft");
                            break;
                        case Facing.RIGHT:
                            (this.DrawModule as AnimatedDrawer).SetCurrentAnimation("standRight");
                            break;
                        case Facing.UP:
                            (this.DrawModule as AnimatedDrawer).SetCurrentAnimation("standUp");
                            break;
                    }
                }
                else // move on
                {
                    switch (Face)
                    {
                        case Facing.DOWN:
                            (this.DrawModule as AnimatedDrawer).SetCurrentAnimation("walkDown");
                            break;
                        case Facing.LEFT:
                            (this.DrawModule as AnimatedDrawer).SetCurrentAnimation("walkLeft");
                            break;
                        case Facing.RIGHT:
                            (this.DrawModule as AnimatedDrawer).SetCurrentAnimation("walkRight");
                            break;
                        case Facing.UP:
                            (this.DrawModule as AnimatedDrawer).SetCurrentAnimation("walkUp");
                            break;
                    }
                }
            }

            CM.Update(gameTime);

            #region Calc tilepos

            TilePosition = new Point((int)(Position.X / Constants.TileSize), (int)(Position.Y / Constants.TileSize));

            if (_drawModule is AnimatedDrawer)
                ((AnimatedDrawer)_drawModule).Update(gameTime);


            #endregion
            if (_interactModule != null)
                _interactModule.Update(gameTime);
            if (_thinkModule != null)
                _thinkModule.Update(gameTime);
            if (_dieModule != null && Health <= 0)
                _dieModule.Die();
            if (_drawModule != null)
            {
                _drawModule.Update(gameTime);
                if (_drawModule is AnimatedDrawer)
                    if (((AnimatedDrawer)_drawModule).HasEnded)
                    {
                        this._drawModule = null;
                        this.ToBeRemoved = true;
                    }
            }
        }

        public void Draw(GameTime gameTime)
        {
            DrawInformation information = new DrawInformation()
            {
                Position = this.Position,
                Rotation = _rotation,
                Scale = this.Size,
                Altitude = this.Altitude,
                Shadow = DropShadow,
                Flashing = _hitModule != null ? _hitModule.IsFlashing() : false,
                DrawLights=Game1.Instance.DrawingLights
            };

            if (_drawModule != null)
                _drawModule.Draw(Game1.Instance.SpriteBatch, information);
        }

        public void Interact(Entity user)
        {
            if(_interactModule != null)
                _interactModule.Interact(user);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns>True if entity is passable, false otherwise.</returns>
        public void Collide(Entity source)
        {
            if (_collideModule != null)
            {
                CollideInformation info = new CollideInformation() { Collider = source, IsPlayer = source is Player };
                _collideModule.OnCollide(info);
            }
        }

        public bool IsPassable(Entity source, float altitude, float bodyHeight)
        {
            if (_collideModule != null)
            {
                CollideInformation info = new CollideInformation() { Collider = source, IsPlayer = source is Player };
                return _collideModule.IsPassable(info);
            }

            return true;
        }

        internal void RemoveAllModules()
        {
            _collideModule = null;
            _interactModule = null;
            _thinkModule = null;
            _dieModule = null;
            _drawModule = null;
            _hitModule = null;
        }

        public int CompareTo(Entity other)
        {
            if (this.Position.Y < other.Position.Y)
                return -1;
            else if (this.Position.Y == other.Position.Y)
                return 0;
            else
                return 1;
        }
    }
}

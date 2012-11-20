using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using WarTornLands.Counter;
using Microsoft.Xna.Framework.Content;
using WarTornLands;
using WarTornLands.PlayerClasses;
using WarTornLands.Entities.Modules.Draw;
using WarTornLandsRefurbished.Entities.Modules.Interact;
using WarTornLandsRefurbished.Entities;
using WarTornLandsRefurbished.Entities.Modules.Think;
using WarTornLands.Entities.Modules.Die;

namespace WarTornLands.Entities
{
    public enum Facing
    {
        LEFT,
        RIGHT,
        UP,
        DOWN
    }



    public  class Entity : DrawableGameComponent
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
        public bool CanBeAttacked { get; protected set; }
        public bool CanSpeak { get; protected set; }
        public bool CanBeUsed { get; protected set; }
        public bool CanBePickedUp { get; protected set; }
        /////////////////

        // Counters ///
        protected readonly string _cHit = "HitCounter";
        ///////////////

        public Vector2 Position { get; set; }
        public Vector2 InitialPosition { get; internal set; }
        protected Point TilePosition { get; set; }
        public float Height { get; internal set; }
        public float BaseHeight { get; internal set; }
        public int Health { get; internal set; }
        public string Name { get; internal set; }

        internal CounterManager CM;

        #region DrawModule
        protected IDrawExecuter _mDrawModule;

        private float _rotation;
        private float _scale;
        #endregion

        #region InteractModule
        protected IInteractModule _mInteractModule;
        #endregion

        #region ThinkModule
        protected IThinkModule _mThinkModule;
        #endregion

        #region DieModule
        protected IDyingModule _mDieModule;
        #endregion


        public Entity(Game1 game, Vector2 position, IDrawExecuter drawExecuter, String name = "Entity")
            : base(game)
        {
            this.Position = position;
            this.Health = 1;
            this.Name = name;
            this._mDrawModule = drawExecuter;
        }

        public int Damage(int damage)
        {
            if (this.CanBeAttacked)
            {
                // TODO evtl Rüstungen abziehen
                this.Health -= Math.Min(damage, Health);
                return Math.Min(damage, Health);
            }
            else
            {
                return 0;
            }
        }

        public void SetPosition(Vector2 position)
        {
            Position = position;
        }

        public virtual void Reset(int health)
        {
            Position = InitialPosition;
            Health = health;
        }

        /// <summary>
        /// Gibt den Objekttyp als Zahl zurück.
        /// </summary>
        /// <returns>0 = keine Aktionmöglich, 1 = Angreifbar, 2 = Ansprechbar, 3 = Benutzbar(öffnen), 4 = Aufhebbar</returns>
        public int IdentifyObjectType()
        {
            if (CanBeAttacked) return 1;
            if (CanSpeak) return 2;
            if (CanBeUsed) return 3;
            if (CanBePickedUp) return 4;
            return 0;
        }

        public Vector2 Size
        {
            get
            {
                if (_mDrawModule == null)
                {
                    return new Vector2(0, 0);
                }
                else
                {
                    return _mDrawModule.Size;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            #region Calc tilepos

            TilePosition = new Point((int)(Position.X / Constants.TileSize), (int)(Position.Y / Constants.TileSize));

            if (_mDrawModule is AnimatedDrawer)
                ((AnimatedDrawer)_mDrawModule).Update(gameTime);


            #endregion
        }

        public override void Draw(GameTime gameTime)
        {
           // Point drawPos = new Point((int)this.GetDrawPosition().X, (int)this.GetDrawPosition().Y);

            DrawInformation information=new DrawInformation(){
                Position=this.Position,
                Rotation=_rotation,
                Scale=this.Size
            };

            _mDrawModule.Draw(((Game1)Game).SpriteBatch, information);
        }

      /*  protected virtual Vector2 GetDrawPosition()
        {
            Vector2 center = (Game as Game1).Player.Position;
            return new Vector2((this.Position.X - center.X - _texture.Width * 0.5f + (float)Math.Round((Game as Game1).Window.ClientBounds.Width / 2.0f)),
                                (this.Position.Y - center.Y - _texture.Height * 0.5f + (float)Math.Round((Game as Game1).Window.ClientBounds.Height / 2.0f)));
        }*/

        public virtual void OnDie()
        {
            // Fucking explode!!!!
        }

        public virtual void UseThis(Player player)
        { }

        public virtual void OnCollide(Entity source)
        {
           
        }
    }
}

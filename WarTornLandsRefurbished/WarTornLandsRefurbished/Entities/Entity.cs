using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using WarTornLands.Counter;
using Microsoft.Xna.Framework.Content;
using WarTornLands;
using WarTornLands.Entities.Implementations;
using WarTornLands.PlayerClasses;
using WarTornLands.Entities.Modules.Draw;

namespace WarTornLands.Entities
{
    public enum Facing
    {
        LEFT,
        RIGHT,
        UP,
        DOWN
    }



    public abstract class Entity : GameComponent
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
        protected readonly string _animCounter = "anim_counter";
        protected readonly string _hitCounter = "hit_counter";
        ///////////////

        public Vector2 Position { get; protected set; }
        public Vector2 InitialPosition { get; protected set; }
        protected Point TilePosition { get; protected set; }
        public int Health { get; protected set; }
        public string Name { get; protected set; }

        protected Texture2D _texture;
        protected float _radius;
        protected CounterManager _cm;
        protected float _weaponRange;

        #region DrawModule
        IDrawExecuter _drawModule;

        private float _rotation;
        private float _scale;
        #endregion


        public abstract event EventHandler<DieEventArgs> Die;


        public Entity(Game game, Vector2 position, Texture2D texture, String name = "Entity")
            : base(game)
        {
            this.Position = position;
            this._texture = texture;
            this.Health = 1;
            this.Name = name;
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
                if (_texture == null)
                {
                    return new Vector2(0, 0);
                }
                else
                {
                    return new Vector2(_texture.Width, _texture.Height);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            #region Calc tilepos

            TilePosition = new Point((int)(Position.X / Constants.TileSize), (int)(Position.Y / Constants.TileSize));

            if (_drawModule is AnimatedDrawer)
                ((AnimatedDrawer)_drawModule).Update(gameTime);


            #endregion
        }

        public override void Draw(GameTime gameTime)
        {
            Point drawPos = new Point((int)this.GetDrawPosition().X, (int)this.GetDrawPosition().Y);
            int width = (int)Math.Floor((double)(Game as Game1)._tileSetTexture.Width / Constants.TileSize);
            Vector2 size = Size();

            Rectangle drawRec = new Rectangle(
                            drawPos.X,
                            drawPos.Y,
                            (int)size.X, (int)size.Y);

            DrawInformation information=new DrawInformation(){
                Position=GetDrawPosition(),
                Rotation=_rotation,
                Scale=_scale
            }

            _drawModule.Draw(((Game1)Game).SpriteBatch,information)
        }

        protected virtual Vector2 GetDrawPosition()
        {
            Vector2 center = (Game as Game1).Player.GetPosition();
            return new Vector2((this.Position.X - center.X - _texture.Width * 0.5f + (float)Math.Round((Game as Game1).Window.ClientBounds.Width / 2.0f)),
                                (this.Position.Y - center.Y - _texture.Height * 0.5f + (float)Math.Round((Game as Game1).Window.ClientBounds.Height / 2.0f)));
        }

        public virtual void OnDie()
        {
            // Fucking explode!!!!
        }

        public virtual void UseThis(Player player)
        { }

        public virtual void OnCollide(Entity source)
        {
            // "einsammeln"
            if (_canBePickedUp)
            {
                if (source == (Game as Game1).Player)
                {
                    this._health = 0;

                    // TODO give player some item
                }
            }
        }
    }
}

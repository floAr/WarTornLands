using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarTornLands.Counter;
using Microsoft.Xna.Framework.Content;
using WarTornLands.PlayerClasses;

namespace WarTornLands.EntityClasses
{
    public enum Facing
    {
        LEFT,
        UP,
        RIGHT,
        DOWN
    }
    public enum AnimationType
    {
        MOVING,
        STANDING
    }

    public class Entity : DrawableGameComponent
    {
        protected Texture2D _texture;
        protected Vector2 _position;
        protected Point _tilepos;
        protected float _radius;

        //Animation////////////////////////7
        protected Facing _animFacing;
        protected AnimationType _animationType;
        protected Texture2D _animTexture;
        // Rectangle _animSource;
        protected Vector2 _animSource;
        protected Rectangle _animTarget;
        protected int _frame;
        /////////////////////////////////////

        // Bool Werte für Entity Eigenschaften
        protected bool _canBeAttacked;
        protected bool _canSpeak;
        protected bool _canBeUsed;
        protected bool _canBePickedUp;

        protected int _health;

        protected string _name;

        protected Texture2D _weaponTex;
        protected CounterManager _cm;
        protected float _weaponRange;
        protected string _hitCounter = "hit_counter";

        /// <summary>
        /// Gibt den Objekttyp als Zahl zurück.
        /// </summary>
        /// <returns>0 = keine Aktionmöglich, 1 = Angreifbar, 2 = Ansprechbar, 3 = Benutzbar(öffnen), 4 = Aufhebbar</returns>
        public int IdentifyObjektTyp()
        {
            if (_canBeAttacked) return 1;
            if (_canSpeak) return 2;
            if (_canBeUsed) return 3;
            if (_canBePickedUp) return 4;
            return 0;
        }

        public Entity(Game game, Vector2 position, Texture2D texture, String name="Entity")
            : base(game)
        {
            this._position = position;
            this._texture = texture;
            this._health = 1;
            this._name = name;
        }
        public String GetName()
        {
            return _name;
        }

        public bool CanBePickedUp()
        {
            return _canBePickedUp;
        }

        public bool CanSpeak()
        {
            return _canSpeak;
        }

        public bool CanBeUsed()
        {
            return _canBeUsed;
        }

        public virtual void LoadContent(ContentManager cm)
        { }

        public override void Update(GameTime gameTime)
        {
            #region Calc tilepos

            _tilepos = new Point((int)(_position.X / Constants.TileSize), (int)(_position.Y / Constants.TileSize));

            #endregion
        }

        public override void Draw(GameTime gameTime)
        {
            Point drawPos = new Point((int)this.GetDrawPosition().X, (int)this.GetDrawPosition().Y);
            int width = (int)Math.Floor((double)(Game as Game1)._tileSetTexture.Width / Constants.TileSize);
            Vector2 size = GetSize();

            Rectangle drawRec = new Rectangle(
                            drawPos.X,
                            drawPos.Y,
                            (int)size.X, (int)size.Y);

            (Game as Game1)._spriteBatch.Draw(_texture, drawRec, Color.White);
        }

        public Vector2 GetPosition()
        {
            return _position;
        }

        public virtual Vector2 GetDrawPosition()
        {
            Vector2 center = (Game as Game1)._player.GetPosition();
            return new Vector2((_position.X - center.X - _texture.Width * 0.5f + (float)Math.Round((Game as Game1).Window.ClientBounds.Width / 2.0f)),
                                (_position.Y - center.Y - _texture.Height * 0.5f + (float)Math.Round((Game as Game1).Window.ClientBounds.Height / 2.0f)));
        }

        public virtual Vector2 GetRelWeaponDrawPos()
        {
            if (_cm.GetPercentage(_hitCounter) != 0)
            {
                float baseAngle = Constants.Player_WeaponStartAngle + (float)GetRoundedAngle();

                float maxAddition = Constants.Player_WeaponGoalAngle - Constants.Player_WeaponStartAngle;
                float finalAngle = _cm.GetPercentage(_hitCounter) * maxAddition + baseAngle;
                Vector2 weaponPos = new Vector2(_weaponRange * (float)Math.Cos(finalAngle),
                                                _weaponRange * (float)Math.Sin(finalAngle));

                if ((Game as Game1)._currentLevel.IsPlayerAt(_position + weaponPos))
                {
                    (Game as Game1)._player.Damage(5);
                }

                return weaponPos;
            }

            return new Vector2(9001);   // Over 9k defines that the entity is not hitting atm
        }

        public double GetRoundedAngle()
        {
            switch (_animFacing)
            {
                case Facing.LEFT: return 0;
                case Facing.UP: return 0.5 * Math.PI;
                case Facing.RIGHT: return Math.PI;
                case Facing.DOWN: return 1.5 * Math.PI;
            }
            return 0;
        }

        public void SetPosition(Vector2 position)
        {
            _position = position;
        }

        public Vector2 GetSize()
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

        public int Damage(int damage)
        {
            if (this._canBeAttacked)
            {
                // TODO evtl Rüstungen abziehen
                this._health -= Math.Min(damage, _health);
                return Math.Min(damage, _health);
            }
            else
            {
                return 0;
            }
        }

        public int GetHealth()
        {
            return _health;
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
                if (source == (Game as Game1)._player)
                {
                    this._health = 0;

                    // TODO give player some item
                }
            }
        }
    }
}

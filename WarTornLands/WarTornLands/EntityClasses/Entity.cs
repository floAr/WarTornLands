using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarTornLands.Counter;

namespace WarTornLands.EntityClasses
{
    public class Entity : DrawableGameComponent
    {
        protected Texture2D _texture;
        protected Vector2 _position;
        protected Point _tilepos;

        // Bool Werte für Entity Eigenschaften
        protected bool _canbeattacked;
        protected bool _canspeak;
        protected bool _canbeused;
        protected bool _canbepickedup;

        protected int _health;

        /// <summary>
        /// Gibt den Objekttyp als Zahl zurück.
        /// </summary>
        /// <returns>0 = keine Aktionmöglich, 1 = Angreifbar, 2 = Ansprechbar, 3 = Benutzbar(öffnen), 4 = Aufhebbar</returns>
        public int IdentifyObjektTyp()
        {
            if (_canbeattacked) return 1;
            if (_canspeak) return 2;
            if (_canbeused) return 3;
            if (_canbepickedup) return 4;
            return 0;
        }

        public Entity(Game game, Vector2 position, Texture2D texture)
            : base(game)
        {
            this._position = position;
            this._texture = texture;
            this._health = 1;
        }

        public bool CanBePickedUp()
        {
            return _canbepickedup;
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 center = (Game as Game1)._player.GetPosition();
            int width = (int)Math.Floor((double)(Game as Game1)._tileSetTexture.Width / Constants.TileSize);
            Vector2 size = GetSize();

            Rectangle drawRec = new Rectangle((int)(_position.X - center.X - _texture.Width * 0.5f + (int)Math.Round((Game as Game1).Window.ClientBounds.Width / 2.0f)),
                            (int)(_position.Y - (int)center.Y - _texture.Height * 0.5f + (int)Math.Round((Game as Game1).Window.ClientBounds.Height / 2.0f)),
                            (int)size.X, (int)size.Y);

            (Game as Game1)._spriteBatch.Draw(_texture, drawRec, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            #region Calc tilepos

            _tilepos = new Point((int)(_position.X % Constants.TileSize), (int)(_position.Y % Constants.TileSize));

            #endregion


        }

        public Vector2 GetPosition()
        {
            return _position;
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
            if (this._canbeattacked)
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

        public virtual void OnCollide(Entity source)
        {
            // "einsammeln"
            if (_canbepickedup)
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

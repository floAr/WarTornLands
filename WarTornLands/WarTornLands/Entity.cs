using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WarTornLands
{
    public class Entity : DrawableGameComponent
    {
        private Texture2D _texture;
        private Vector2 _position;
        private Vector2 _offset;

        // Bool Werte für Entity Eigenschaften
        private bool _canbeattacked;
        private bool _canspeak;
        private bool _canbeused;
        private bool _canbepickedup;

        /// <summary>
        /// Gibt den Objekttyp als Zahl zurück.
        /// </summary>
        /// <returns>0 = keine Aktionmöglich, 1 = Angreifbar, 2 = Ansprechbar, 3 = Benutzbar(öffnen), 4 = Aufhebbar</returns>
        public int IdentifeyObjektTyp()
        {
            if(_canbeattacked) return 1;
            if(_canspeak) return 2;
            if(_canbeused) return 3;
            if (_canbepickedup) return 4;
            return 0;
        }

        public Entity(Game game, Vector2 position) : base(game)
        {
            this._position = position;
            this._offset = Vector2.Zero;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            ((SpriteBatch)Game.Services.GetService(typeof(SpriteBatch))).Draw(_texture, _position *Constants.TileSize + _offset, Color.White);
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
          //  (InputManager)Game.Services.GetService(typeof(InputManager)));
            if (_offset.X >= Constants.TileSize/2)
            {
                _offset.X -= Constants.TileSize;
                _position.X++;
            }
            if (_offset.Y >= Constants.TileSize/2)
            {
                _offset.Y -= Constants.TileSize;
                _position.Y++;
            }
            if (_offset.X <= -Constants.TileSize/2)
            {
                _offset.X += Constants.TileSize;
                _position.X--;
            }
            if (_offset.Y <= -Constants.TileSize/2)
            {
                _offset.Y += Constants.TileSize;
                _position.Y--;
            }
            base.Update(gameTime);
        }
      
    }
}

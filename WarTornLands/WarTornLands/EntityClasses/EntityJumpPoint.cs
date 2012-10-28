using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WarTornLands.EntityClasses
{
    class EntityJumpPoint : Entity
    {
        private Level _destinationLevel;
        private Vector2 _destinationPos;

        public EntityJumpPoint(Game game, Vector2 position, Texture2D texture, Level destinationLevel, Vector2 destinationPos)
            : base(game, position, texture)
        {
            _canbepickedup = false;

            _destinationLevel = destinationLevel;
            _destinationPos = destinationPos;
        }

        public override void OnCollide(Entity source)
        {
            // Nicht einsammeln, also nicht die Basisfunktion aufrufen!

            if (source == (Game as Game1)._player)
            {
                (Game as Game1).SetLevel(_destinationLevel);
                source.SetPosition(_destinationPos);
            }
        }
    }
}

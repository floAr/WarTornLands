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
        private EntityJumpPoint _destination;

        public EntityJumpPoint(Game game, Vector2 position, Texture2D texture)
            : base(game, position, texture)
        {
            _canbepickedup = true;
        }

        public override void OnCollide(Entity source)
        {
            // Nicht einsammeln, also nicht die Basisfunktion aufrufen!

            // TODO Level des _destination jump points laden
            // TODO (Game as Game1)._currentLevel = _destinationLevel
            // TODO Player an _destination.GetPosition() positionieren
        }
    }
}

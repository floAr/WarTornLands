using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WarTornLands.EntityClasses
{
    class EntityPotion : Entity
    {
        private Game _game;
        private Vector2 vektor;
        private Texture2D texture2D;

        public EntityPotion(Game game, Vector2 position, Texture2D texture) : base(game, position, texture)
        {
            _canbepickedup = true;
        }

        public override void OnCollide(Entity source)
        {
            // "einsammeln"
            if (_canbepickedup)
            {
                if (source == (Game as Game1)._player)
                {
                    (source as PlayerClasses.Player).GivePotion();
                }
            }

            base.OnCollide(source);
        }
    }
}

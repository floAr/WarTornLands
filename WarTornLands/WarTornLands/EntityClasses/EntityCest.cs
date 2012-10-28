using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WarTornLands.EntityClasses
{
    class EntityCest : Entity
    {
        private Game _game;
        private Vector2 vektor;
        private Texture2D texture2D;

        public EntityCest(Game game, Vector2 position, Texture2D texture) : base(game, position, texture)
        {
            _canbeused = true;
        }

        public override void OnCollide(Entity source)
        {
            // "einsammeln"
            if (_canbeused && (Game as Game1)._input.Interact)
            {
                if (source == (Game as Game1)._player)
                {
                    // Open cheast
                    (source as PlayerClasses.Player).GivePotion();

                }
            }

            base.OnCollide(source);
        }
    }
}

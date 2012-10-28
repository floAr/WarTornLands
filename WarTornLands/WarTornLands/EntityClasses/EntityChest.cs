using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarTornLands.PlayerClasses;

namespace WarTornLands.EntityClasses
{
    class EntityChest : Entity
    {
        private Game _game;
        private Vector2 vektor;
        private Texture2D texture2D;

        public EntityChest(Game game, Vector2 position, Texture2D texture) : base(game, position, texture)
        {
            _canBeUsed = true;
        }

        public override void UseThis(Player player)
        {
            // "einsammeln"
            if (player == (Game as Game1)._player)
            {
                // Open chest
                player.GivePotion();
            }

            base.OnCollide(player);
        }
    }
}

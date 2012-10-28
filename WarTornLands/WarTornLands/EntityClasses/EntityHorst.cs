using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WarTornLands.EntityClasses
{
    class EntityHorst : Entity
    {
         private Game _game;
        private Vector2 vektor;
        private Texture2D texture2D;

        public EntityHorst(Game game, Vector2 position, Texture2D texture, String name = "Horst") : base(game, position, texture)
        {
            _canSpeak = true;
            _name = name;
        }

        public override void OnCollide(Entity source)
        {
             

            base.OnCollide(source);
        }
    }
}

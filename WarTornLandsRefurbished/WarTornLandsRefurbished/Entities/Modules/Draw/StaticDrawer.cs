using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WarTornLands.Entities.Modules.Draw
{
    /// <summary>
    /// Static drawer class, to draw entities consisting of a single static texture
    /// </summary>
    public class StaticDrawer : IDrawExecuter
    {


        private Vector2 _loc;
        private Vector2 _size;
        private Texture2D _tex;

        /// <summary>
        /// Gets or sets the texture.
        /// </summary>
        /// <value>
        /// The texture.
        /// </value>
        public Texture2D Texture { get { return _tex; } set { _tex = value; _size = new Vector2(_tex.Width, _tex.Height); } }


        /// <summary>
        /// Draws the specified batch.
        /// </summary>
        /// <param name="batch">The batch.</param>
        /// <param name="information">The information.</param>
        public void Draw(SpriteBatch batch, DrawInformation information)
        {
            if (information.Centered)               
                _loc = information.Position ;
            else
                //add up because its centered anyway because the origin in the draw
                _loc = information.Position + (_size / 2);
            batch.Draw(Texture, new Rectangle((int)_loc.X, (int)_loc.Y, (int)_size.X, (int)_size.Y), new Rectangle(0, 0, (int)_size.X, (int)_size.Y), Color.White, information.Rotation, _size / 2, SpriteEffects.None, 0.5f);
        }


        public Vector2 Size
        {
            get { return new Vector2(_tex.Width, _tex.Height); }
        }
    }
}

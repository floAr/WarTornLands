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
    public class StaticDrawer :BaseModule, IDrawExecuter
    {
        private Vector2 _loc;
        private Vector2 _size;
        private Texture2D _tex;
        private bool _isLight = false;
        /// <summary>
        /// Gets or sets the texture.
        /// </summary>
        /// <value>
        /// The texture.
        /// </value>
        public Texture2D Texture { get { return _tex; } set { _tex = value; _size = new Vector2(_tex.Width, _tex.Height); } }
        public bool IsLight { get { return _isLight; } set { _isLight = value; } }

        /// <summary>
        /// Draws the specified batch.
        /// </summary>
        /// <param name="batch">The batch.</param>
        /// <param name="information">The information.</param>
        public void Draw(SpriteBatch batch, DrawInformation information)
        {
            if (_isLight != information.DrawLights)
                return;
            
            if (information.Centered)
                _loc = information.Position - (_size / 2);
            else
                _loc = information.Position;

            Vector2 center = Game1.Instance.Player.Position;
            Rectangle bounds = Game1.Instance.Window.ClientBounds;

            batch.Draw(Texture, new Rectangle((int)_loc.X - (int)center.X + (int)Math.Round(bounds.Width / 2.0f),
                (int)_loc.Y - (int)center.Y + (int)Math.Round(bounds.Height / 2.0f), (int)_size.X, (int)_size.Y),
                new Rectangle(0, 0, (int)_size.X, (int)_size.Y), Color.White, information.Rotation, _size / 2, SpriteEffects.None, 0.5f);
        }

        public Vector2 Size
        {
            get { return new Vector2(_tex.Width, _tex.Height); }
        }


        public void Update(GameTime gameTime)
        {
           
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarTornLands.PlayerClasses;

namespace WarTornLands.Infrastructure
{
    public class Interface : DrawableGameComponent
    {
        Texture2D dummyTexture;

        public Interface() : base(Game1.Instance)
        {
            DrawOrder = 1000;
        }

        protected override void LoadContent()
        {
            dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });
        }

        public override void Draw(GameTime time)
        {
            Game1 g = Game1.Instance;
            int border = 2;
            int maxWidth = 150;
            int width = (int)(Player.Instance.Health / (float)Player.Instance.MaxHealth * maxWidth);
            Vector2 start = new Vector2(g.Window.ClientBounds.Width - 10 - maxWidth, 10);
            g.SpriteBatch.Draw(dummyTexture, new Rectangle((int)start.X - border, (int)start.Y - border, maxWidth + 2 * border, 25 + 2 * border), Color.DarkRed);
            g.SpriteBatch.Draw(dummyTexture, new Rectangle((int)start.X, (int)start.Y, width, 25), Color.Red);
        }
    }
}

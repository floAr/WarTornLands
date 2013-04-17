using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarTornLands.Infrastructure.ResolutionIndependence;

namespace WarTornLands.Infrastructure
{
    public class Interface : DrawableGameComponent
    {
        //const float KeyIconScale = 1.0f;
        const float KeyStringSpacer = 10f;

        Vector2 _key1Pos = new Vector2(.02f, .21f);
        Vector2 _key2Pos = new Vector2(.02f, .27f);
        //Vector2 _fixPoint = new Vector2(.5f, .5f);

        float _keyWidth = .1f;

        Texture2D _dummyTexture;
        MagicSprite _normalKey;
        MagicSprite _masterKey;
        SpriteFont _keyFont;

        public Interface() : base(Game1.Instance)
        {
            DrawOrder = 1000;
        }

        protected override void LoadContent()
        {
            _dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
            _dummyTexture.SetData(new Color[] { Color.White });

            _normalKey = new MagicSprite(Game1.Instance.SpriteBatch, Game.Content.Load<Texture2D>("sprite/normal_key"), _key1Pos, _keyWidth, null, -1);
            _masterKey = new MagicSprite(Game1.Instance.SpriteBatch, Game.Content.Load<Texture2D>("sprite/normal_key"), _key2Pos, _keyWidth, null, -1);

            _keyFont = Game.Content.Load<SpriteFont>("font/keyFont");
        }

        public override void Draw(GameTime time)
        {
            Game1 g = Game1.Instance;
            int border = 2;
            int maxWidth = 150;
            int width = (int)(g.Player.Health / (float)g.Player.MaxHealth * maxWidth);
            Vector2 start = new Vector2(g.Window.ClientBounds.Width - 10 - maxWidth, 10);
            g.SpriteBatch.Draw(_dummyTexture, new Rectangle((int)start.X - border, (int)start.Y - border, maxWidth + 2 * border, 25 + 2 * border), Color.DarkRed);
            g.SpriteBatch.Draw(_dummyTexture, new Rectangle((int)start.X, (int)start.Y, width, 25), Color.Red);


            if (Game1.Instance.Level != null)
            {

                #region Draw Keys

                _normalKey.Draw();
                g.SpriteBatch.DrawString(_keyFont, PlayerClasses.Player.Instance.Inventory.GetNormalKeys.ToString(), _normalKey.Position + new Vector2(KeyStringSpacer + _normalKey.Width, 0), Color.White);

                _masterKey.Draw();
                g.SpriteBatch.DrawString(_keyFont, PlayerClasses.Player.Instance.Inventory.GetNormalKeys.ToString(), _masterKey.Position + new Vector2(KeyStringSpacer + _masterKey.Width, 0), Color.White);

                #endregion

            }
        }
    }
}

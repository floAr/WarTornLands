using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace WarTornLands.DEBUG
{
    public class ScreenLogComponent : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteFont _logFont;
        SpriteBatch _logSpriteBatch;
        float _lineHeight;

        public Vector2 DisplayPosition;
        public Color DisplayColor;

        const int DISPLAY_LINE_COUNT = 10;
        static List<string> _logStrings = new List<string>(DISPLAY_LINE_COUNT);
        static GameTime _lastTime;

        public ScreenLogComponent(Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            DisplayPosition = new Vector2((float)Game.GraphicsDevice.PresentationParameters.BackBufferWidth * 0.05f, (float)Game.GraphicsDevice.PresentationParameters.BackBufferHeight * 0.9f);
            DisplayColor = Color.Magenta;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _logFont = Game.Content.Load<SpriteFont>("font/debug");
            _logSpriteBatch = new SpriteBatch(Game.GraphicsDevice);
            _lineHeight = _logFont.MeasureString("A").Y;
        }

        public static void AddLog(String log)
        {
            _logStrings.Insert(0, String.Format("[{0}]: {1}", _lastTime.TotalGameTime, log));
            while (_logStrings.Count >= DISPLAY_LINE_COUNT)
            {
                _logStrings.RemoveAt(_logStrings.Count - 1);
            }
        }

        public override void Update(GameTime gameTime)
        {
            _lastTime = gameTime;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            Vector2 drawPosition = DisplayPosition;
            _logSpriteBatch.Begin();
            foreach (String log in _logStrings)
            {
                _logSpriteBatch.DrawString(_logFont, log, drawPosition, DisplayColor);
                drawPosition.Y -= _lineHeight;
            }
            _logSpriteBatch.End();
        }
    }
}
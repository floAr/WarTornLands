using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Infrastructure.Systems;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using WarTornLands;
using WarTornLands.PlayerClasses;
using WarTornLands.Infrastructure.Systems.DialogSystem;
using System.IO;

namespace WarTornLands.Infrastructure
{
    public class DialogManager : DrawableGameComponent
    {
        private SpriteBatch _spriteBatch;
        private Texture2D _textBox;
        private SpriteFont _font;
        private ConversationItem _currentDisplay;
        private static Vector2 _topLeftPosition;
        private readonly static Vector2 _relBoxPosition = new Vector2(0, -10);
        // TODO display speaker name
        //private readonly static Vector2 _relSpeakerNamePosition = new Vector2(10, 10);
        private readonly static Vector2 _relTextPosition = new Vector2(20, 15);
        private readonly static float _lineSpacing = -3;
        private static float _lineLength;    // in pixels

        // Text format commands
        // Example: /n executes a line wrap
        private readonly static char commandIndicator = '/';
        private readonly static char lineWrap = 'n';


        #region Singleton Stuff
        private static DialogManager _instance;

        public static DialogManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DialogManager(Game1.Instance.Player);
                }
                return _instance;
            }
        }
        #endregion

        private DialogManager(Player player)
            : base(Game1.Instance)
        {
            this.DrawOrder = 500;
            _spriteBatch = Game1.Instance.SpriteBatch;
        }

        protected override void LoadContent()
        {
            _textBox = Game1.Instance.Content.Load<Texture2D>("sprite/dialogbox");
            _lineLength = _textBox.Width - 2 * _relTextPosition.X;
            _font = Game1.Instance.Content.Load<SpriteFont>("font/DialogFont");
            _topLeftPosition = new Vector2(GraphicsDeviceManager.DefaultBackBufferWidth * .5f - _textBox.Width * .5f + _relBoxPosition.X,
                                           GraphicsDeviceManager.DefaultBackBufferHeight - _textBox.Height + _relBoxPosition.Y);

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            if (_currentDisplay != null)
            {
                ShowConversation();
            }

            base.Draw(gameTime);
        }

        private void ShowConversation()
        {
            Game1 game = Game1.Instance;
            string text = _currentDisplay.Text;
            Vector2 pos = Vector2.Zero;

            game.SpriteBatch.Draw(game.Content.Load<Texture2D>("sprite/dialogbox"),
                _topLeftPosition, Color.White);

            while (text.Length > 0)
            {
                string next = NextWord(ref text, ref pos);

                // If the next word would exeed line length wrap it
                if (pos.X + _font.MeasureString(next).X > _lineLength)
                {
                    Wrap(ref pos);
                }
                if (pos.X + _font.MeasureString(next).X > _lineLength)
                    throw new Exception("Word is too long for a line to contain it: " + next + " Line length is set to " + _lineLength + ".");

                Color drawColor = Catalog.Instance.CheckString(next);

                next = next.Replace('~', ' ');
                game.SpriteBatch.DrawString(_font, next, pos + _topLeftPosition + _relTextPosition, drawColor);

                pos += new Vector2(_font.MeasureString(next).X, 0);
            }
        }

        private string NextWord(ref string text, ref Vector2 position)
        {
            int i = 0;
            string res = "";

            while (i <= text.Length)
            {
                // Check if the next symbol is a space or terminating a sentence
                if (i == text.Length || text[i].Equals(' ') || text[i].Equals('.') || text[i].Equals('!') || text[i].Equals('?'))
                {
                    // If the read-in just started cut the symbol off
                    if (i == 0)
                    {
                        if (text[i].Equals(' ') && position.X + _font.MeasureString(text[i].ToString()).X > _lineLength)
                        {
                            // In the special case that a SPACE would stand at the beginning of a line, dont show it
                        }
                        else
                            res += text[i];

                        text = text.Remove(0, i + 1);                            
                    }
                    else
                    {
                        text = text.Remove(0, i);
                    }
                    break;
                }

                // Check for command
                if (text[i].Equals(commandIndicator))
                {
                    // If the read in just started execute command, else return the current word
                    if (i != 0)
                    {
                        text = text.Remove(0, i);
                        break;
                    }

                    // Check for lineBreak
                    if (text[i + 1].Equals(lineWrap))
                    {
                        text = text.Remove(0, i + 2);
                        Wrap(ref position);
                        break;
                    }
                }

                res += text[i];
                ++i;
            }

            return res;
        }

        private void Wrap(ref Vector2 position)
        {
            position += new Vector2(0, _lineSpacing + _font.MeasureString("Ig").Y);
            position.X = 0;
        }

        #region Subscribed events

        public void CallDialog(ConversationItem statement)
        {
            if (statement == null)
            {
                _currentDisplay = null;
                Player.Instance.Enabled = true;
            }
            else
            {
                _currentDisplay = statement;
                Player.Instance.Enabled = false;
            }
        }

        #endregion
    }
}
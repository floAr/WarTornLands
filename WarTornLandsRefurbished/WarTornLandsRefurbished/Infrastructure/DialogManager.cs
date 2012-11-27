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

namespace WarTornLands.Infrastructure
{
    public class DialogManager : DrawableGameComponent
    {
        private SpriteBatch _spriteBatch;
        private Texture2D _textBox;
        private SpriteFont _font;
        private ConversationItem _currentDisplay;
        private string _currentSpeaker;
        private readonly static Vector2 _boxPosition = new Vector2(0, 180);
        private readonly static Vector2 _textPosition = new Vector2(20, 200);

        public event EventHandler ConversationEnded;

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
            _textBox = Game1.Instance.Content.Load<Texture2D>("dialogbox");
            _font = Game1.Instance.Content.Load<SpriteFont>("Test");

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
            _spriteBatch.Begin();

            _spriteBatch.Draw(_textBox, _boxPosition, Color.White);
            _spriteBatch.DrawString(_font, _currentDisplay.Text, _textPosition, Color.White);

            _spriteBatch.End();
        }

        #region Subscribed events

        public void CallDialog(string speaker, ConversationItem statement)
        {
            if (statement is ComboBreaker)
            {
                _currentDisplay = null;
                _currentSpeaker = "";

                if(ConversationEnded != null)
                    ConversationEnded(null, EventArgs.Empty);
            }
            else
            {
                _currentDisplay = statement;
                _currentSpeaker = speaker;
            }
        }

        #endregion
    }
}

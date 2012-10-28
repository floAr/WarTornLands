using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WarTornLands
{
    public class DialogSystem : DrawableGameComponent
    {
        #region Variable
        XML_Parser _parser;
        Game1 _game;
        List<String> _dialog;
        bool _dialogstarted;
        SpriteFont _font;
        #endregion

        public bool isdialogstarted()
        {
            return _dialogstarted;
        }

        public DialogSystem(Game1 game) : base(game)
        {
            this._game = game;
            this._parser = new XML_Parser(game);
            this._dialogstarted = false;
            this._font = _game.Content.Load<SpriteFont>("Test");
        }

        public void SpeakwithPerson(String entityname, bool speakmodus, int textstelle)
        {
            _parser.SetFilename(entityname);
            _parser.LoadText();
            _dialog = _parser.GetDialouge(speakmodus, textstelle);
            this._dialogstarted = true;
        }

        public void EndDialog()
        {
            this._dialogstarted = false;
        }

        public void DrawText()
        {
            //show dialog at 0.
            (Game as Game1)._spriteBatch.Begin();
            (Game as Game1)._spriteBatch.DrawString(_font, _dialog.First(), new Vector2(5, 5), Color.Black);
            (Game as Game1)._spriteBatch.End();
        }

        public void NextText()
        {
            _dialog.RemoveAt(0);
            if (_dialog.Count <= 0)
            {
                EndDialog();
            }
        }
    }
}

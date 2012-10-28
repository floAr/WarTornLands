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
        Texture2D _dialogbox;
        Vector2 _position;
        int _textbreite;
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
            this._dialogbox = _game.Content.Load<Texture2D>("dialogbox");
        }

        public void TestDialog(bool ButtonStatus, PlayerClasses.Player player, Level level)
        {
            if (ButtonStatus)
            {
                if (!_dialogstarted)
                {
                    _position = player.GetPosition();
                    if (player.GetRoundedAngle() == 0)
                    {
                        _position.X -= 30;
                    }
                    else
                    {
                        if (player.GetRoundedAngle() == 0.5 * Math.PI)
                        {
                            _position.Y -= 30;
                        }
                        else
                        {
                            if (player.GetRoundedAngle() == Math.PI)
                            {
                                _position.X += 30;
                            }
                            else
                            {
                                if (player.GetRoundedAngle() == 0.5 * Math.PI)
                                {
                                    _position.Y += 30;
                                }
                            }
                        }
                    }
                    EntityClasses.Entity ent = level.GetEntityAt(_position);
                    if (ent == null)
                    {

                    }
                    else
                    {
                        try
                        {
                            SpeakwithPerson(ent.GetName(), ent.GetPosition(), true, 0);
                        }catch(Exception e)
                        {

                        }
                    }
                }
                else
                {
                    NextText();
                }
            }
        }

        public void SpeakwithPerson(String entityname,Vector2 position, bool speakmodus, int textstelle)
        {
            _parser.SetFilename(entityname);
            _parser.LoadText();
            _dialog = _parser.GetDialouge(speakmodus, textstelle);
            _textbreite = 0;
            foreach (String text in _dialog)
            {
                if (_textbreite < text.Length)
                {
                    _textbreite = text.Length;
                }
            } 
            _position = position;
            this._dialogstarted = true;
        }

        public void EndDialog()
        {
            this._dialogstarted = false;
        }

        public void DrawText()
        {
            if (_dialogstarted)
            {
                //show dialog at 0.
                (Game as Game1)._spriteBatch.Begin();
                _game._spriteBatch.Draw(_dialogbox, new Rectangle((int)_position.X, (int)_position.Y, _textbreite * 12, 100), Color.White);
                _game._spriteBatch.DrawString(_font, _dialog.First(), _position + new Vector2(5, 5), Color.White);
                (Game as Game1)._spriteBatch.End();
            }
        }

        public void NextText()
        {

            if (_dialog.Count >= 1)
            {
                _dialog.RemoveAt(0);
            }
            if (_dialog.Count <= 0)
            {
                EndDialog();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarTornLands.EntityClasses;

namespace WarTornLands
{
    public class DialogSystem : DrawableGameComponent
    {
        #region Variable
        XML_Parser _parser;
        Game1 Game;
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
            this._parser = new XML_Parser(game);
            this._dialogstarted = false; 
            this._font = Game.Content.Load<SpriteFont>("Test");
            this._dialogbox = Game.Content.Load<Texture2D>("dialogbox");
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
                        _position.X -= Constants.Player_TalkDistance;
                    }
                    else
                    {
                        if (player.GetRoundedAngle() == 0.5 * Math.PI)
                        {
                            _position.Y -= Constants.Player_TalkDistance;
                        }
                        else
                        {
                            if (player.GetRoundedAngle() == Math.PI)
                            {
                                _position.X += Constants.Player_TalkDistance;
                            }
                            else
                            {
                                if (player.GetRoundedAngle() == 1.5 * Math.PI)
                                {
                                    _position.Y += Constants.Player_TalkDistance;
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
                            if (ent.CanSpeak())
                            {
                                SpeakwithPerson(ent.GetName(), ent.GetPosition(), true, 0);
                            }
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
                (Game as Game1)._spriteBatch.Draw(_dialogbox, new Rectangle((int)_position.X, (int)_position.Y, _textbreite * 12, 100), Color.White);
                (Game as Game1)._spriteBatch.DrawString(_font, _dialog.First(), _position + new Vector2(5, 5), Color.White);
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

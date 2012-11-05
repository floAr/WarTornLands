using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Infrastructure.Systems;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using WarTornLands;
using WarTornLandsRefurbished.World;
using WarTornLandsRefurbished.Entities;
using WarTornLands.PlayerClasses;

namespace WarTornLandsRefurbished.Infrastructure
{
    class DialogManager : GameComponent
    {
        private static DialogManager _dialogManager;

        public bool DialogStarted { get; private set; }

        private XML_Parser _parser;
        private List<String> _dialog;
        private SpriteFont _font;
        private Texture2D _dialogbox;
        private Vector2 _position;
        private int _textbreite;

        private DialogManager(Game game) 
            : base(game)
        {
            this._parser = (game as Game1).XMLParser; 

            (Game as Game1).Input.Interact.Pressed += new EventHandler(TestDialog);
        }

        public static DialogManager GetInstance(Game game)
        {
            if (_dialogManager == null)
                _dialogManager = new DialogManager(game);

            return _dialogManager;
        }

        public void LoadContent()
        {
            this._font = Game.Content.Load<SpriteFont>("Test");
            this._dialogbox = Game.Content.Load<Texture2D>("dialogbox");
        }

        #region Subsribed events

        public void TestDialog(object sender, EventArgs e)
        {
            Player player = (Game as Game1).Player;
            Level level = (Game as Game1).Level;

            if (!DialogStarted)
            {
                _position = player.Position;
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

                Entity ent = level.GetEntityAt(_position);
                if (ent == null)
                {

                }
                else
                {
                    try
                    {
                        if (ent.CanSpeak())
                        {
                            SpeakWithPerson(ent.GetName(), ent.GetPosition(), true, 0);
                        }
                        if (ent.CanBeUsed())
                        {
                            ent.UseThis(player);
                        }
                    }
                    catch
                    {

                    }
                }
            }
            else
            {
                NextText();
            }

        }

        #endregion
    }
}

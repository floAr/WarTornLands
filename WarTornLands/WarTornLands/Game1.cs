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
using WarTornLands.PlayerClasses;

namespace WarTornLands
{
    /// <summary>
    /// Dies ist der Haupttyp für Ihr Spiel
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;
        public Texture2D _tileSetTexture;
        public Texture2D _treeTexture;
        public Texture2D _deadTreeTexture;
        public Texture2D _gruselUteTexture;
        public Texture2D _blackHoleTexture;
        //GameServiceContainer services;
        public InputManager _input;
        public Player _player;
        XML_Parser _parser;
        public Level _currentLevel;
        DialogSystem _dialogSystem;
        KeyboardState prevkey;
        KeyboardState key;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Ermöglicht dem Spiel, alle Initialisierungen durchzuführen, die es benötigt, bevor die Ausführung gestartet wird.
        /// Hier können erforderliche Dienste abgefragt und alle nicht mit Grafiken
        /// verbundenen Inhalte geladen werden.  Bei Aufruf von base.Initialize werden alle Komponenten aufgezählt
        /// sowie initialisiert.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Fügen Sie Ihre Initialisierungslogik hier hinzu

            base.Initialize();
        }

        /// <summary>
        /// LoadContent wird einmal pro Spiel aufgerufen und ist der Platz, wo
        /// Ihr gesamter Content geladen wird.
        /// </summary>
        protected override void LoadContent()
        {
            // Erstellen Sie einen neuen SpriteBatch, der zum Zeichnen von Texturen verwendet werden kann.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _tileSetTexture = Content.Load<Texture2D>("grass");
            _treeTexture = Content.Load<Texture2D>("tree");
            _deadTreeTexture = Content.Load<Texture2D>("deadtree");
            _gruselUteTexture = Content.Load<Texture2D>("gruselute");
            _blackHoleTexture = Content.Load<Texture2D>("blackhole");


            _input = new InputManager(this);

            _player = new Player(this);
            _parser = new XML_Parser(this);
            _dialogSystem = new DialogSystem(this);
            _dialogSystem.SpeakwithPerson("Horst", true, 0);
            //_parser.SetFilename("Horst");
            //_parser.SaveText();
            _parser.SetFilename("0");

            //_parser.SetLevel();
            //_parser.SaveLevel();
            try
            {
                _parser.LoadLevel();
                _currentLevel = _parser.GetLevel();
            }
            catch (Exception e)
            {
                _currentLevel = new Level(this);
                int[,] layer0 = new int[,] {
                            {1,1,1,1,1,1,1,1},
                            {1,9,9,9,9,9,9,1},
                            {1,9,9,9,9,9,9,1},
                            {1,9,9,54,111,111,9,1},
                            {1,9,9,9,111,111,9,1},
                            {1,9,9,9,9,54,9,1},
                            {1,9,9,9,9,9,9,1},
                            {1,1,1,1,1,1,1,1}};
                int[,] layer1 = { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 87 } };
                layer0[1, 5] = 65;
                _currentLevel.AddFloor(layer0);
                _currentLevel.AddCeiling(new int[0, 0]);
            } 
            PlayerClasses.CollisionDetector.Setup(_currentLevel);

            this.Components.Add(_input);

            _player.LoadContent(Content);
            
            // TODO: Verwenden Sie this.Content, um Ihren Spiel-Inhalt hier zu laden
        }

        /// <summary>
        /// UnloadContent wird einmal pro Spiel aufgerufen und ist der Ort, wo
        /// Ihr gesamter Content entladen wird.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Entladen Sie jeglichen Nicht-ContentManager-Inhalt hier
        }

        /// <summary>
        /// Ermöglicht dem Spiel die Ausführung der Logik, wie zum Beispiel Aktualisierung der Welt,
        /// Überprüfung auf Kollisionen, Erfassung von Eingaben und Abspielen von Ton.
        /// </summary>
        /// <param name="gameTime">Bietet einen Schnappschuss der Timing-Werte.</param>
        protected override void Update(GameTime gameTime)
        {
            // Ermöglicht ein Beenden des Spiels
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            _player.Update(gameTime);
            _currentLevel.Update(gameTime); 
            key = Keyboard.GetState();
            if (key.IsKeyDown(Keys.T) && !prevkey.IsKeyDown(Keys.T))
            {
                _dialogSystem.NextText();
            }
            prevkey = key;

            base.Update(gameTime);
        }

        /// <summary>
        /// Dies wird aufgerufen, wenn das Spiel selbst zeichnen soll.
        /// </summary>
        /// <param name="gameTime">Bietet einen Schnappschuss der Timing-Werte.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Fügen Sie Ihren Zeichnungscode hier hinzu

            // Kapseln in eigene Klasse, für Menüs etc.
            _currentLevel.Draw(gameTime, 0);
            _currentLevel.DrawEntities(gameTime);
            _player.Draw(gameTime);
            _currentLevel.Draw(gameTime, 1);

            // Test für Textmenue
            if (_dialogSystem.isdialogstarted())
            {
                _dialogSystem.DrawText();
            }
            base.Draw(gameTime);
        }
    }
}

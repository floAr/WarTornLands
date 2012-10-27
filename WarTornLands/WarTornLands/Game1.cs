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
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public Texture2D TileSetTexture;
        //GameServiceContainer services;
        public InputManager input;
        Player player;
        XML_Parser _parser;
        public Level testLevel; // TODO remove

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            input = new InputManager(this);

            player = new Player(this);
            _parser = new XML_Parser(this);
            _parser.SetFilename("0");
            _parser.SetLevel();
            _parser.SaveLevel();
            try
            {
                _parser.LoadLevel();
                testLevel = _parser.GetLevel();
            }catch(Exception e)
            {
                testLevel = new Level(this);
                int[,] layer0 = new int[,] {
                            {1,1,1,1,1,1,1,1},
                            {1,9,9,9,9,9,9,1},
                            {1,9,9,9,9,9,9,1},
                            {1,9,9,54,111,111,9,1},
                            {1,9,9,9,111,111,9,1},
                            {1,9,9,9,9,54,9,1},
                            {1,9,9,9,9,9,9,1},
                            {1,1,1,1,1,1,1,1}};
            int[,] layer1 = {{0,0,0,0},{0,0,0,0},{0,0,0,87}};
                layer0[1, 5] = 65;
                testLevel.AddLayer(0, layer0);
            }
            testLevel.AddLayer(1, layer1);
            testLevel.AddLayer(2, new int[0, 0]);

            PlayerClasses.CollisionDetector.Setup(testLevel);

            this.Components.Add(testLevel);
            this.Components.Add(input);
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
            spriteBatch = new SpriteBatch(GraphicsDevice);

            TileSetTexture = Content.Load<Texture2D>("dg_grounds32");
            player.LoadContent(Content);
            
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

            // TODO: Fügen Sie Ihre Aktualisierungslogik hier hinzu

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
            testLevel.Draw(gameTime, 0);
            player.Draw(gameTime);
            testLevel.Draw(gameTime, 1);
            testLevel.Draw(gameTime, 2);

            base.Draw(gameTime);
        }
    }
}

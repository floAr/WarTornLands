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
using WarTornLands.Infrastructure;

using WarTornLands.Infrastructure.Systems;
using WarTornLandsRefurbished.World;
using WarTornLandsRefurbished.Infrastructure;
using WarTornLands.Entities.Modules.Draw;
using WarTornLands.Entities;

namespace WarTornLands
{
    /// <summary>
    /// Dies ist der Haupttyp für Ihr Spiel
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager _graphics;

        public SpriteBatch SpriteBatch { get; private set; }
        public InputManager Input { get; private set; }
        Entity staticTest;
        Entity dynamicTest;
        //    public Player Player { get; private set; }
        //     public XML_Parser XMLParser { get; private set; }
        //    public DialogManager DialogManager { get; private set; }
        //    public Interface Interface { get; private set; }
        //    public Level Level { get; private set; }        


        //public Texture2D _tileSetTexture;
        //public Texture2D _treeTexture;
        //public Texture2D _deadTreeTexture;
        //public Texture2D _gruselUteTexture;
        //public Texture2D _blackHoleTexture;
        //public Texture2D _potionTexture;
        //public Texture2D _cestTexture;
        //GameServiceContainer services;

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
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            /*  TextureCatalog.LoadContent(Content);

              Input = InputManager.GetInstance(this);
              //Interface = new Interface(this);

              Player = Player.GetInstance(this);
          //    XMLParser = XML_Parser.GetInstance(this);
              DialogManager = DialogManager.GetInstance(this);
              //_parser.SetFilename("Horst");
              //_parser.SaveText();
           //   XMLParser.SetFilename("0");

              //_parser.SetLevel();
              //_parser.SaveLevel();
              try
              {
                  XMLParser.Load();
                  Level = XMLParser.GetLevel();
              }
              catch (Exception e)
              {

              } 

              PlayerClasses.CollisionManager.Setup(Level);

              this.Components.Add(Input);

              Level.LoadContent();

              Player.LoadContent(Content);
             * */
            StaticDrawer sd = new StaticDrawer();

            sd.Texture = Content.Load<Texture2D>("blackhole");

            staticTest = new Entity(this, new Vector2(10, 10), sd, "loch");

            AnimatedDrawer animS = new AnimatedDrawer(Content.Load<Texture2D>("character_64x128"));

            Player player = null;
            player.Height = 10;

            Animation anim = new Animation("walkDown");

            for (int i = 0; i < 4; i++)
                anim.AddFrame(new Rectangle(64 * i, 0, 64, 128));
            animS.AddAnimation(anim);

            animS.SetCurrentAnimation("walkDown");

            dynamicTest = new Entity(this, new Vector2(250, 250), animS, "Ute");

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
            staticTest.Update(gameTime);
            dynamicTest.Update(gameTime);

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
            SpriteBatch.Begin();
            staticTest.Draw(gameTime);
            dynamicTest.Draw(gameTime);

            SpriteBatch.End();

            base.Draw(gameTime);
        }


    }
}

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
using WarTornLands.Entities.Modules.Draw;
using WarTornLands.Entities;
using WarTornLands.Entities.Modules.Die;
using WarTornLands.Entities.Modules.Draw.ParticleSystem;
using WarTornLands.World; 

namespace WarTornLands
{
    /// <summary>
    /// Dies ist der Haupttyp für Ihr Spiel
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager _graphics;

        public SpriteBatch SpriteBatch { get; private set; }
        Entity staticTest;
        Entity particleTest;
        Entity dynamicTest;

        public Player Player { get; private set; }

        //public Interface Interface { get; private set; }
        public Level Level { get; private set; }
  

        private  static Game1 _instance = new Game1();

        public static Game1 Instance
        {
            get
            {
                return _instance;
            }
        }

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

            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Level = new Level(this);
            Level.LoadTestLevel();

            this.Components.Add(InputManager.Instance);
            Player = Player.Instance;
            Player.DrawOrder = 100;
            this.Components.Add(Player);
            this.Components.Add(DialogManager.Instance);

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

            //Add Default Explosion Module
            Catalog.Instance.SetupTestCatalog();

            StaticDrawer sd = new StaticDrawer();
            

            sd.Texture = Content.Load<Texture2D>("blackhole");

            staticTest = new Entity(this, new Vector2(50, 50), "loch");
            staticTest.AddModule(sd);
            staticTest.AddModule(new ExplodeAndLoot(new PlayerClasses.Items.Item(PlayerClasses.Items.ItemTypes.Potion)));
            staticTest.Health = 100;
            List<Texture2D> pL= new List<Texture2D>();
            pL.Add(Content.Load<Texture2D>("flame3"));
            particleTest = new Entity(this, new Vector2(150,300));
            ParticleSystem pSystem = new ParticleSystem(
                new EmitterSetting()
                {
                    DirectionX = new Range() { Min = -1, Max = 1 },
                    DirectionY = new Range() { Min = -1, Max = -3 },
                    AnglePermutation = new Range() { Min = -1, Max = 1 },
                    Lifetime = new Range() { Min = 1000, Max = 2500 },
                    MaxParticles = new Range(250),
                    Size = new Range() { Min = 0.1f, Max = 0.3f },
                    SpeedX = new Range() { Min = -1, Max = 1 },
                    SpeedY = new Range() { Min = -1, Max = -3 },
                    Alpha=new Range(1),
                    AlphaDecay=new Range(0.01f,0.1f)
                    
                },
        pL, new Vector2(150, 500));
            particleTest.AddModule(pSystem);
                


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

        int counter = 0;
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
            particleTest.Update(gameTime);
            counter += gameTime.ElapsedGameTime.Milliseconds;
            if (counter > 1000)
            {
                staticTest.Health -= 10;
                counter -= 1000;
            }

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
       
            particleTest.Draw(gameTime);
            staticTest.Draw(gameTime);
            particleTest.Draw(gameTime);
            base.Draw(gameTime);

            SpriteBatch.End();
        }


    }
}

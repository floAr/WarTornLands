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
using WarTornLands.Infrastructure.Systems.SkyLight; 

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

        private bool _drawLights = false;
        public bool DrawingLights { get { return _drawLights; } set { _drawLights = value; } }

        public Player Player { get; private set; }

        //public Interface Interface { get; private set; }
        public Level Level { get; private set; }

        private BackBuffer _BackBuffer;

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
            //Level.LoadTestLevel();
            Level.LoadChristmasCaverns();

            this.Components.Add(InputManager.Instance);
            Player = Player.Instance;
            Player.DrawOrder = 100;
            Player.Position = new Vector2(13 * Constants.TileSize, 20 * Constants.TileSize);
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
        pL);
            particleTest.AddModule(pSystem);

            _BackBuffer = new BackBuffer(GraphicsDevice, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight));

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

        int drawcounter = 0;
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

        int counter = 0;
        bool foreward = true;
        /// <summary>
        /// Dies wird aufgerufen, wenn das Spiel selbst zeichnen soll.
        /// </summary>
        /// <param name="gameTime">Bietet einen Schnappschuss der Timing-Werte.</param>
        /// 
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            /*
            // TODO: Fügen Sie Ihren Zeichnungscode hier hinzu

            // Kapseln in eigene Klasse, für Menüs etc.
            SpriteBatch.Begin();
       
            particleTest.Draw(gameTime);
            staticTest.Draw(gameTime);
            particleTest.Draw(gameTime);
            base.Draw(gameTime);

            SpriteBatch.End();*/
            if(foreward)
            drawcounter += gameTime.ElapsedGameTime.Milliseconds;
            else
                drawcounter -= gameTime.ElapsedGameTime.Milliseconds;
            if (drawcounter <= 0)
            {
                foreward = true;
                drawcounter = 0;
            }
            if (drawcounter >= 15000)
            {
                foreward = false;
            }
            Color fill = new Color(drawcounter / 100, drawcounter / 100, drawcounter / 100 );

            DrawingLights = true;
            // Set the render target
            GraphicsDevice.SetRenderTarget(_BackBuffer.SourceMap);

            GraphicsDevice.Clear(new Color(150, 150, 150)); //Set the Background of the SourceMap to Black (Important!)

            SpriteBatch.Begin(SpriteSortMode.Deferred, CustomBlendState.Add);
            SpriteBatch.Draw(Content.Load<Texture2D>("light"), new Rectangle(-20, -20, _graphics.PreferredBackBufferWidth + 40, _graphics.PreferredBackBufferHeight + 20), fill);
            Lightmanager.Draw(gameTime);
            SpriteBatch.End();

            GraphicsDevice.SetRenderTarget(_BackBuffer.LightMap); //Activate the LightMap-BackBuffer
            GraphicsDevice.Clear(Color.White); //Set the Background of the LightMap
            SpriteBatch.Begin(SpriteSortMode.Deferred, CustomBlendState.ReverseSubtract); //The SoureMap as to be reverse substracted to the BackBuffer to invert the color values
            SpriteBatch.Draw(_BackBuffer.SourceMap, new Vector2(0, 0), Color.White); //'Merge the SourceMap to the BackBuffer
            SpriteBatch.End();
            DrawingLights = false;
            //set back normal target and draw game
            GraphicsDevice.SetRenderTarget(null);
            SpriteBatch.Begin();
            base.Draw(gameTime);
            SpriteBatch.End();

            //add lights
            SpriteBatch.Begin(SpriteSortMode.Deferred, CustomBlendState.ReverseSubtract);
            SpriteBatch.Draw(_BackBuffer.LightMap, new Vector2(0, 0), new Color(255, 255, 255, 255));
            SpriteBatch.End(); 

        }

                   

    }
}

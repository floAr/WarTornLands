using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WarTornLands.Entities;
using WarTornLands.Entities.Modules.Die;
using WarTornLands.Entities.Modules.Draw;
using WarTornLands.Entities.Modules.Draw.ParticleSystem;
using WarTornLands.Infrastructure;
using WarTornLands.Infrastructure.Systems.SkyLight;
using WarTornLands.PlayerClasses;
using WarTornLands.World;
using WarTornLands.Infrastructure.Systems.Camera2D;
using System;

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
        //test für draw
        public Inventory Inventory { get; private set; }

        public Interface Interface { get; private set; }
        public Level Level { get; private set; }

        private BackBuffer _BackBuffer;

        private Camera2D _camera;
        public Camera2D Camera { get { return _camera; } }

        //Debug
        Texture2D weaponMarker;
        

        private static Game1 _instance = new Game1();

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


            Player = Player.Instance;
            Player.DrawOrder = 100;
            Player.Position = new Vector2(14 * Constants.TileSize, 19 * Constants.TileSize);
            this.Components.Add(InputManager.Instance);


            _camera = new Camera2D(Player);
            Inventory = Player.Inventory;
            this.Components.Add(Player);
            this.Components.Add(DialogManager.Instance);
            Interface = new Interface();
            this.Components.Add(Interface);


            InputManager.Instance.KQuit.Pressed += new EventHandler(OnQuit);

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

            //    SoundManager.Instance.StartPlaying("ambient");
            //Add Default Explosion Module
            Catalog.Instance.SetupTestCatalog();

            StaticDrawer sd = new StaticDrawer();

            Game1.Instance.Components.Add(Inventory);
            sd.Texture = Content.Load<Texture2D>("blackhole");

            staticTest = new Entity(this, new Vector2(50, 50), "loch");
            staticTest.AddModule(sd);
            staticTest.AddModule(new ExplodeAndLoot(new PlayerClasses.Items.Item(PlayerClasses.Items.ItemTypes.Potion)));
            staticTest.Health = 100;
            List<Texture2D> pL = new List<Texture2D>();
            pL.Add(Content.Load<Texture2D>("flame3"));
            particleTest = new Entity(this, new Vector2(150, 300));
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
                    Alpha = new Range(1),
                    AlphaDecay = new Range(0.01f, 0.1f)

                },
            pL);
            particleTest.AddModule(pSystem);

            _BackBuffer = new BackBuffer(GraphicsDevice, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight));

            List<Color> daylight = new List<Color>();
            daylight.Add(new Color(40, 30, 80));
            daylight.Add(new Color(40, 30, 70));
            daylight.Add(new Color(40, 20, 70));
            daylight.Add(new Color(40, 30, 75));
            daylight.Add(new Color(25, 25, 80));
            daylight.Add(new Color(40, 30, 80));

            Lightmanager.SetDayCycle(daylight, 18000);
            #region opening
            List<Vector2> points = new List<Vector2>();
            points.Add(Player.Position);
            points.Add(new Vector2(574, 546));
            points.Add(new Vector2(670, 916));
            points.Add(new Vector2(994, 1053));
            points.Add(new Vector2(1244, 908));
            points.Add(new Vector2(1250, 415));
            points.Add(Player.Position);
            _camera.PlayCinematic(points, 6000);
            #endregion
            //    Lightmanager.SetStaticColor(Color.White);

          //    Lightmanager.SetStaticColor(Color.White);
            // TODO: Verwenden Sie this.Content, um Ihren Spiel-Inhalt hier zu laden


            weaponMarker = Content.Load<Texture2D>("weapontest");
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
            _camera.Update(gameTime);
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                _camera.BreakCinematic();

            }
              
            if (Player.ToBeRemoved)
            {
                FreezeGame();
            }

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

        private void FreezeGame()
        {
            foreach (GameComponent comp in Components)
            {
                if (comp is WarTornLands.World.Layers.EntityLayer)
                    comp.Enabled = false;
            }
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
            if (foreward)
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
            Color fill = new Color(drawcounter / 100, drawcounter / 100, drawcounter / 100);

            DrawingLights = true;
            // Set the render target
            GraphicsDevice.SetRenderTarget(_BackBuffer.SourceMap);

            GraphicsDevice.Clear(new Color(150, 150, 150)); //Set the Background of the SourceMap to Black (Important!)

            SpriteBatch.Begin(SpriteSortMode.Deferred, CustomBlendState.Add);
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
            //   SpriteBatch.DrawString(Content.Load<SpriteFont>("Test"), Player.Position.ToString(), Vector2.Zero, Color.White);
            SpriteBatch.End();


            //add lights
            SpriteBatch.Begin(SpriteSortMode.Deferred, CustomBlendState.ReverseSubtract);
            SpriteBatch.Draw(_BackBuffer.LightMap, new Vector2(0, 0), new Color(255, 255, 255, 255));
            SpriteBatch.End();

            if (Player.ToBeRemoved)
            {
                SpriteFont font = Content.Load<SpriteFont>("Test");
                string go = "Game Over :(";
                SpriteBatch.Begin();
                SpriteBatch.DrawString(font, go, new Vector2(_graphics.PreferredBackBufferWidth / 2 - (font.MeasureString(go).X / 2), _graphics.PreferredBackBufferHeight / 2 - (font.MeasureString(go).Y / 2)), Color.OrangeRed);
                SpriteBatch.End();

            }

            if (Level.Ute.ToBeRemoved)
            {
                SpriteFont font = Content.Load<SpriteFont>("Test");
                string line1 = "CONGRATURATION";
                string line2 = "A WINRAR";
                string line3 = "IS YOU";
                SpriteBatch.Begin();
                SpriteBatch.DrawString(font, line1, new Vector2(_graphics.PreferredBackBufferWidth / 2 - (font.MeasureString(line1).X / 2), _graphics.PreferredBackBufferHeight / 2 - (font.MeasureString(line1).Y * .5f) - font.MeasureString(line1).Y), Color.OrangeRed);
                SpriteBatch.DrawString(font, line2, new Vector2(_graphics.PreferredBackBufferWidth / 2 - (font.MeasureString(line2).X / 2), _graphics.PreferredBackBufferHeight / 2 - (font.MeasureString(line2).Y / 2)), Color.OrangeRed);
                SpriteBatch.DrawString(font, line3, new Vector2(_graphics.PreferredBackBufferWidth / 2 - (font.MeasureString(line3).X / 2), _graphics.PreferredBackBufferHeight / 2 - (font.MeasureString(line3).Y / 2) + font.MeasureString(line1).Y), Color.OrangeRed);           
                SpriteBatch.End();
                Player.ProvisionalFreezePlayerForDialog();
            }

            SpriteBatch.Begin();
            float scale = .2f;
            SpriteBatch.Draw(weaponMarker, WarTornLands.Entities.Modules.Think.Parts.SwingHitAbility.WeaponMarkerA - new Vector2(weaponMarker.Height * scale * .5f), null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            SpriteBatch.Draw(weaponMarker, WarTornLands.Entities.Modules.Think.Parts.SwingHitAbility.WeaponMarkerB - new Vector2(weaponMarker.Height * scale * .5f), null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            SpriteBatch.End();
        }

        #region Subscribed events

        private void OnQuit(object sender, EventArgs e)
        {
            this.Exit();
        }

        #endregion
    }
}

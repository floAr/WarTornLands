using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WarTornLands.Infrastructure;
using WarTornLands.Infrastructure.Systems.Camera2D;
using WarTornLands.PlayerClasses;
using WarTornLands.World;
using WarTornLands.Infrastructure.Systems.GameState;
using WarTornLands.Infrastructure.Systems.GameState.States;
#if DEBUG
using WarTornLands.DEBUG;
using WarTornLands.Infrastructure.Systems.SaveLoad;
#endif

namespace WarTornLands
{
    /// <summary>
    /// Dies ist der Haupttyp für Ihr Spiel
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
#if DEBUG
        public static ScreenLogComponent GlobalLog;
#endif
        GraphicsDeviceManager _graphics;

        public SpriteBatch SpriteBatch { get; private set; }

       
         private Stack<BaseGameState> _states;
        public Player Player { get; private set; }
        //test für draw
        public Inventory Inventory { get; private set; }

        public Interface Interface { get; private set; }
        public Level Level { get; set; }

        private Camera2D _camera;
        public Camera2D Camera
        { get { return _camera; } }

        public BaseGameState CurrentState
        {
            get { return _states.Peek(); }
        }

        public bool DrawingLights { get; set; }
       


        private static Game1 _instance = new Game1();

        public static Game1 Instance
        {
            get
            {
                return _instance;
            }
        }

        private Vector2 _clientBounds;
        public Vector2 ClientBounds { get { return _clientBounds; } }
        public Vector2 ClientBoundsHalf { get { return _clientBounds/2; } }

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
            IsFixedTimeStep = false;
            _graphics.SynchronizeWithVerticalRetrace = false;
            // TODO: Fügen Sie Ihre Initialisierungslogik hier hinzu
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            _clientBounds = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

            _states = new Stack<BaseGameState>();


            this.Components.Add(InputManager.Instance);


            SmartStorage<SaveGameData>.Init();
            
            Player = Player.Instance;
            Player.Position = new Vector2(14 * Constants.TileSize, 19 * Constants.TileSize); // Spawn: Frederik
            //Player.Position = new Vector2(39 * Constants.TileSize, 18 * Constants.TileSize); // Spawn: GruselUte

            _camera=new Camera2D(Player);

            //Inventory = Player.Inventory;

           
            this.Components.Add(DialogManager.Instance);
            Interface = new Interface();
            this.Components.Add(Interface);

#if DEBUG
            GlobalLog = new ScreenLogComponent(this);
            this.Components.Add(GlobalLog);
            this.Components.Add(new CuteFrametimeCounterComponent(this,true,false));

#endif

            //InputManager.Instance.KQuit.Pressed += new EventHandler(OnQuit);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent wird einmal pro Spiel aufgerufen und ist der Platz, wo
        /// Ihr gesamter Content geladen wird.
        /// </summary>
        protected override void LoadContent()
        {
            this.PushState(MainMenueState.Instance);
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

            _camera.Update(gameTime);
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                _camera.BreakCinematic();

            }              
            if (Player.ToBeRemoved)
            {
                FreezeGame();
            }
            _states.Peek().Update(gameTime);
     
            base.Update(gameTime);
        }

        private void FreezeGame()
        {
            // TODO freeze game
        }
        /// <summary>
        /// Dies wird aufgerufen, wenn das Spiel selbst zeichnen soll.
        /// </summary>
        /// <param name="gameTime">Bietet einen Schnappschuss der Timing-Werte.</param>
        /// 
        protected override void Draw(GameTime gameTime)
        {
            
            _states.Peek().Draw(gameTime);
            SpriteBatch.Begin();
            base.Draw(gameTime);
            RectangleDrawer.Draw();
            SpriteBatch.End();
        }

        #region Subscribed events

        private void OnQuit(object sender, EventArgs e)
        {
            this.Exit();
        }

        #endregion


        internal void PushState(BaseGameState newState)
        {

            if (_states.Count>0&&newState == _states.Peek())
                return;
            if(_states.Count>0)
                _states.Peek().Pause();
            _states.Push(newState);
            if (!newState.IsInitializedAndLoaded)
            {
                newState.Initialize();
                newState.LoadContent();
            }
            else
                newState.Resume();

        }

        internal void PopState()
        {
            if (_states.Count > 0)
            {
                _states.Peek().Pause();
                _states.Pop();
            }
            if (_states.Count > 0)
                _states.Peek().Resume();
            else
                this.Exit();
        }

        internal void DebugDraw(GameTime gameTime)
        {

            base.Draw(gameTime);
        }
    }
}

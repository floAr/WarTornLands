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
using WarTornLandsRefurbished.Entities;
using WarTornLandsRefurbished.Infrastructure.Interfaces;
using WarTornLandsRefurbished.Infrastructure.Systeme.AnimationsSystem;

namespace WarTornLandsRefurbished
{
    /// <summary>
    /// Dies ist der Haupttyp für Ihr Spiel
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Entity staticTest;
        Entity animatedTest;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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
            spriteBatch = new SpriteBatch(GraphicsDevice);
            StaticDrawer sd=new StaticDrawer();
            sd.Texture=Content.Load<Texture2D>("blackhole");
            staticTest = new Entity(sd);
            AnimationSystem animS=new AnimationSystem(Content.Load<Texture2D>("character_64x128"));

            Animation anim = new Animation( "walkDown");
            for (int i = 0; i < 4; i++)
                anim.AddFrame(new Rectangle(64 * i, 0, 64, 128));
            animS.AddAnimation(anim);
            animS.SetCurrentAnimation("walkDown");
            animatedTest = new Entity(animS);
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
            animatedTest.Update(gameTime);
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
            spriteBatch.Begin();
            staticTest.Draw(spriteBatch, gameTime);
            animatedTest.Draw(spriteBatch, gameTime);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Liefert true, falls an position der Spieler befindet
        /// </summary>
        public bool IsPlayerAt(Vector2 position)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Liefert das Entity an der Position zur Interaktion, null wenn keine Entity vorhanden ist
        /// </summary>
        public Entities.Entity TryInteractionAt(Vector2 position)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Liefert das Entity an der Position zum Angriff, null wenn keine Entity vorhanden ist
        /// </summary>
        public Entities.Entity TryHitAt(Vector2 position)
        {
            throw new System.NotImplementedException();
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WarTornLands.Infrastructure.Systems.SkyLight;
using WarTornLands.Entities;
using WarTornLands.World;
using WarTornLands.PlayerClasses;
using WarTornLands.Entities.Modules.Draw;
using Microsoft.Xna.Framework.Input;
using WarTornLands.Entities.Modules.Think;

namespace WarTornLands.Infrastructure.Systems.GameState.States
{
    public class RunningGameState : BaseGameState
    {

        private BackBuffer _BackBuffer;

        private RenderTarget2D _lastFrame;
        //Debug
        Texture2D weaponMarker;

        // TODO remove splash screen
        public Entity splash { get; set; }




        public override void Initialize()
        {


            Game1.Instance.Level = new Level(Game1.Instance);
            //Level.LoadTestLevel();
            Game1.Instance.Level.LoadChristmasCaverns();
            this._inputSheet.RegisterKey("Exit", Keys.Escape);
            this._inputSheet.RegisterKey("New", Keys.Enter);
            this._inputSheet.RegisterKey("Hit", Keys.Enter);
            this._inputSheet.RegisterKey("Jump", Keys.Space);
            this._inputSheet.RegisterKey("Interact", Keys.T);
            this._inputSheet.RegisterKey("UsePotion", Keys.P);
            this._inputSheet.RegisterKey("Inventory", Keys.I);
            this._inputSheet.RegisterKey("Quit", Keys.Escape);
            this._inputSheet.RegisterKey("Move", new Keys[] { Keys.W, Keys.A, Keys.S, Keys.D });




        }

        public override void LoadContent()
        {
            Catalog.Instance.SetupTestCatalog();

            Game1.Instance.Player.AddModule(new ThinkInputGuided());

            _BackBuffer = new BackBuffer(Game1.Instance.GraphicsDevice, new Rectangle(0, 0, Game1.Instance.GraphicsDevice.Viewport.Width, Game1.Instance.GraphicsDevice.Viewport.Height));

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
            points.Add(new Vector2(-400, Game1.Instance.Player.Position.Y));
            points.Add(new Vector2(-400, Game1.Instance.Player.Position.Y));
            points.Add(Game1.Instance.Player.Position);
            Game1.Instance.Camera.PlayCinematic(points, 6000);
            #endregion

            weaponMarker = Game1.Instance.Content.Load<Texture2D>("sprite/weapontest");
            splash = new Entity(Game1.Instance, new Vector2(-400, 608));
            StaticDrawer splashSd = new StaticDrawer();
            splashSd.Texture = Game1.Instance.Content.Load<Texture2D>("sprite/xmasSplash");
            splash.AddModule(splashSd);

            base.LoadContent();
        }

        public override void Pause()
        {

        }

        public override void Resume()
        {

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.M))
                Game1.Instance.PopState();
            if (Keyboard.GetState().IsKeyDown(Keys.I))
                Game1.Instance.PushState(new InventoryState(_BackBuffer.LastFrame));
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Game1.Instance.GraphicsDevice.Clear(Color.CornflowerBlue);

            Game1.Instance.DrawingLights = true;
            // Set the render target
            Game1.Instance.GraphicsDevice.SetRenderTarget(_BackBuffer.SourceMap);

            Game1.Instance.GraphicsDevice.Clear(new Color(150, 150, 150)); //Set the Background of the SourceMap to Black (Important!)

            Game1.Instance.SpriteBatch.Begin(SpriteSortMode.Deferred, CustomBlendState.Add);
            Lightmanager.Draw(gameTime);
            Game1.Instance.SpriteBatch.End();

            Game1.Instance.GraphicsDevice.SetRenderTarget(_BackBuffer.LowerLightMap); //Activate the LightMap-BackBuffer
            Game1.Instance.GraphicsDevice.Clear(Color.White); //Set the Background of the LightMap
            Game1.Instance.SpriteBatch.Begin(SpriteSortMode.Deferred, CustomBlendState.ReverseSubtract); //The SoureMap as to be reverse substracted to the BackBuffer to invert the color values
            Game1.Instance.SpriteBatch.Draw(_BackBuffer.SourceMap, new Vector2(0, 0), Color.White); //'Merge the SourceMap to the BackBuffer
            Game1.Instance.SpriteBatch.End();

            Game1.Instance.DrawingLights = false;
            //set back normal target and draw game
            Game1.Instance.GraphicsDevice.SetRenderTarget(_BackBuffer.LastFrame);
            Game1.Instance.SpriteBatch.Begin();
            Game1.Instance.DebugDraw(gameTime);
            //   Game1.Instance.SpriteBatch.DrawString(Content.Load<SpriteFont>("Test"), Player.Position.ToString(), Vector2.Zero, Color.White);
            Game1.Instance.SpriteBatch.End();


            //add lights
            Game1.Instance.SpriteBatch.Begin(SpriteSortMode.Deferred, CustomBlendState.ReverseSubtract);
            Game1.Instance.SpriteBatch.Draw(_BackBuffer.LowerLightMap, new Vector2(0, 0), new Color(255, 255, 255, 255));
            Game1.Instance.SpriteBatch.End();


            Game1.Instance.GraphicsDevice.SetRenderTarget(null);
            Game1.Instance.SpriteBatch.Begin();
            Game1.Instance.SpriteBatch.Draw(_BackBuffer.LastFrame, Vector2.Zero, Color.White);
            Game1.Instance.SpriteBatch.End();


            if (Game1.Instance.Player.ToBeRemoved)
            {
                SpriteFont font = Game1.Instance.Content.Load<SpriteFont>("Test");
                string go = "Game Over :(";
                Game1.Instance.SpriteBatch.Begin();
                Game1.Instance.SpriteBatch.DrawString(font, go, new Vector2(Game1.Instance.GraphicsDevice.Viewport.Width / 2 - (font.MeasureString(go).X / 2), Game1.Instance.GraphicsDevice.Viewport.Height / 2 - (font.MeasureString(go).Y / 2)), Color.OrangeRed);
                Game1.Instance.SpriteBatch.End();

            }

            if (Level.Ute.ToBeRemoved)
            {
                SpriteFont font = Game1.Instance.Content.Load<SpriteFont>("font/Test");
                string line1 = "CONGRATURATION";
                string line2 = "A WINRAR";
                string line3 = "IS YOU";
                Game1.Instance.SpriteBatch.Begin();
                Game1.Instance.SpriteBatch.DrawString(font, line1, new Vector2(Game1.Instance.GraphicsDevice.Viewport.Width / 2 - (font.MeasureString(line1).X / 2), Game1.Instance.GraphicsDevice.Viewport.Height / 2 - (font.MeasureString(line1).Y * .5f) - font.MeasureString(line1).Y), Color.OrangeRed);
                Game1.Instance.SpriteBatch.DrawString(font, line2, new Vector2(Game1.Instance.GraphicsDevice.Viewport.Width / 2 - (font.MeasureString(line2).X / 2), Game1.Instance.GraphicsDevice.Viewport.Height / 2 - (font.MeasureString(line2).Y / 2)), Color.OrangeRed);
                Game1.Instance.SpriteBatch.DrawString(font, line3, new Vector2(Game1.Instance.GraphicsDevice.Viewport.Width / 2 - (font.MeasureString(line3).X / 2), Game1.Instance.GraphicsDevice.Viewport.Height / 2 - (font.MeasureString(line3).Y / 2) + font.MeasureString(line1).Y), Color.OrangeRed);
                Game1.Instance.SpriteBatch.End();

            }

            Game1.Instance.SpriteBatch.Begin();
            float scale = .2f;
            Game1.Instance.SpriteBatch.Draw(weaponMarker, WarTornLands.Entities.Modules.Think.Parts.SwingHitAbility.WeaponMarkerA - new Vector2(weaponMarker.Height * scale * .5f), null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            Game1.Instance.SpriteBatch.Draw(weaponMarker, WarTornLands.Entities.Modules.Think.Parts.SwingHitAbility.WeaponMarkerB - new Vector2(weaponMarker.Height * scale * .5f), null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            Game1.Instance.SpriteBatch.End();

            Game1.Instance.SpriteBatch.Begin();
            splash.Draw(new GameTime());
            Game1.Instance.SpriteBatch.End();
        }


    }
}

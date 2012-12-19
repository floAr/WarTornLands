#if DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace WarTornLands.DEBUG
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class CuteFrametimeCounterComponent : Microsoft.Xna.Framework.DrawableGameComponent
    {
        enum PerfIconType
        {
            Stopwatch,
            GarbageBin,
            HappyFace,
            NeutralFace,
            SadFace
        }

        Stopwatch FrametimeStopwatch;

        const int PERF_ICON_WIDTH = 32;
        Texture2D PerfIconSheet;
        SpriteFont FrametimeDisplayFont;
        SpriteBatch FrametimeDisplayBatch;

        bool ShowFrametimeText;
        bool ShowIcons;

        public Vector2 FrametimeDisplayPosition;
        public Color FrametimeDisplayColor;

        const double FRAMETIME_DANGER_THRESHOLD = 1000.0f / 60.0f;      //If averaged frametime goes over this threshold...
        const float DANGER_COLOR_DISPLAY_SECONDS = 2.0f;                //... the frametime display will change color for this many seconds.

        float DangerTimer;
        public Color FrametimeDangerDisplayColor;
        float AveragedFrameTime;

        public bool ShowAveragedFrametime;

        WeakReference GcDetectionReference;
        long TotalMemoryKB;

        float RecentGarbageCollections;

        public CuteFrametimeCounterComponent(Game game, bool showFrametimeText,bool showIcons)
            : base(game)
        {

            ShowAveragedFrametime = true;
            FrametimeDisplayPosition = Vector2.One * 10.0f;
            FrametimeDisplayColor = Color.Green;
            FrametimeDangerDisplayColor = Color.Magenta;
            GcDetectionReference = new WeakReference(null);
            RecentGarbageCollections = 0;
            ShowFrametimeText = showFrametimeText;
            ShowIcons = showIcons;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            FrametimeStopwatch = new Stopwatch();
            FrametimeStopwatch.Start();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            FrametimeDisplayBatch = new SpriteBatch(Game.GraphicsDevice);
            PerfIconSheet = Game.Content.Load<Texture2D>("sprite/PerfIconSheet");
            FrametimeDisplayFont = Game.Content.Load<SpriteFont>("font/debug");
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TimeSpan frameTime = FrametimeStopwatch.Elapsed;
            FrametimeStopwatch.Reset();
            FrametimeStopwatch.Start();

            RecentGarbageCollections = MathHelper.Max(0.0f, RecentGarbageCollections - deltaSeconds * 0.1f);        //Subtract 1 RecentGC every 10 seconds
            if (!GcDetectionReference.IsAlive)
            {
                GcDetectionReference = new WeakReference(new Object());
                TotalMemoryKB = GC.GetTotalMemory(false) / 1024;
                RecentGarbageCollections += 1.0f;
            }


            AveragedFrameTime = AveragedFrameTime * 0.9f + (float)frameTime.TotalMilliseconds * 0.1f;
            DangerTimer = MathHelper.Clamp(DangerTimer - deltaSeconds, 0.0f, DANGER_COLOR_DISPLAY_SECONDS);
            if (AveragedFrameTime > FRAMETIME_DANGER_THRESHOLD)
            {
                DangerTimer = DANGER_COLOR_DISPLAY_SECONDS;
            }

            int displayItemCount = 0;
            FrametimeDisplayBatch.Begin();

            if (ShowFrametimeText)
            {
                FrametimeDisplayBatch.DrawString(FrametimeDisplayFont, "Frame Time: " + (ShowAveragedFrametime ? AveragedFrameTime : frameTime.TotalMilliseconds) + "ms", FrametimeDisplayPosition + Vector2.UnitY * (FrametimeDisplayFont.LineSpacing * displayItemCount++), DangerTimer > 0.0f ? FrametimeDangerDisplayColor : FrametimeDisplayColor);
                FrametimeDisplayBatch.DrawString(FrametimeDisplayFont, "Total Memory: " + TotalMemoryKB + "KB", FrametimeDisplayPosition + Vector2.UnitY * (FrametimeDisplayFont.LineSpacing * displayItemCount++), FrametimeDisplayColor);
                FrametimeDisplayBatch.DrawString(FrametimeDisplayFont, "Recent GCs: " + (int)RecentGarbageCollections, FrametimeDisplayPosition + Vector2.UnitY * (FrametimeDisplayFont.LineSpacing * displayItemCount++), FrametimeDisplayColor);
            }
            if (ShowIcons)
            {
                DrawIcon(PerfIconType.Stopwatch, FrametimeDisplayPosition + Vector2.UnitY * (FrametimeDisplayFont.LineSpacing * displayItemCount));
                DrawIcon(FrametimeToPerfIcon(ShowAveragedFrametime ? AveragedFrameTime : (float)frameTime.TotalMilliseconds), FrametimeDisplayPosition + Vector2.UnitX * PERF_ICON_WIDTH + Vector2.UnitY * (FrametimeDisplayFont.LineSpacing * displayItemCount));
                displayItemCount++;

                int garbageX = 0;
                int garbageY = 0;
                for (int i = 0; i < (int)RecentGarbageCollections; ++i)
                {
                    DrawIcon(PerfIconType.GarbageBin, FrametimeDisplayPosition + Vector2.UnitX * PERF_ICON_WIDTH * garbageX + Vector2.UnitY * (FrametimeDisplayFont.LineSpacing * displayItemCount) + Vector2.UnitY * (FrametimeDisplayFont.LineSpacing * garbageY));
                    garbageX++;
                    if (garbageX > 10)
                    {
                        garbageY++;
                        garbageX = 0;
                    }
                }
                displayItemCount++;
            }
            FrametimeDisplayBatch.End();

            base.Draw(gameTime);
        }

        private void DrawIcon(PerfIconType iconType, Vector2 position)
        {
            FrametimeDisplayBatch.Draw(PerfIconSheet, position, new Rectangle((int)iconType * PERF_ICON_WIDTH, 0, PERF_ICON_WIDTH, PERF_ICON_WIDTH), Color.White);
        }

        private PerfIconType FrametimeToPerfIcon(float frameTime)
        {
            if (frameTime < FRAMETIME_DANGER_THRESHOLD * 0.75f)
            {
                return PerfIconType.HappyFace;
            }
            else if (frameTime < FRAMETIME_DANGER_THRESHOLD * 1.5f)
            {
                return PerfIconType.NeutralFace;
            }

            return PerfIconType.SadFace;
        }
    }
}
#endif
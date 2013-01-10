using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using WarTornLands.Infrastructure.Systems.SkyLight;
#if DEBUG
using WarTornLands.DEBUG;
#endif

namespace WarTornLands.Infrastructure.Systems.DrawSystem
{
    public class DrawManager
    {


        private GameTime _gameTime;
        private int _state = 0;

        private RenderTarget2D _temp;
        private Pool<RenderTarget2D> _targetPool;
        private Pool<int> testPo;
        private Effect _defaultEffect;

        private RenderTarget2D _lastFrame;
        public RenderTarget2D LastFrame { get { return _lastFrame; } }

        SpriteSortMode _choosenSortMode;
        /// <summary>
        /// Manager who handles all the drawing and effect applying.
        /// </summary>
        public DrawManager()
        {
            _defaultEffect = Game1.Instance.Content.Load<Effect>("effect/reset");
            _targetPool = new Pool<RenderTarget2D>(createRT);
            _targetPool.Preallocate(2);
            //_lastFrame = _targetPool.GetFreeItem();
        }
        /// <summary>
        /// Starts the bake process and prepares a fresh <c>RenderTarget2D</c>.
        /// </summary>
        /// <param name="gameTime">Time since last update</param>
        /// <param name="customSortMode">If needed specify <c>SpriteSortMode</c> for this bake process</param>
        /// <param name="customBlendState">If needed specify <c>BlendState</c> for this bake process</param>
        public void BeginBake(GameTime gameTime, SpriteSortMode customSortMode = SpriteSortMode.Deferred, BlendState customBlendState = null)
        {
            _gameTime = gameTime;
            _temp = _targetPool.GetFreeItem();
            Game1.Instance.GraphicsDevice.SetRenderTarget(_temp);
            _state = 1;
            _choosenSortMode = customSortMode;
            if (customBlendState == null)
                Game1.Instance.SpriteBatch.Begin(customSortMode, BlendState.AlphaBlend);
            else
                Game1.Instance.SpriteBatch.Begin(customSortMode, customBlendState);
        }
        /// <summary>
        /// Starts the bake process and uses a predefined <c>RenderTarget2D</c>.
        /// </summary>
        /// <param name="gameTime">Time since last update</param>
        /// <param name="plate">The <c>RenderTarget2D</c> which is used to bake on</param>
        /// <param name="customSortMode">If needed specify <c>SpriteSortMode</c> for this bake process</param>
        /// <param name="customBlendState">If needed specify <c>BlendState</c> for this bake process</param>
        public void BeginBake(GameTime gameTime, RenderTarget2D plate, SpriteSortMode customSortMode = SpriteSortMode.Deferred, BlendState customBlendState = null)
        {
            _gameTime = gameTime;
            _temp = plate;
            Game1.Instance.GraphicsDevice.SetRenderTarget(_temp);
            _state = 1;
            if (customBlendState == null)
                Game1.Instance.SpriteBatch.Begin(customSortMode, BlendState.AlphaBlend);
            else
                Game1.Instance.SpriteBatch.Begin(customSortMode, customBlendState);
        }

        /// <summary>
        /// Fills the whole current <c>RenderTarget2D</c> with one color.
        /// </summary>
        /// <param name="c">Color to apply on the whole <c>RenderTarget2D</c></param>
        public void BakeFill(Color c)
        {
            if (_state != 1)
                if (_state == 0)
                    throw new Exception("BeginBake must be called before BakeFill");
            Game1.Instance.GraphicsDevice.Clear(c);
        }

        /// <summary>
        /// Starts the excecution of a <c>Effect</c>.\n
        /// Note that SpriteSortMode MUST be 'Immediate'.
        /// </summary>
        /// <param name="effect">Effect to be startet</param>
        /// <exception cref="System.Exception">BeginBake must be called before BakeBeginEffect</exception>
        /// <exception cref="System.Exception">When using Effect stick to SpriteSortMode.Immediate.\nEffects won´t affect Sprite when deferred drawing is used</exception>
        public void BakeBeginEffect(Effect effect)
        {
            if (_state != 1)
                if (_state == 0)
                    throw new Exception("BeginBake must be called before BakeBeginEffect");
            if (_choosenSortMode != SpriteSortMode.Immediate)
                throw new Exception("When using Effect stick to SpriteSortMode.Immediate.\nEffects won´t affect Sprite when deferred drawing is used");
            effect.CurrentTechnique.Passes[0].Apply();
        }

        /// <summary>
        /// Removes an applied effect from the bake process.
        /// </summary>
        /// <exception cref="System.Exception">BeginBake must be called before BakeBeginEffect</exception>
        public void BakeEndEffect()
        {
            if (_state != 1)
                if (_state == 0)
                    throw new Exception("BeginBake must be called before BakeBeginEffect");
            _defaultEffect.CurrentTechnique.Passes[0].Apply();
        }

        /// <summary>
        /// Bakes the specified <c>RenderTarget2D</c>.
        /// </summary>
        /// <param name="drawProvider">The RenderTarget to draw.</param>
        /// <exception cref="System.Exception">BeginBake must be called before Bake</exception>
        public void Bake(RenderTarget2D drawProvider)
        {
            if (_state != 1)
                if (_state == 0)
                    throw new Exception("BeginBake must be called before Bake");
            Game1.Instance.SpriteBatch.Draw(drawProvider, Vector2.Zero, Color.White);
        }
        public void Bake(Texture2D drawProvider)
        {
            if (_state != 1)
                if (_state == 0)
                    throw new Exception("BeginBake must be called before Bake");
            Game1.Instance.SpriteBatch.Draw(drawProvider, Vector2.Zero, Color.White);
        }

        /// <summary>
        /// Bakes the specified <c>IDrawProvider</c> by calling its Draw(GameTime).
        /// </summary>
        /// <param name="drawProvider">The draw provider implementing Draw(GameTime).</param>
        /// <exception cref="System.Exception">BeginBake must be called before Bake</exception>
        public void Bake(IDrawProvider drawProvider)
        {
            if (_state != 1)
                if (_state == 0)
                    throw new Exception("BeginBake must be called before Bake");
            drawProvider.Draw(_gameTime);
        }

        /// <summary>
        /// Bakes all of the specified <c>IDrawProvider</c>s by calling their Draw(GameTime).
        /// </summary>
        /// <param name="drawProviders">Array containing all the <c>IDrawProvider</c>s</param>
        /// <exception cref="System.Exception">BeginBake must be called before Bake</exception>
        public void Bake(IDrawProvider[] drawProviders)
        {
            if (_state != 1)
                if (_state == 0)
                    throw new Exception("BeginBake must be called before Bake");
            for (int i = 0; i < drawProviders.Length; ++i)
                drawProviders[i].Draw(_gameTime);
        }

        /// <summary>
        /// Ends the baking process and return the well done <c>RenderTarget2D</c>.
        /// </summary>
        /// <returns>The finished <c>RenderTarget2D</c></returns>
        /// <exception cref="System.Exception">BeginBake must be called before EndBake</exception>
        public RenderTarget2D EndBake()
        {
            if (_state != 1)
                if (_state == 0)
                    throw new Exception("BeginBake must be called before EndBake");
            Game1.Instance.SpriteBatch.End();
            return _temp;
        }


        /// <summary>
        /// Creates a new <c>RenderTarget2D</c>.
        /// </summary>
        /// <returns>New <c>RenderTarget2D</c>.</returns>
        private RenderTarget2D createRT()
        {

            return new RenderTarget2D(Game1.Instance.GraphicsDevice, Game1.Instance.GraphicsDevice.Viewport.Width, Game1.Instance.GraphicsDevice.Viewport.Height, false, Game1.Instance.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);
        }

        /// <summary>
        /// Draws the specified <c>RenderTarget2D</c>.
        /// </summary>
        /// <param name="source">The <c>RenderTarget2D</c> to put on the screen.</param>
        /// <param name="gameTime">The game time since last Update.</param>
        public void Draw(RenderTarget2D source, GameTime gameTime)
        {
            
            this._gameTime = gameTime;
            if (_gameTime == null)
                return;
            _lastFrame = source;
            Game1.Instance.GraphicsDevice.SetRenderTarget(null);
            Game1.Instance.SpriteBatch.Begin();
            Game1.Instance.SpriteBatch.Draw(source, Vector2.Zero, Color.White);
            Game1.Instance.SpriteBatch.End();
            foreach (RenderTarget2D item in _targetPool.Items)
            {
                _targetPool.FlagFreeItem(item);
            }
        }

        public void Update(GameTime gameTime)
        {
            _gameTime = gameTime;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using WarTornLands.Infrastructure.Systems.SkyLight;

namespace WarTornLands.Infrastructure.Systems.DrawSystem
{
    public class DrawManager
    {
        //    private Stack<RenderTarget2D> _chain;
        private GameTime _gameTime;
        private int _state = 0;

        private RenderTarget2D _temp;

        private Effect _defaultEffect;

       private RenderTarget2D _lastFrame;
       public RenderTarget2D LastFrame { get { return _lastFrame; } }

       SpriteSortMode _choosenSortMode;

        public DrawManager()
        {
           _defaultEffect= Game1.Instance.Content.Load<Effect>("effect/reset");
           _lastFrame = createRT();
        }

        public void BeginBake(GameTime gameTime, SpriteSortMode customSortMode = SpriteSortMode.Deferred, BlendState customBlendState = null)
        {
            _gameTime = gameTime;
            _temp = createRT();
            Game1.Instance.GraphicsDevice.SetRenderTarget(_temp);
            _state = 1;
            _choosenSortMode = customSortMode;
            if (customBlendState == null)
                Game1.Instance.SpriteBatch.Begin(customSortMode, BlendState.AlphaBlend);
            else
                Game1.Instance.SpriteBatch.Begin(customSortMode, customBlendState);
        }
        public void BeginBake(GameTime gameTime,RenderTarget2D plate, SpriteSortMode customSortMode = SpriteSortMode.Deferred, BlendState customBlendState = null)
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

        public void BakeFill(Color c)
        {
            if (_state != 1)
                if (_state == 0)
                    throw new Exception("BeginBake must be called before BakeFill");
            Game1.Instance.GraphicsDevice.Clear(c);
        }

        public void BakeBeginEffect(Effect effect)
        {
            if (_state != 1)
                if (_state == 0)
                    throw new Exception("BeginBake must be called before BakeBeginEffect");
            if (_choosenSortMode != SpriteSortMode.Immediate)
                throw new Exception("When using Effect stick to SpriteSortMode.Immediate.\nEffects won´t affect Sprite when deferred drawing is used");
            effect.CurrentTechnique.Passes[0].Apply();
        }

        public void BakeEndEffect()
        {
            if (_state != 1)
                if (_state == 0)
                    throw new Exception("BeginBake must be called before BakeBeginEffect");
            _defaultEffect.CurrentTechnique.Passes[0].Apply();
        }

        public void Bake(RenderTarget2D drawProvider)
        {
            if (_state != 1)
                if (_state == 0)
                    throw new Exception("BeginBake must be called before Bake");
            Game1.Instance.SpriteBatch.Draw(drawProvider, Vector2.Zero, Color.White);
        }

        public void Bake(IDrawProvider drawProvider)
        {
            if (_state != 1)
                if (_state == 0)
                    throw new Exception("BeginBake must be called before Bake");
            drawProvider.Draw(_gameTime);
        }

        public void Bake(IDrawProvider[] drawProviders)
        {
            if (_state != 1)
                if (_state == 0)
                    throw new Exception("BeginBake must be called before Bake");
            for(int i=0;i<drawProviders.Length;++i)
            drawProviders[i].Draw(_gameTime);
        }

        public RenderTarget2D EndBake()
        {
            if (_state != 1)
                if (_state == 0)
                    throw new Exception("BeginBake must be called before EndBake");
            Game1.Instance.SpriteBatch.End();
            return _temp;
        }


        #region craaaaap
        /*
        public RenderTarget2D B(RenderTarget2D plate, RenderTarget2D bake, bool bakeFlag, BlendState customBlendState = null)
        {
            Game1.Instance.GraphicsDevice.SetRenderTarget(plate);
            if (customBlendState == null)
                Game1.Instance.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            else
                Game1.Instance.SpriteBatch.Begin(SpriteSortMode.Deferred, customBlendState);
            Game1.Instance.SpriteBatch.Draw(bake, Vector2.Zero, Color.White);
            Game1.Instance.SpriteBatch.End();
            Game1.Instance.GraphicsDevice.SetRenderTarget(null);
            return plate;
        }
        public RenderTarget2D B(RenderTarget2D source, Effect effect, BlendState customBlendState = null)
        {
            RenderTarget2D target = createRT();
            Game1.Instance.GraphicsDevice.SetRenderTarget(target);
            Game1.Instance.GraphicsDevice.Clear(Color.Transparent);
            //Game1.Instance.SpriteBatch.Begin();
            if (customBlendState == null)
                Game1.Instance.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            else
                Game1.Instance.SpriteBatch.Begin(SpriteSortMode.Deferred, customBlendState);
            effect.CurrentTechnique.Passes[0].Apply();
            Game1.Instance.SpriteBatch.Draw(source, Vector2.Zero, Color.White);
            Game1.Instance.SpriteBatch.End();
            Game1.Instance.GraphicsDevice.SetRenderTarget(null);
            return target;
        }

        public RenderTarget2D B(RenderTarget2D upper, RenderTarget2D lower, BlendState customBlendState = null)
        {
            RenderTarget2D target = createRT();
            Game1.Instance.GraphicsDevice.SetRenderTarget(target);
            Game1.Instance.GraphicsDevice.Clear(Color.Transparent);
            if (customBlendState == null)
                Game1.Instance.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            else
                Game1.Instance.SpriteBatch.Begin(SpriteSortMode.Deferred, customBlendState);
            Game1.Instance.SpriteBatch.Draw(lower, Vector2.Zero, Color.White);
            Game1.Instance.SpriteBatch.Draw(upper, Vector2.Zero, Color.White);
            Game1.Instance.SpriteBatch.End();
            Game1.Instance.GraphicsDevice.SetRenderTarget(null);
            return target;
        }

        public RenderTarget2D B(IDrawProvider source, BlendState customBlendState = null)
        {
            RenderTarget2D target = createRT();
            Game1.Instance.GraphicsDevice.SetRenderTarget(target);
            Game1.Instance.GraphicsDevice.Clear(Color.Transparent);
            if (customBlendState == null)
                Game1.Instance.SpriteBatch.Begin();
            else
                Game1.Instance.SpriteBatch.Begin(SpriteSortMode.Deferred, customBlendState);
            source.Draw(_gameTime);
            Game1.Instance.SpriteBatch.End();
            Game1.Instance.GraphicsDevice.SetRenderTarget(null);
            return target;
        }

        public RenderTarget2D B(Color fill)
        {
            RenderTarget2D target = createRT();
            Game1.Instance.GraphicsDevice.SetRenderTarget(target);
            Game1.Instance.GraphicsDevice.Clear(fill);
            return target;
        }
        //*/
        #endregion

        private RenderTarget2D createRT()
        {

            return new RenderTarget2D(Game1.Instance.GraphicsDevice, Game1.Instance.GraphicsDevice.Viewport.Width, Game1.Instance.GraphicsDevice.Viewport.Height, false, Game1.Instance.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);
        }

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
        }

        public void Update(GameTime gameTime)
        {
            _gameTime = gameTime;
        }



    }
}

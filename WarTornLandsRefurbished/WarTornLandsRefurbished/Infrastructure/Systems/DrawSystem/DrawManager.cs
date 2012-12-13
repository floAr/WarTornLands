using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WarTornLands.Infrastructure.Systems.DrawSystem
{
   public class DrawManager
    {
  //    private Stack<RenderTarget2D> _chain;
     private  GameTime _gameTime;

       public void Register(IDrawProvider drawProvider)
       {
         //  _bindings.Add(drawProvider, target);
       }
       public RenderTarget2D B(RenderTarget2D source, Effect effect)
       {
           RenderTarget2D target = createRT();
           Game1.Instance.GraphicsDevice.SetRenderTarget(target);
           Game1.Instance.GraphicsDevice.Clear(Color.Transparent);
           //Game1.Instance.SpriteBatch.Begin();
              Game1.Instance.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            effect.CurrentTechnique.Passes[0].Apply();
           Game1.Instance.SpriteBatch.Draw(source, Vector2.Zero, Color.White);
           Game1.Instance.SpriteBatch.End();
           Game1.Instance.GraphicsDevice.SetRenderTarget(null);
           return target;
       }

       public RenderTarget2D B(RenderTarget2D upper, RenderTarget2D lower)
       {
           RenderTarget2D target = createRT();
           Game1.Instance.GraphicsDevice.SetRenderTarget(target);
           Game1.Instance.GraphicsDevice.Clear(Color.Transparent);
           Game1.Instance.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
           Game1.Instance.SpriteBatch.Draw(lower, Vector2.Zero, Color.White);
           Game1.Instance.SpriteBatch.Draw(upper, Vector2.Zero, Color.White);
           Game1.Instance.SpriteBatch.End();
           Game1.Instance.GraphicsDevice.SetRenderTarget(null);
           return target;
       }

       public RenderTarget2D B(IDrawProvider source)
       {
           RenderTarget2D target = createRT();
           Game1.Instance.GraphicsDevice.SetRenderTarget(target);
           Game1.Instance.GraphicsDevice.Clear(Color.Transparent);
           Game1.Instance.SpriteBatch.Begin();
           source.Draw(_gameTime);
           Game1.Instance.SpriteBatch.End();
           Game1.Instance.GraphicsDevice.SetRenderTarget(null);
           return target;
       }

       private RenderTarget2D createRT()
       {
          
          return new RenderTarget2D(Game1.Instance.GraphicsDevice, Game1.Instance.GraphicsDevice.Viewport.Width, Game1.Instance.GraphicsDevice.Viewport.Height, false, Game1.Instance.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);
       }

       public void Draw(RenderTarget2D source,GameTime gameTime)
       {
           this._gameTime = gameTime;
           Game1.Instance.GraphicsDevice.SetRenderTarget(null);
           Game1.Instance.SpriteBatch.Begin();
           Game1.Instance.SpriteBatch.Draw(source, Vector2.Zero, Color.White);
           Game1.Instance.SpriteBatch.End();
       }



   }
}

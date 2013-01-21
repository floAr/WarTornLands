using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WarTornLands.Infrastructure.Systems.InputSystem;
using Microsoft.Xna.Framework;
using WarTornLands.Infrastructure.Systems.DrawSystem.Effects;

namespace WarTornLands.Infrastructure.Systems.GameState.States
{
    class ShaderTestStage:BaseGameState
    {
        private ShockwaveEffect effect;
 private Texture2D tex;
 public ShaderTestStage() : base() { }


        public override void Initialize()
        {
            tex = Game1.Instance.Content.Load<Texture2D>("sprite/grid");
            this._inputSheet.RegisterKey("Exit", Keys.Escape);
            effect = new ShockwaveEffect(new Vector2(0.5f, 0.5f),1f, TimeSpan.FromSeconds(30f),3,0.1f);
           base.Initialize();
          
        }

        public override void LoadContent()
        {
            //own logic here
            (InputManager.Instance["Exit"] as Key).Pressed += new EventHandler(Close);
            //bind input
            base.LoadContent();
        }

        void Close(object sender, EventArgs e)
        {
            Game1.Instance.PopState();
        }
       

        public override void Pause()
        {
           
        }

        public override void Resume()
        {
            base.Resume();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            effect.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            _drawManager.BeginBake(gameTime,SpriteSortMode.Immediate);
            _drawManager.BakeBeginEffect(effect);
            _drawManager.Bake(tex);
            _drawManager.BakeEndEffect();
            RenderTarget2D result = _drawManager.EndBake();
            _drawManager.Draw(result,gameTime);
        }

    }
}

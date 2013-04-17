using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using WarTornLands.Infrastructure.Systems.InputSystem;
using WarTornLands.PlayerClasses;

namespace WarTornLands.Infrastructure.Systems.GameState.States
{
    public class InventoryState:BaseOverlayGameState
    {
        private Effect _backgroundEffekt;

        public InventoryState(RenderTarget2D background) : base(background) { }


        public override void Initialize()
        {
            this._inputSheet.RegisterKey("Exit", Keys.I);
            _backgroundEffekt = Game1.Instance.Content.Load<Effect>("effect/desaturateAndBlur");
            base.Initialize();
          
        }

        public override void LoadContent()
        {
            //own logic here
            (InputManager.Instance["Exit"] as Key).Pressed += new EventHandler(CloseInventory);

            //bind input
            base.LoadContent();
        }

        void CloseInventory(object sender, EventArgs e)
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
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            _drawManager.BeginBake(gameTime,SpriteSortMode.Immediate);
            _drawManager.BakeBeginEffect(_backgroundEffekt);
            _drawManager.Bake(_background);
            _drawManager.BakeEndEffect();
            _drawManager.Bake(Player.Instance.Inventory.Drawer);
            RenderTarget2D result = _drawManager.EndBake();
            _drawManager.Draw(result,gameTime);
        }
    }
}

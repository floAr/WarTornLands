using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WarTornLands.Infrastructure.ResolutionIndependence;
using Microsoft.Xna.Framework.Graphics;
using WarTornLands.Infrastructure.Systems.DrawSystem;
using WarTornLands.PlayerClasses;

namespace WarTornLands.Infrastructure
{
    class InventoryDrawer : IDrawProvider
    {
        Vector2 _crossPos = new Vector2(.5f, .3f);

        Vector2 _slotPos1;
        Vector2 _slotPos2;
        Vector2 _slotPos3;
        Vector2 _slotPos4;

        MagicSprite _itemCross;
        MagicSprite _selector;
        MagicSprite _potionBag;
        MagicSprite _telekinesis;
        MagicSprite _gun;
        MagicSprite _iceHammer;

        private Inventory _inventory;

        public InventoryDrawer(Inventory inventory)
        {
            _inventory = inventory;

            _itemCross = new MagicSprite(Game1.Instance.SpriteBatch, Game1.Instance.Content.Load<Texture2D>("sprite/cross"), _crossPos, null, .5f);

            float panning = .36f;
            _slotPos1 = new Vector2(_itemCross.Center.X, _itemCross.Center.Y - _itemCross.Height * panning);
            _slotPos2 = new Vector2(_itemCross.Center.X + _itemCross.Width * panning, _itemCross.Center.Y);
            _slotPos3 = new Vector2(_itemCross.Center.X, _itemCross.Center.Y + _itemCross.Height * panning);
            _slotPos4 = new Vector2(_itemCross.Center.X - _itemCross.Width * panning, _itemCross.Center.Y);

            _selector = new MagicSprite(Game1.Instance.SpriteBatch, Game1.Instance.Content.Load<Texture2D>("sprite/selector"), Vector2.Zero, panning * .4f, null);
            _selector.Position = _slotPos4;

            _potionBag = new MagicSprite(Game1.Instance.SpriteBatch, Game1.Instance.Content.Load<Texture2D>("sprite/potion"), Vector2.Zero, panning * .3f, null);
            _potionBag.Position = _slotPos4;
            _telekinesis = new MagicSprite(Game1.Instance.SpriteBatch, Game1.Instance.Content.Load<Texture2D>("sprite/telekinesis"), Vector2.Zero, panning * .3f, null);
            _telekinesis.Position = _slotPos2;
            _gun = new MagicSprite(Game1.Instance.SpriteBatch, Game1.Instance.Content.Load<Texture2D>("sprite/gun"), Vector2.Zero, panning * .5f, null);
            _gun.Position = _slotPos1;
            _iceHammer = new MagicSprite(Game1.Instance.SpriteBatch, Game1.Instance.Content.Load<Texture2D>("sprite/ice"), Vector2.Zero, panning * .3f, null);
            _iceHammer.Position = _slotPos3;
        }

        public void Draw(GameTime gameTime)
        {
            _itemCross.Draw();

            // Debug
            _inventory.HasPotionbag = true;
            _inventory.HasTelekinesis  = true;
            _inventory.HasIceHammer = true;
            _inventory.HasGun = true;

            if (_inventory.HasPotionbag) _potionBag.Draw();
            if (_inventory.HasTelekinesis) _telekinesis.Draw();
            if (_inventory.HasIceHammer) _iceHammer.Draw();
            if (_inventory.HasGun) _gun.Draw();
            
            _selector.Draw();
        }
    }
}

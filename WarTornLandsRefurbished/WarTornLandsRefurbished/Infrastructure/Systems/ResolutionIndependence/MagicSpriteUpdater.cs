using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace WarTornLands.Infrastructure.Systems.ResolutionIndependence
{
    class MagicSpriteUpdater
    {
        private static MagicSpriteUpdater _instance;
        public static MagicSpriteUpdater Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MagicSpriteUpdater();

                return _instance;
            }
        }

        private GraphicsDevice _graphics;

        public float Width { get; private set; }
        public float Height { get; private set; }
        public event EventHandler<ResolutionEventArgs> OnResolutionChanged;

        private MagicSpriteUpdater()
        {
            _graphics = Game1.Instance.GraphicsDevice;

            SnagResolution();
        }

        private void SnagResolution()
        {
            Width = _graphics.Viewport.Width;
            Height = _graphics.Viewport.Height;
        }

        public void UpdateRes()
        {
            SnagResolution();
        }
    }
}

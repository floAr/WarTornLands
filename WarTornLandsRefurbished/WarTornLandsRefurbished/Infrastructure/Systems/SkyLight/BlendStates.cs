using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WarTornLands.Infrastructure.Systems.SkyLight
{
    public class CustomBlendState
    {
        public static BlendState ReverseSubtract
        {
            get
            {
                BlendState CustomBlenderState = new BlendState();
                var _with1 = CustomBlenderState;
                _with1.AlphaSourceBlend = Blend.One;
                _with1.ColorSourceBlend = Blend.One;
                _with1.AlphaDestinationBlend = Blend.One;
                _with1.ColorDestinationBlend = Blend.One;
                _with1.AlphaBlendFunction = BlendFunction.ReverseSubtract;
                _with1.ColorBlendFunction = BlendFunction.ReverseSubtract;
                return CustomBlenderState;
            }
        }

        public static BlendState Subtract
        {
            get
            {
                BlendState CustomBlenderState = new BlendState();
                var _with2 = CustomBlenderState;
                _with2.AlphaSourceBlend = Blend.One;
                _with2.ColorSourceBlend = Blend.One;
                _with2.AlphaDestinationBlend = Blend.One;
                _with2.ColorDestinationBlend = Blend.One;
                _with2.AlphaBlendFunction = BlendFunction.Subtract;
                _with2.ColorBlendFunction = BlendFunction.Subtract;
                return CustomBlenderState;
            }
        }

        public static BlendState Add
        {
            get
            {
                BlendState CustomBlenderState = new BlendState();
                var _with3 = CustomBlenderState;
                _with3.AlphaSourceBlend = Blend.One;
                _with3.ColorSourceBlend = Blend.One;
                _with3.AlphaDestinationBlend = Blend.One;
                _with3.ColorDestinationBlend = Blend.One;
                _with3.AlphaBlendFunction = BlendFunction.Add;
                _with3.ColorBlendFunction = BlendFunction.Add;
                return CustomBlenderState;
            }
        }
    }

    public class BackBuffer
    {
        private RenderTarget2D _SourcetMap;

        private RenderTarget2D _LightMap;
        public RenderTarget2D SourceMap
        {
            get { return _SourcetMap; }
            set { _SourcetMap = value; }
        }
        public RenderTarget2D LightMap
        {
            get { return _LightMap; }
            set { _LightMap = value; }
        }

        public BackBuffer(GraphicsDevice Device, Rectangle Size)
        {
            _SourcetMap = new RenderTarget2D(Device, Size.Width, Size.Height, false, Device.DisplayMode.Format, DepthFormat.Depth24);
            _LightMap = new RenderTarget2D(Device, Size.Width, Size.Height, false, Device.DisplayMode.Format, DepthFormat.Depth24);
        }
    }

}

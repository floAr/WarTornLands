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
        private RenderTarget2D _SourceMap;
        private RenderTarget2D _lowerLightMap;
        private RenderTarget2D _upperLightMap;

        private RenderTarget2D _lastFrame;

        public RenderTarget2D SourceMap
        {
            get { return _SourceMap; }
            set { _SourceMap = value; }
        }
        public RenderTarget2D UpperLightMap
        {
            get { return _upperLightMap; }
            set { _upperLightMap = value; }
        }
        public RenderTarget2D LowerLightMap
        {
            get { return _lowerLightMap; }
            set { _lowerLightMap = value; }
        }

        public RenderTarget2D LastFrame
        {
            get { return _lastFrame; }
            set { _lastFrame = value; }
        }

        public BackBuffer(GraphicsDevice Device, Rectangle Size)
        {
            _SourceMap = new RenderTarget2D(Device, Size.Width, Size.Height, false, Device.DisplayMode.Format, DepthFormat.Depth24);
            _upperLightMap = new RenderTarget2D(Device, Size.Width, Size.Height, false, Device.DisplayMode.Format, DepthFormat.Depth24);
            _lowerLightMap = new RenderTarget2D(Device, Size.Width, Size.Height, false, Device.DisplayMode.Format, DepthFormat.Depth24);
            _lastFrame = new RenderTarget2D(Device, Size.Width, Size.Height, false, Device.DisplayMode.Format, DepthFormat.Depth24);
        }
    }

}

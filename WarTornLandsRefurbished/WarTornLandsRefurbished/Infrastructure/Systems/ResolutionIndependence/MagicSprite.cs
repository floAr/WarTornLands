using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WarTornLands.Infrastructure.ResolutionIndependence
{
    /// <summary>
    /// A magic sprite is a texture with a fix position on the screen, beeing drawn in the right spot depending on the current solution.
    /// It is not affected by the camera.
    /// </summary>
    class MagicSprite
    {
        /// <summary>
        /// The position relative to the screen resolution.
        /// Coordinates ranging from 0 to 1
        /// </summary>
        private Vector2 _relPosition;
        /// <summary>
        /// In case a precise pixel position is desired use the Position setter to override the relative routine.
        /// </summary>
        private Vector2? _overridePos = null;
        /// <summary>
        /// The proportions relative to the screen resolution.
        /// Coordinates ranging from 0 to 1
        /// </summary>
        private float? _relWidth;
        private float? _relHeight;
        /// <summary>
        /// The texture
        /// </summary>
        private Texture2D _sprite;
        /// <summary>
        /// The sprite is panned by the value of this vector relative to its dimensions.
        /// A panning of (0,0) means the sprite is drawn centered around the given _relPosition.
        /// A dimension going towards 1 means a pan of the sprite in that direction.
        /// 
        /// Examples:
        /// (-1,-1) = The sprite is drawn with the given _relPosition beeing the topleft corner.
        /// (1,1) = The _relPosition is the bottom right corner
        /// (-.5,0) = The _relPosition is centered in the Y-dimension and half the way from the right side to the center in the X-dimension
        /// </summary>
        private Vector2 _panning;
        private SpriteBatch _sb;
        private GraphicsDevice _grahpics;
        private Rectangle _rec;

        /// <summary>
        /// Gets the actual top-left postion in pixels.
        /// Sets the actual position and then calculates the new drawing rectangle. Still dependend on panning
        /// </summary>
        /// <returns></returns>
        public Vector2 Position
        {
            get {
                return new Vector2(_rec.X, _rec.Y); 
            }
            set {
                _overridePos = value;
                GetDrawingRec();
            }
        }

        /// <summary>
        /// Gets the actual center position in pixel.
        /// </summary>
        /// <value>
        /// The center.
        /// </value>
        public Vector2 Center
        {
            get
            {
                return new Vector2(_rec.X + Width * .5f, _rec.Y + Height * .5f);
            }
        }

        /// <summary>
        /// Gets the actual pixel width.
        /// </summary>
        /// <returns></returns>
        public int Width
        {
            get {
                return _rec.Width;
            }
        }

        /// <summary>
        /// Gets the actual pixel height.
        /// </summary>
        /// <returns></returns>
        public int Height
        {
            get {
                return _rec.Height;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MagicSprite" /> class.
        /// It is possible to pass null for either relWidth or relHeight. 
        /// In this case the respective other value is calculated in respect to the sprites aspect ratio to ensure a non-warped presentation.
        /// </summary>
        /// <param name="sb">The SpriteBatch used to draw the sprite.</param>
        /// <param name="sprite">The sprite texture.</param>
        /// <param name="relPosition">The relative position.</param>
        /// <param name="relWidth">The relative width.</param>
        /// <param name="relHeight">The relative height.</param>
        /// <param name="Xpan">The pan in X-direction.</param>
        /// <param name="Ypan">The pan in Y-direction.</param>
        /// <exception cref="System.Exception">Sprite dimensions not specified. Called in case both width and height are passed as null.</exception>
        public MagicSprite(SpriteBatch sb, Texture2D sprite, Vector2 relPosition, float? relWidth, float? relHeight, float Xpan = 0, float Ypan = 0)
        {
            if (relWidth == null && relHeight == null)
                throw new Exception("Sprite dimensions not specified.");

            _sb = sb;
            _sprite = sprite;
            _relPosition = relPosition;
            _relWidth = relWidth;
            _relHeight = relHeight;
            _panning = new Vector2(Xpan, Ypan);
            _grahpics = Game1.Instance.GraphicsDevice;

            GetDrawingRec();
        }

        public void Draw()
        {
            if (_rec.IsEmpty)
                GetDrawingRec();

            _sb.Draw(_sprite, _rec, Color.White);
        }

        private void GetDrawingRec()
        {
            int portWidth = _grahpics.Viewport.Width;
            int portHeight = _grahpics.Viewport.Height;

            #region Case both width and height are specified

            if (_relWidth != null && _relHeight != null)
            {
                int width = (int)(portWidth * _relWidth);
                int height = (int)(portHeight * _relHeight);
                int x = (int)(portWidth * _relPosition.X - (float)width * (.5f + .5f * _panning.X));
                int y = (int)(portHeight * _relPosition.Y - (float)height * (.5f + .5f * _panning.Y));

                _rec = new Rectangle(
                   x,
                   y,
                   width,
                   height);

                CheckOverride();

                return;
            }
            #endregion

            #region Case only width is specified

            if (_relWidth != null && _relHeight == null)
            {
                float spriteAspectRatio = (float)_sprite.Height / (float)_sprite.Width;

                int width = (int)(portWidth * _relWidth);
                int height = (int)(portWidth * _relWidth * spriteAspectRatio);
                int x = (int)(portWidth * _relPosition.X - (float)width * (.5f + .5f * _panning.X));
                int y = (int)(portHeight * _relPosition.Y - (float)height * (.5f + .5f * _panning.Y));

                _rec = new Rectangle(
                    x,
                    y,
                    width,
                    height);

                CheckOverride();

                return;
            }
            #endregion

            #region Case only height is specified

            if (_relWidth == null && _relHeight != null)
            {
                float spriteAspectRatio = _sprite.Width / _sprite.Height;

                int width = (int)(portHeight * _relHeight * spriteAspectRatio);
                int height = (int)(portHeight * _relHeight);
                int x = (int)(portWidth * _relPosition.X - (float)width * (.5f + .5f * _panning.X));
                int y = (int)(portHeight * _relPosition.Y - (float)height * (.5f + .5f * _panning.Y));

                _rec = new Rectangle(
                    x,
                    y,
                    width,
                    height);

                CheckOverride();

                return;
            }
            #endregion

            throw new Exception("Sprite dimensions not specified.");
        }

        private void CheckOverride()
        {
            if (_overridePos != null)
            {
                int portWidth = _grahpics.Viewport.Width;
                int portHeight = _grahpics.Viewport.Height;

                _rec.X = (int)(_overridePos.Value.X - _rec.Width * (.5f + .5f * _panning.X));
                _rec.Y = (int)(_overridePos.Value.Y - _rec.Height * (.5f + .5f * _panning.Y));
            }
        }
    }
}

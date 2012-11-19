using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WarTornLands.Entities.Modules.Draw
{
    /// <summary>
    /// Structure which holds all the information to locate a sprite on the screen
    /// </summary>
    public struct DrawInformation
    {
        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Vector2 Position { get; set; }
        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        /// <value>
        /// The scale.
        /// </value>
        public Vector2 Scale { get; set; }
        /// <summary>
        /// Gets or sets the rotation.
        /// </summary>
        /// <value>
        /// The rotation.
        /// </value>
        public float Rotation { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this sprite is drawn at its center.
        /// </summary>
        /// <value>
        ///   <c>true</c> if centered; otherwise, <c>false</c>.
        /// </value>
        public bool Centered { get; set; }
    }
    /// <summary>
    /// Interface for the class which executes the draw.
    /// Implemented in AnimationSystem and StaticDrawer
    /// </summary>
  public  interface IDrawExecuter
    {
         void Draw(SpriteBatch batch, DrawInformation information);

         Vector2 Size { get; }
    }
}

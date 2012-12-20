using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WarTornLands.Infrastructure.Systems.Camera2D
{
    public interface ISpatial
    {
        Vector2 Position { get; set; }
        Rectangle BoundingRect { get; }
        bool ToBeRemoved { get; set; }
    }
}

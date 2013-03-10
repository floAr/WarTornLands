using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization;

namespace WarTornLands.Infrastructure.Systems.SaveLoad
{
   public class SaveLoadHelper
    {
       public static void SaveRectangle(ref SerializationInfo info, Rectangle rect,string name)
       {
           info.AddValue(name + "X", rect.X);
           info.AddValue(name + "Y", rect.Y);
           info.AddValue(name + "W", rect.Width);
           info.AddValue(name + "H", rect.Height);
       }
       public static Rectangle LoadRectangle(ref SerializationInfo info, string name)
       {
           int x = info.GetInt32(name + "X");
           int y = info.GetInt32(name + "Y");
           int w = info.GetInt32(name + "W");
           int h = info.GetInt32(name + "H");
           return new Rectangle(x, y, w, h);
       }
    }
}

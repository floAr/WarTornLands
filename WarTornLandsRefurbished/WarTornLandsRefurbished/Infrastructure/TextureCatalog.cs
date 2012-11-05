using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace WarTornLands.Infrastructure
{
    static class TextureCatalog
    {
        public static Texture2D DeadTree { get; private set; }
        public static Texture2D Potion { get; private set; }

        private static ContentManager _cm;

        public static void LoadContent(ContentManager cm)
        {
            _cm = cm;

            DeadTree = Load("deadtree");
            Potion = Load("potion");


            _cm = null;
        }

        private static Texture2D Load(string fileName)
        {
            string[] files = Directory.GetFiles(_cm.RootDirectory, fileName +".*");

            if (files.Count() == 0)
                throw new Exception("Texture "+ fileName +" not found.");
            if (files.Count() > 1)
                throw new Exception("Texture "+ fileName +" is present multiple times.");

            return _cm.Load<Texture2D>(files[0]);
        }
    }
}

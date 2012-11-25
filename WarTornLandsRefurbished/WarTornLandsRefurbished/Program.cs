using System;
using WarTornLands;

namespace WarTornLands
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        static void Main(string[] args)
        {
            using (Game1 game = Game1.Instance)
            {
                game.Run();
            }
        }
    }
#endif
}


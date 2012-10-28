using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarTornLands
{
    static class Constants
    {
        public static int TileSize = 64;
        
        // Player
        public const int Player_Radius = 32;
        public const float Player_Speed = .125f;
        public const int Player_HitDuration = 400;
        public const float Player_WeaponRange = 50;
        public const float Player_WeaponStartAngle = 1.3f;
        public const float Player_WeaponGoalAngle = (float)(Math.PI * 2 - Player_WeaponStartAngle);
        public const float Player_TalkDistance = 48.0f;


        // GruselUte
        public const float GruselUte_SightRange = 300;
        public const float GruselUte_HitRange = 30;
        public const float GruselUte_Speed = .05f;
        public const int GruselUte_Health = 100;
        public const int GruselUte_Radius = 16;
    }
}

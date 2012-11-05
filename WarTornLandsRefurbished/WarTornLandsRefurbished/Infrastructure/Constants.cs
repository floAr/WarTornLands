using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace WarTornLands
{
    static class Constants
    {
        public static int TileSize = 64;
        
        // Default controls ///

        // Keyboard
        public readonly static Keys KeyboardDefault_Hit          = Keys.O;
        public readonly static Keys KeyboardDefault_Jump         = Keys.Space;
        public readonly static Keys KeyboardDefault_Interact     = Keys.T;
        public readonly static Keys KeyboardDefault_UsePotion    = Keys.P;
        public readonly static Keys[] KeyboardDefault_Move       = { Keys.W, Keys.A, Keys.S, Keys.D };

        // GamePad

        public const float GamePadTStickThreshold = .1f;
            

        ///////////////////////

        // Player ///

        // Movement
        public const int Player_Radius = 32;
        public const float Player_Speed = .125f;
        public const int Player_HitDuration = 400;
        public const float Player_WeaponRange = 50;
        public const float Player_WeaponStartAngle = 1.3f;
        public const float Player_WeaponGoalAngle = (float)(Math.PI * 2 - Player_WeaponStartAngle);
        public const float Player_TalkDistance = 48.0f;

        // Inventory
        public const int Inventory_MaxPotions = 5;







        /////////////


        // NPCs ////////////////

        // GruselUte
        public const float GruselUte_SightRange = 300;
        public const float GruselUte_HitRange = 30;
        public const float GruselUte_Speed = .05f;
        public const int GruselUte_Health = 100;
        public const int GruselUte_Radius = 16;



        ////////////////////////
    }
}

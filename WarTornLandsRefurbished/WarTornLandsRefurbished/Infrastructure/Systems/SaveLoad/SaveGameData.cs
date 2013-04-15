using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WarTornLands.World;

namespace WarTornLands.Infrastructure.Systems.SaveLoad
{

   public class SaveGameData
    {
       public SaveGameData()
       {
           CheckpointPosition = GlobalState.CheckpointLocation;
           Triggers = GlobalState.Triggers;
           ValuesS = GlobalState.Values.Keys.ToList<string>();
           ValuesO = GlobalState.Values.Values.ToList<object>();
       }

       
       //Player

       //GlobalState
       public Vector2 CheckpointPosition;
       public List<String> Triggers;
       public List<string> ValuesS;
       public List<object> ValuesO;
    }

}

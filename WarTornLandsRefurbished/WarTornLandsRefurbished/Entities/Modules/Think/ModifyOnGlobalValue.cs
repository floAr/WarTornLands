using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.World;

namespace WarTornLands.Entities.Modules.Think
{
    class ModifyOnGlobalValue<T>:BaseModule,IThinkModule where T:IEquatable<T>
    {
        private List<BaseModule> _newModules = new List<BaseModule>();
        private String _counter;
        private T _targetValue;

        public void AddModule(BaseModule module)
        {
            _newModules.Add(module);
        }
        public ModifyOnGlobalValue(String targetCounter, T targetValue,List<BaseModule> newModules=null)
        {
            _counter = targetCounter;
            _targetValue = targetValue;
            if (newModules != null)
                _newModules = newModules;
        }
      
        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_targetValue.Equals((T)GlobalState.GetValue<T>(_counter)))
            {
                foreach (BaseModule module in _newModules)
                    _owner.AddModule(module);
            }
        }

        public void SetZone(AI.Zone zone)
        {
            return;
        }
    }
}

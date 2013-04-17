using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.World;

namespace WarTornLands.Entities.Modules.Die
{
    class IncreaseCounterOnDeath:BaseModule,IDieModule
    {
        private string _counter;
        private IDieModule _followUpModule;

        public IncreaseCounterOnDeath(String counterIdentifier,IDieModule dieModule)
        {
            _followUpModule = dieModule;
            _counter = "counter_" + counterIdentifier;
            GlobalState.RegisterValue<int>(_counter);
        }
        public void Die()
        {
            int currentValue = GlobalState.GetValue<int>(_counter);
            GlobalState.SetValue<int>(_counter, currentValue++);
            _followUpModule.Die();
        }
    }
}

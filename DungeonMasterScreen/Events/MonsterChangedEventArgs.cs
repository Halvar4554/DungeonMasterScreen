using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMasterScreen.Events
{
    public abstract class MonsterChangedEventArgs:EventArgs
    {
        public string Name { get; protected set; }

    }
}

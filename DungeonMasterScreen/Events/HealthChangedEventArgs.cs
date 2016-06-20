using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMasterScreen.Events
{
    class HealthChangedEventArgs:MonsterChangedEventArgs
    {

        public int Original { get; private set; }
        public int Updated { get; private set; }

        public HealthChangedEventArgs(string name, int original, int updated) {
            Name = name;
            Original = original;
            Updated = updated;
        }
    }
}

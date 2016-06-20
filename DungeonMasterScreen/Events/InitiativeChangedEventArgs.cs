using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMasterScreen.Events
{
    class InitiativeChangedEventArgs:MonsterChangedEventArgs
    {
        public int Initiative { get;private set; }

        public InitiativeChangedEventArgs(string name,int initiative)
        {
            Initiative = initiative;
            Name = name;
        }
    }
}

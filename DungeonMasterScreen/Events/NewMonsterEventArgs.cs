using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMasterScreen.Events
{
    class NewMonsterEventArgs
    {
        public int NewMonsterCount { get; set; }

        public NewMonsterEventArgs(int newCount)
        {
            NewMonsterCount = newCount;
        }
    }
}

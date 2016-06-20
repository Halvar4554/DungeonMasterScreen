using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMasterScreen.Events
{
    /// <summary>
    /// This class carries counts of monsters in <see cref="DungeonMasterScreen.Controller.MonsterCave"/> when their count was changed
    /// </summary>
    public class MonsterCountChangedEventArgs : EventArgs
    {
        public int OldCount { get; private set; }
        public int NewCount { get; private set; }

        public MonsterCountChangedEventArgs(int oldCount, int newCount){
            this.OldCount = oldCount;
            this.NewCount = newCount;
        }
    }
}

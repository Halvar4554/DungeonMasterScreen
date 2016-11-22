using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMasterScreen.Events
{
    public class BattleLogEventArgs
    {
        public string Message { get; private set; }

        public BattleLogEventArgs(string message)
        {
            Message = message;
        }
    }
}

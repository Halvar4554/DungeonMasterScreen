using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMasterScreen.Events
{
    public class NewTurnEventArgs : EventArgs
    {
        public int Turn { get; private set; }

        public NewTurnEventArgs(int turn)
        {
            Turn = turn;
        }
    }
}

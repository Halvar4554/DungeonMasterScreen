using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMasterScreen.Exceptions
{
    public class MonsterManualOpenException:InvalidOperationException
    {
        public MonsterManualOpenException(string message) : base(message) { }
    }
}

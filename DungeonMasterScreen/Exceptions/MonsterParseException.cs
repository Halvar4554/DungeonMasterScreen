using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMasterScreen.Exceptions
{
    class MonsterParseException:ArgumentException
    {
        public MonsterParseException():base() { }

        public MonsterParseException(string message) : base(message) { }

        public MonsterParseException(string message,Exception inner):base(message,inner) { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterScreen.Model;

namespace DungeonMasterScreen.Comparers
{
    class MonsterInitiativeComparator : Comparer<Monster>
    {
        public override int Compare(Monster x, Monster y)
        {
            if (x.Initiative<y.Initiative)
            {
                return 1;
            }
            else if (x.Initiative==y.Initiative)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonMasterScreen.Model;

namespace DungeonMasterScreen.Controller
{
    /// <summary>
    /// This class is responsible to hold and distribute one instance of <see cref="MonsterCave"/>.
    /// All acces to <see cref="MonsterCave"/> must be done trough this class
    /// </summary>
    class MonsterCaveFactory
    {
        private static MonsterCave monsterCave;
        private static List<Monster> reserveMonsters = new List<Monster>();

        private MonsterCaveFactory() {}

        public static void InitFactory(List<Monster> reserveMonsters) {
            monsterCave = MonsterCave.GetInstance(reserveMonsters);
        }

        public static MonsterCave getMonsterCaveInstance()
        {
            return monsterCave;
        }
    }
}

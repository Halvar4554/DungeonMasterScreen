using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMasterScreen.Core
{
    public class Constants
    {
        public const char DELIMITER = ';';
        public const int NAME_INDEX = 0;
        public const int INITIATIVE_INDEX = 1;
        public const int LIVES_INDEX = 2;
        public const int ATTACKS_INDEX = 3;
        public const int DAMAGE_INDEX = 4;
        public const int DEFENSE_INDEX = 5;
        public const int EFFECTS_INDEX = 6;
        public const int NUMBER_OF_ATTRIBUTES = 7;
        public const string MONSTER_MANUAL_FILE = "Files/monstermanual.txt";
        public const string MONSTER_MANUAL_FORMAT = "b";
        public const string MONSTER_FILE_FORMAT = "m";
        public const string FIGHT_FILE_FORMAT = "s";
        public const int FIGHT_FORMAT_INDEX = 0;
        public const int MONSTER_COUNT_INDEX = 1;
        public const int ACTUAL_TURN_INDEX = 2;
        public const int ACTUAL_COMBATANT_INDEX = 3;
        public const int FIGHT_FORMAT_PARAMS_COUNT = 4;
        public const string FILE_FILTER = "Text files (*.txt)|*.txt";
    }
}

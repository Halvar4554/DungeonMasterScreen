using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMasterScreen.Model
{
    /// <summary>
    /// Helper class for carrying data about encounter
    /// </summary>
    public class EncounterCarrier
    {

        public EncounterCarrier() {
            this.Monsters = new List<Monster>();
            this.BattleLog = new List<string>();
            this.ActualCombatant = 0;
        }

        public int ActualCombatant { get; set; }

        public int ActualTurn { get; set; }

        public List<Monster> Monsters { get; set; }

        public List<string> BattleLog { get; set; }
    }
}

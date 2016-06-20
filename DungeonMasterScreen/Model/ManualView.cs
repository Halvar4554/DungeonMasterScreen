using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMasterScreen.Model
{
    class ManualView
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Health { get; set; }
        public string Attack { get; set; }
        public string Defense { get; set; }

        public ManualView(int id, string name, string health, string attack, string defense) {
            this.Id = id;
            this.Name = name;
            this.Health = health;
            this.Attack = attack;
            this.Defense = defense;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMasterScreen.Model
{
    class MonsterView
    {
        public string Initiative { get; set; }
        public string Name { get; set; }
        public string Health { get; set; }
        public int Id { get; }

        public MonsterView(int id,string initiative,string name, string health)
        {
            this.Id = id;
            this.Initiative = initiative;
            this.Name = name;
            this.Health = health;
        }
    }
}

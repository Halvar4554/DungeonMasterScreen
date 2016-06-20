using DungeonMasterScreen.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMasterScreen.Model
{
    /// <summary>
    /// Is DTO for easier handling monster attributes on gui. MonsterDto are not stored, it is simple transport object.
    /// </summary>
    class MonsterDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string initiative { get; set; }
        public string lifes { get; set; }
        public string attackBonusess { get; set; }
        public string damage { get; set; }
        public string defense { get; set; }
        public string effects { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(name).Append(Constants.DELIMITER);
            builder.Append(initiative).Append(Constants.DELIMITER);
            builder.Append(lifes).Append(Constants.DELIMITER);
            builder.Append(attackBonusess).Append(Constants.DELIMITER);
            builder.Append(damage).Append(Constants.DELIMITER);
            builder.Append(effects).Append(Constants.DELIMITER);
            return builder.ToString();
        }
    }
}

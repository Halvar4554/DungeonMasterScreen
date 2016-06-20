using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMasterScreen.Core
{
    /// <summary>
    /// This is Singleton implemented generator for ID's for monsters. ID are not stored so for each instance of DMScreen the ID are generated again.
    /// The <see cref="Model.Monster.Id"/> is simple mechanism for identifiying monsters.  Because <see cref="Model.Monster.Name"/> and <see cref="Model.Monster.Initiative"/> may be same
    /// for lot of monsters of same type.
    /// </summary>
    class IdentityGenerator
    {
        private static int lasGeneratedId = 0;

        private static IdentityGenerator generator;

        private IdentityGenerator() { }

        private static int generateNewId()
        {
            return lasGeneratedId++;
        }

        public static int GetNewId() {
            if (generator == null) {
                generator = new IdentityGenerator();
            }
            return generateNewId();
        }
    }
}

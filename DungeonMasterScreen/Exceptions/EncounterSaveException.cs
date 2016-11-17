using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMasterScreen.Exceptions
{
    public class EncounterSaveException:ArgumentException
    {
        public string ErrorMessage { get; private set; }

        public EncounterSaveException(string message) {
            ErrorMessage = message;
        }
    }
}

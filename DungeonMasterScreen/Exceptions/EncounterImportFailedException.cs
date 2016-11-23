using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;
using DungeonMasterScreen.Properties;

namespace DungeonMasterScreen.Exceptions
{
    /// <summary>
    /// Výjimka, která je vyhozena, jestliže se nepovede import souboje.
    /// </summary>
    public class EncounterImportFailedException:InvalidOperationException
    {
   
        public EncounterImportFailedException(string message) : base(string.Format(Resources.EIFE_ERROR_MESSAGE,message))
        {
           
        }
    }
}

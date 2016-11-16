using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMasterScreen.Exceptions
{
    /// <summary>
    /// This exception is thrown when is not possible to parse parameters from first line in encounter file.
    /// </summary>
    public class FightParamParsingException:ArgumentException
    {

        public string ErrorMessage { get; private set; }

        public FightParamParsingException(string message) {
            this.ErrorMessage = message;
        }
    }
}

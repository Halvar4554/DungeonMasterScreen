using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMasterScreen.Exceptions
{
    class ValidationException:ArgumentException
    {
        public ValidationException() : base() { }

        public ValidationException(string message) : base(message) { }

        public ValidationException(string message,Exception innerException) : base(message, innerException) { }
    }
}

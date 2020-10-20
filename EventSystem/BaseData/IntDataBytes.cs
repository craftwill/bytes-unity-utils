using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bytes
{
    class IntDataBytes : Bytes.Data
    {
        public IntDataBytes(int intValue) { IntValue = intValue; }
        public int IntValue { get; private set; }
    }
}

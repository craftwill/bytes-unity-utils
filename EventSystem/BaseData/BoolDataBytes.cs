using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bytes
{
    class BoolDataBytes : Bytes.Data
    {
        public BoolDataBytes(bool boolValue) { BoolValue = boolValue; }
        public bool BoolValue { get; private set; }
    }
}

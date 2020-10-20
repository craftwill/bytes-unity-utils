using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bytes
{
    class StringDataBytes : Bytes.Data
    {
        public StringDataBytes(string stringValue) { StringValue = stringValue; }
        public string StringValue { get; private set; }
    }
}

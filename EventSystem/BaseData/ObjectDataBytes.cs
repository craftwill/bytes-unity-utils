using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bytes
{
    class ObjectDataBytes : Bytes.Data
    {
        public ObjectDataBytes(object objectParam) { ObjectValue = objectParam; }
        public object ObjectValue { get; private set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bytes
{
    class ProgressionDataBytes : Bytes.Data
    {
        public ProgressionDataBytes(int current, int max) { Current = current; Max = max; }
        public int Current { get; private set; }
        public int Max { get; private set; }
    }
}

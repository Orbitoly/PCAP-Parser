using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcapParser.Protocols
{
    abstract class Protocol
    {
        protected abstract void Parse(byte[] rawData);

    }
}

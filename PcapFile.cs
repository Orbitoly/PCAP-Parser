using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PcapParser
{
    public class PcapFile
    {
        PcapHeader _header;
        public PcapFile(PcapHeader header)
        {
            _header = header;
        }

        public PcapFile(PcapHeader header, List<PcapPacket> packets)
        {
            
        }
    }
}

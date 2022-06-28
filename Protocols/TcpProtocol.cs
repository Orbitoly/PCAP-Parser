using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PcapParser.Interfaces;

namespace PcapParser.Protocols
{
    class TcpProtocol:Protocol,ITransportDataLayer
    {
        public IInternetLayer LowerLayer { get; set; }
        public RawPacketData WrappedProtocolRaw { get; }
        protected override void Parse(byte[] rawData)
        {

        }
    }
}

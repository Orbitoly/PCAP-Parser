using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PcapParser.Protocols;

namespace PcapParser
{
    public class RawPacketData
    {
        public byte[] RawBytes { get; }
        public ProtocolEnum ProtocolName { get; }
        public RawPacketData(byte[] rawData,ProtocolEnum protocol)
        {
            RawBytes = rawData;
            ProtocolName = protocol;
        }
    }
    public enum ProtocolEnum { NULL,UNKNOWN,IPv4,ARP,IPv6,Ethernet,DNS,DHCP,TCP,UDP,HTTP,HTTPS,ICMP}
}

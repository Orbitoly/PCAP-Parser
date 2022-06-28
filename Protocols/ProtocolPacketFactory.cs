using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcapParser.Protocols
{
    static class ProtocolPacketFactory
    {
        public static ProtocolPacket BuildProtocolPacket(ProtocolEnum name,byte[] data)
        {
            switch (name)
            {
                
                case ProtocolEnum.NULL:
                    return null;
                    break;
                case ProtocolEnum.UNKNOWN:
                    break;
                case ProtocolEnum.IPv4:
                    return new IPv4Packet(data);
                case ProtocolEnum.ARP:
                    break;
                case ProtocolEnum.IPv6:
                    break;
                case ProtocolEnum.Ethernet:
                    return new EthernetPacket(data);
                case ProtocolEnum.DNS:
                    break;
                case ProtocolEnum.DHCP:
                    break;
                case ProtocolEnum.TCP:
                    return new TcpPacket(data);
                case ProtocolEnum.UDP:
                    break;
                case ProtocolEnum.HTTP:
                    return new HttpPacket(data);
                case ProtocolEnum.HTTPS:
                    break;
                case ProtocolEnum.ICMP:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(name), name, null);
            }
            throw new NotImplementedException();
        }

        public static ProtocolPacket BuildProtocolPacket(RawPacketData data)
        {
            return BuildProtocolPacket(data.ProtocolName, data.RawBytes);
        }
    }
}

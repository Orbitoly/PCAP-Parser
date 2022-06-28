using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using PcapParser.Interfaces;
using PcapParser.Protocols;

namespace PcapParser
{
    class EthernetProtocol:Protocol,ILinkDataLayer
    {
        private const int StartIndexDestMac = 0;
        private const int StartIndexSrcMac = 6;
        private const int MacLen = 6;
        private const int StartIndexType = 12;
        private const int TypeLen = 2;
        private const int Size = 14;
        public RawPacketData WrappedProtocolRaw { get; private set; }

        public PhysicalAddress DestinationAddress { get; private set; }
        public PhysicalAddress SourceAddress { get; private set; }
        public EthernetProtocol(RawPacketData rawPacket)
        {
            Parse(rawPacket.RawBytes);
        }

        protected override void Parse(byte[] rawData)
        {
            
            Console.WriteLine("This is Ethernet!");
            byte[] destAddr=null;
            //  |dstMAC(6B) xx:xx:xx:xx:xx:xx|srcMAC(6B) xx:xx:xx:xx:xx:xx|etherType(2B) xxxx|...DATA...|Pading|
            DestinationAddress = new PhysicalAddress(rawData.ToList().GetRange(StartIndexDestMac, MacLen).ToArray());
            DestinationAddress=new PhysicalAddress(BitsOperations.ReadBytesRange(rawData,StartIndexDestMac,MacLen));
            SourceAddress = new PhysicalAddress(rawData.ToList().GetRange(StartIndexSrcMac, MacLen).ToArray());
            string type=BitConverter.ToString(rawData.ToList().GetRange(StartIndexType, TypeLen).ToArray());

            WrappedProtocolRaw = new RawPacketData(rawData.ToList().GetRange(Size, rawData.Length-Size).ToArray(), ParseProtocolName(type));
        }

        private ProtocolEnum ParseProtocolName(string name)
        {
            var protName=ProtocolEnum.NULL;
            switch (name)
            {
                case "08-00":
                    protName=ProtocolEnum.IPv4;
                    break;
                case "86-DD":
                    protName = ProtocolEnum.IPv6;
                    break;
                case "08-06":
                    protName = ProtocolEnum.ARP;
                    break;
            }
            return protName;
        }
    }
}

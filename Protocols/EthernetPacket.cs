using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using static PcapParser.BitsOperations;
namespace PcapParser.Protocols
{
    class EthernetPacket : ProtocolPacket
    {

        public PhysicalAddress DestinationAddress { get; private set; }
        public PhysicalAddress SourceAddress { get; private set; }

        public EthernetPacket(byte[] data) : base(data)
        {
        }


        protected override RawPacketData Parse(byte[] data)
        {

            //  |dstMAC(6B) xx:xx:xx:xx:xx:xx|srcMAC(6B) xx:xx:xx:xx:xx:xx|etherType(2B) xxxx|...DATA...|Pading|
            //------------------------------------------------------------------------------------------------------------------------------------//

            var dataList = data.ToList();

            //  |dstMAC(6B) xx:xx:xx:xx:xx:xx |
            DestinationAddress = new PhysicalAddress(BitsOperations.ReadBytesRange(dataList, (int)FieldSizeBits.DestinationAddress/BYTE_LEN));
            dataList = dataList.Skip(((int)FieldSizeBits.DestinationAddress) / BYTE_LEN).ToList();

            //  |srcMAC(6B) xx:xx:xx:xx:xx:xx |
            SourceAddress = new PhysicalAddress(BitsOperations.ReadBytesRange(dataList, (int)FieldSizeBits.SourceAddress / BYTE_LEN));
            dataList = dataList.Skip(((int)FieldSizeBits.SourceAddress) / BYTE_LEN).ToList();

            //  |etherType(2B) xxxx|
            string type = BitConverter.ToString(BitsOperations.ReadBytesRange(dataList, (int)FieldSizeBits.Type / BYTE_LEN));
            dataList = dataList.Skip(((int)FieldSizeBits.Type) / BYTE_LEN).ToList();

            return new RawPacketData( dataList.ToArray(), ParseProtocolName(type));
            
        }

        public override bool Valid { get; set; }

        protected override List<ProtocolEnum> GetSupportedPorotocls()
        {
            return new List<ProtocolEnum>() {ProtocolEnum.IPv4, ProtocolEnum.IPv6, ProtocolEnum.ARP};
        }

        private ProtocolEnum ParseProtocolName(string name)
        {
            var protName = ProtocolEnum.NULL;
            switch (name)
            {
                case "08-00":
                    protName = ProtocolEnum.IPv4;
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
        internal enum FieldSizeBits : int
        {
            DestinationAddress =48,
            SourceAddress = 48,
            Type = 16,
        }

        public override string GetAttributes()
        {
            var str = "<Ethernet>\n"+
                      "Destination Address: " +  DestinationAddress.ToString() + 
                      "\nSource Address: "    +  SourceAddress.ToString() + "\n\n";
            return str;
        }
    }
}
